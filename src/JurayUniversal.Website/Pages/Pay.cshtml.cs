using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Enum;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JurayUniversal.Website.Pages
{
    public class PayModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
        private readonly UserManager<Profile> _userManager;

        public PayModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, UserManager<Profile> userManager)
        {
            _context = context;
            _settingsService = settingsService;
            _userManager = userManager;
        }

        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }

        public PaymentCategory PaymentCategory { get; set; }

        [BindProperty]
        public long PaymentCategoryId { get; set; }

        [BindProperty]
        public string Fullname { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty(SupportsGet = true)]
        public long? Id { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            Setting = await _context.Settings.FirstOrDefaultAsync();

            var httpContext = HttpContext;
            var setting = await _settingsService.ValidateWeb(httpContext);

            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }

            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);
            }

            SuperSetting = setting.SuperSetting;

            if (id == null)
            {
                TempData["error"] = "Invalid payment category.";
                return RedirectToPage("/Payments");
            }

            PaymentCategory = await _context.PaymentCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (PaymentCategory == null)
            {
                TempData["error"] = "Payment category not found.";
                return RedirectToPage("/Payments");
            }

            PaymentCategoryId = PaymentCategory.Id;
            Amount = PaymentCategory.Amount;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Setting = await _context.Settings.FirstOrDefaultAsync();

            var httpContext = HttpContext;
            var setting = await _settingsService.ValidateWeb(httpContext);

            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }

            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);
            }

            SuperSetting = setting.SuperSetting;

            PaymentCategory = await _context.PaymentCategories.FirstOrDefaultAsync(x => x.Id == PaymentCategoryId);

            if (PaymentCategory == null)
            {
                TempData["error"] = "Payment category not found.";
                return Page();
            }

            Amount = PaymentCategory.Amount;

            if (string.IsNullOrWhiteSpace(Fullname) || string.IsNullOrWhiteSpace(Email))
            {
                TempData["error"] = "Fullname and Email are required.";
                return Page();
            }

            var paymentReference = $"PAY-{Guid.NewGuid():N}".ToUpper();

            var payment = new Payment
            {
                PaymentCategoryId = PaymentCategory.Id,
                UserId = User.Identity.IsAuthenticated
        ? _userManager.GetUserId(User)
        : null,

                Fullname = string.IsNullOrWhiteSpace(Fullname) ? null : Fullname,
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email,
                Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone,
                AmountPaid = PaymentCategory.Amount,
                PaymentReference = paymentReference,
                DatePaid = DateTime.UtcNow,
                Status = Domain.Enum.Enum.PaymentStatus.Pending
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SuperSetting.PaystackSecretKey}");

            var callbackUrl = string.IsNullOrWhiteSpace(SuperSetting.PaystackCallbackUrl)
                ? $"{Request.Scheme}://{Request.Host}/PaymentReturn"
                : SuperSetting.PaystackCallbackUrl;

            var payload = new
            {
                email = Email,
                amount = (int)(PaymentCategory.Amount * 100),
                reference = paymentReference,
                callback_url = callbackUrl,
                metadata = new
                {
                    paymentId = payment.Id,
                    categoryId = PaymentCategory.Id,
                    fullname = Fullname,
                    phone = Phone,
                    source = "Website"
                }
            };

            var response = await client.PostAsJsonAsync("https://api.paystack.co/transaction/initialize", payload);

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Unable to initialize payment.";
                return Page();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);

            var status = doc.RootElement.GetProperty("status").GetBoolean();
            if (!status)
            {
                TempData["error"] = "Unable to initialize payment.";
                return Page();
            }

            var authorizationUrl = doc.RootElement.GetProperty("data").GetProperty("authorization_url").GetString();

            return Redirect(authorizationUrl);
        }
    }
}