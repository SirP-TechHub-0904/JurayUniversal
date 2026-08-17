using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Pages.Pay
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class CheckoutModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly PixelPayService _pixelPayService;
        private readonly PaystackService _paystackService;

        public CheckoutModel(
            DashboardDbContext context,
            UserManager<Profile> userManager,
            PixelPayService pixelPayService,
            PaystackService paystackService)
        {
            _context = context;
            _userManager = userManager;
            _pixelPayService = pixelPayService;
            _paystackService = paystackService;
        }

        public MisFinanceCategory Category { get; set; }
        public MisSetting Settings { get; set; }
        public SuperSetting SuperSetting { get; set; }

        // Pre-filled from user profile if logged in
        public string PrefillName { get; set; }
        public string PrefillEmail { get; set; }
        public string PrefillPhone { get; set; }
        public bool IsLoggedIn { get; set; }

        // Which providers are available (enabled + credentials set)
        public bool PixelPayAvailable { get; set; }
        public bool PaystackAvailable { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public string Notes { get; set; }

        // Paystack Inline Popup parameters
        public string PaystackPublicKey { get; set; }
        public string PaystackAccessCode { get; set; }
        public string PaystackReference { get; set; }
        public string PaystackEmail { get; set; }
        public decimal PaystackAmount { get; set; }

        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) return NotFound();

            bool isNumericId = long.TryParse(key, out long parsedId);
            Category = await _context.MisFinanceCategories
                .Include(c => c.MisGroup)
                .Include(c => c.UserCategory)
                .FirstOrDefaultAsync(m => m.Key == key || (isNumericId && m.Id == parsedId));

            if (Category == null || !Category.IsActive) return NotFound();

            // Redirect unauthenticated users if category is restricted to members only or targeted
            bool isRestricted = !Category.AllowGuestPayment || Category.MisGroupId.HasValue || Category.UserCategoryId.HasValue;
            if (isRestricted && User.Identity?.IsAuthenticated != true)
            {
                return RedirectToPage("/AuthV2/Account/Login", new { area = "V2", returnUrl = $"/pay/{key}" });
            }

            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            PaystackPublicKey = Settings?.PaystackPublicKey;
            ResolveAvailableProviders(Settings);

            // Auto-fill details if the user is logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    IsLoggedIn = true;
                    PrefillName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : $"{user.FirstName} {user.LastName}".Trim();
                    PrefillEmail = user.Email;
                    PrefillPhone = user.PhoneNumber;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            long CategoryId,
            string DonorName,
            string DonorEmail,
            string DonorPhone,
            decimal Amount,
            string PaymentProvider,
            int? FirstNumber,
            int? SecondNumber,
            int? Captcha)
        {
            Category = await _context.MisFinanceCategories.FindAsync(CategoryId);
            if (Category == null || !Category.IsActive) return NotFound();

            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            PaystackPublicKey = Settings?.PaystackPublicKey;
            ResolveAvailableProviders(Settings);

            // Validate Captcha security check
            string recaptchaResponse = Request.Form["g-recaptcha-response"];
            bool isCaptchaValid = await JurayUniversal.Website.Helpers.CaptchaValidator.ValidateCaptchaAsync(
                Settings, FirstNumber, SecondNumber, Captcha, recaptchaResponse);

            if (!isCaptchaValid)
            {
                StatusMessage = "Error: Security check (Captcha) failed. Please answer the verification question correctly.";
                IsLoggedIn = User.Identity?.IsAuthenticated == true;
                PrefillName = DonorName; PrefillEmail = DonorEmail; PrefillPhone = DonorPhone;
                return Page();
            }

            // Resolve logged-in user
            string userId = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                userId = user?.Id;
                if (string.IsNullOrEmpty(DonorName) && user != null)
                    DonorName = $"{user.FirstName} {user.LastName}".Trim();
                if (string.IsNullOrEmpty(DonorEmail) && user != null)
                    DonorEmail = user.Email;
            }

            // Validate a provider was chosen and is available
            var provider = (PaymentProvider ?? "").Trim();
            if ((provider == "PixelPay" && !PixelPayAvailable) ||
                (provider == "Paystack" && !PaystackAvailable) ||
                (provider != "PixelPay" && provider != "Paystack"))
            {
                StatusMessage = "Error: Invalid or unavailable payment provider selected.";
                IsLoggedIn = userId != null;
                PrefillName = DonorName; PrefillEmail = DonorEmail; PrefillPhone = DonorPhone;
                return Page();
            }

            // Generate unique reference
            var transactionRef = $"TX-{DateTime.UtcNow:yyyyMMddHHmmss}-{CategoryId}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            // Create Pending transaction record
            var tx = new MisFinanceTransaction
            {
                MisFinanceCategoryId = CategoryId,
                UserId = userId,
                DonorName = DonorName,
                DonorEmail = DonorEmail,
                DonorPhone = DonorPhone,
                Amount = Amount,
                Notes = Notes,
                TransactionReference = transactionRef,
                PaymentProvider = provider,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _context.MisFinanceTransactions.Add(tx);
            await _context.SaveChangesAsync();

            var callbackUrl = $"{Request.Scheme}://{Request.Host}/pay/callback?ref={transactionRef}";

            PaymentInitResult result;

            if (provider == "Paystack")
            {
                var email = (string.IsNullOrWhiteSpace(DonorEmail) ? "guest@payment.com" : DonorEmail).Trim();
                result = await _paystackService.InitializePaymentAsync(transactionRef, Amount, email, callbackUrl);

                if (result.Success && !string.IsNullOrEmpty(result.AccessCode))
                {
                    PaystackAccessCode = result.AccessCode;
                    PaystackReference = transactionRef;
                    PaystackEmail = email;
                    PaystackAmount = Amount;
                    IsLoggedIn = userId != null;
                    PrefillName = DonorName; PrefillEmail = DonorEmail; PrefillPhone = DonorPhone;
                    return Page();
                }
            }
            else
            {
                result = await _pixelPayService.InitializePaymentAsync(transactionRef, Amount, null, callbackUrl);
            }

            if (!result.Success)
            {
                StatusMessage = result.ErrorMessage;
                IsLoggedIn = userId != null;
                PrefillName = DonorName; PrefillEmail = DonorEmail; PrefillPhone = DonorPhone;
                return Page();
            }

            return Redirect(result.PaymentUrl);
        }

        private void ResolveAvailableProviders(MisSetting s)
        {
            if (s == null) return;
            PixelPayAvailable = s.EnablePixelPay
                && !string.IsNullOrWhiteSpace(s.PixelPayKeyId)
                && !string.IsNullOrWhiteSpace(s.PixelPaySecretKey)
                && !string.IsNullOrWhiteSpace(s.PixelPayBaseUrl);

            PaystackAvailable = s.EnablePaystack
                && !string.IsNullOrWhiteSpace(s.PaystackSecretKey);
        }
    }
}
