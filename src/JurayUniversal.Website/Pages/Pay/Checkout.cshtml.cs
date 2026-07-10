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

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Category = await _context.MisFinanceCategories.FindAsync(id);
            if (Category == null || !Category.IsActive) return NotFound();

            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            ResolveAvailableProviders(Settings);

            // Auto-fill details if the user is logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    IsLoggedIn = true;
                    PrefillName = $"{user.FirstName} {user.LastName}".Trim();
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
            string PaymentProvider)
        {
            Category = await _context.MisFinanceCategories.FindAsync(CategoryId);
            if (Category == null || !Category.IsActive) return NotFound();

            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            ResolveAvailableProviders(Settings);

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
                var email = string.IsNullOrEmpty(DonorEmail) ? "guest@payment.com" : DonorEmail;
                result = await _paystackService.InitializePaymentAsync(transactionRef, Amount, email, callbackUrl);
            }
            else
            {
                result = await _pixelPayService.InitializePaymentAsync(transactionRef, Amount, null, callbackUrl);
            }

            if (!result.Success)
            {
                StatusMessage = $"Error: {result.ErrorMessage}";
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
