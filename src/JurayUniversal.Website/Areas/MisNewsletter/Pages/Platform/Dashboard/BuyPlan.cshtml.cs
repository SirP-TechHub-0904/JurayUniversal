using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Dashboard
{
    [Authorize]
    public class BuyPlanModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IConfiguration _configuration;

        public BuyPlanModel(DashboardDbContext context, UserManager<Profile> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public MisNewsletterPlan Plan { get; set; }
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsGlobal { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Plan = await _context.MisNewsletterPlans.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (Plan == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Plan = await _context.MisNewsletterPlans.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (Plan == null) return NotFound();

            string transactionRef = "NEWS-" + Guid.NewGuid().ToString("N").Substring(0, 15).ToUpper();

            // Save Pending Transaction
            var transaction = new MisNewsletterPlanTransaction
            {
                UserId = user.Id,
                MisNewsletterPlanId = Plan.Id,
                Amount = Plan.Amount,
                Quota = Plan.Quota,
                Reference = transactionRef,
                Status = JurayUniversal.Domain.Enum.Enum.PaymentStatus.Pending,
                PaymentProvider = "Paystack",
                IsGlobalPurchase = IsGlobal,
                CreatedAt = DateTime.UtcNow
            };
            _context.MisNewsletterPlanTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            var callbackUrl = $"{Request.Scheme}://{Request.Host}/MisNewsletter/Platform/Dashboard/VerifyPlan?reference={transactionRef}";

            if (Plan.Amount <= 0)
            {
                // Bypass payment for free plans
                return RedirectToPage("./VerifyPlan", new { reference = transactionRef });
            }

            // For paid plans, read Developer keys from AppSettings
            string paystackSecret = _configuration["Paystack:SecretKey"];
            if (string.IsNullOrEmpty(paystackSecret))
            {
                StatusMessage = "Payment gateway is not configured properly by the developer.";
                return Page();
            }

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecret);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new
                {
                    amount = (long)(Plan.Amount * 100), // Paystack requires kobo/cents
                    email = user.Email ?? "user@system.com",
                    reference = transactionRef,
                    callback_url = callbackUrl
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.paystack.co/transaction/initialize", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseString);
                    var authUrl = doc.RootElement.GetProperty("data").GetProperty("authorization_url").GetString();
                    return Redirect(authUrl);
                }
                else
                {
                    StatusMessage = $"Paystack initialization failed: {responseString}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Error contacting payment gateway: " + ex.Message;
                return Page();
            }
        }
    }
}
