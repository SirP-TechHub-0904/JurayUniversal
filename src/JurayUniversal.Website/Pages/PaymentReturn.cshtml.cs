using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Enum;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Pages
{
    public class PaymentReturnModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;

        public PaymentReturnModel(
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context,
            ISettingsService settingsService)
        {
            _context = context;
            _settingsService = settingsService;
        }

        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }

        [BindProperty(SupportsGet = true)]
        public string reference { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

            if (string.IsNullOrWhiteSpace(reference))
            {
                TempData["error"] = "Invalid payment reference.";
                return RedirectToPage("/Payments");
            }

            var payment = await _context.Payments
                .Include(x => x.PaymentCategory)
                .FirstOrDefaultAsync(x => x.PaymentReference == reference);

            if (payment == null)
            {
                TempData["error"] = "Payment record not found.";
                return RedirectToPage("/Payments");
            }

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SuperSetting.PaystackSecretKey}");

            var response = await client.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Unable to verify payment.";
                return RedirectToReceipt(payment.Id);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);

            var apiStatus = doc.RootElement.GetProperty("status").GetBoolean();
            if (!apiStatus)
            {
                TempData["error"] = "Unable to verify payment.";
                return RedirectToReceipt(payment.Id);
            }

            var data = doc.RootElement.GetProperty("data");

            var transactionStatus = data.GetProperty("status").GetString();
            var amountInKobo = data.GetProperty("amount").GetInt32();
            var paidAmount = amountInKobo / 100m;

            if (paidAmount != payment.AmountPaid)
            {
                TempData["error"] = "Payment amount mismatch.";
                return RedirectToReceipt(payment.Id);
            }

            if (transactionStatus == "success")
            {
                if (payment.Status != PaymentStatus.Success)
                {
                    payment.Status = PaymentStatus.Success;
                    payment.DatePaid = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                TempData["success"] = "Payment successful.";
            }
            else if (transactionStatus == "failed")
            {
                payment.Status = PaymentStatus.Failed;
                await _context.SaveChangesAsync();
                TempData["error"] = "Payment failed.";
            }
            else
            {
                payment.Status = PaymentStatus.Pending;
                await _context.SaveChangesAsync();
                TempData["error"] = $"Payment status: {transactionStatus}";
            }

            return RedirectToReceipt(payment.Id);
        }

        private IActionResult RedirectToReceipt(long paymentId)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect($"/Staff/Pages/Dashboard/PaymentReceipt?id={paymentId}");
            }

            return RedirectToPage("/PaymentReceipt", new { id = paymentId });
        }
    }
}