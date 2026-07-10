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
using System.Text.Json;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Dashboard
{
    [Authorize]
    public class VerifyPlanModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IConfiguration _configuration;

        public VerifyPlanModel(DashboardDbContext context, UserManager<Profile> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(string reference)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(reference)) return NotFound();

            var transaction = await _context.MisNewsletterPlanTransactions
                .Include(t => t.MisNewsletterPlan)
                .FirstOrDefaultAsync(t => t.Reference == reference);

            if (transaction == null) return NotFound();

            bool isSuccess = false;

            if (transaction.Amount <= 0)
            {
                isSuccess = true;
            }
            else if (transaction.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success)
            {
                // Already fulfilled by webhook
                return RedirectBasedOnType(transaction);
            }
            else
            {
                // Verify with Paystack directly
                string paystackSecret = _configuration["Paystack:SecretKey"];
                if (!string.IsNullOrEmpty(paystackSecret))
                {
                    try
                    {
                        using var client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecret);
                        
                        var response = await client.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            using var doc = JsonDocument.Parse(responseString);
                            var data = doc.RootElement.GetProperty("data");
                            var status = data.GetProperty("status").GetString();
                            
                            if (status == "success")
                            {
                                isSuccess = true;
                            }
                        }
                    }
                    catch
                    {
                        // Ignore and handle as failure
                    }
                }
            }

            if (isSuccess && transaction.Status != JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success)
            {
                transaction.Status = JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success;
                transaction.PaidAt = DateTime.UtcNow;
                
                var plan = transaction.MisNewsletterPlan;
                
                if (transaction.IsGlobalPurchase)
                {
                    var settings = await _context.MisSettings.FirstOrDefaultAsync();
                    if (settings == null)
                    {
                        settings = new MisSetting();
                        _context.MisSettings.Add(settings);
                    }
                    
                    settings.GlobalNewsletterQuota += transaction.Quota;
                    var newExpiry = DateTime.UtcNow.AddMonths(plan.DurationInMonths);
                    if (settings.GlobalNewsletterQuotaExpiry == null || settings.GlobalNewsletterQuotaExpiry < DateTime.UtcNow)
                    {
                        settings.GlobalNewsletterQuotaExpiry = newExpiry;
                    }
                    else
                    {
                        settings.GlobalNewsletterQuotaExpiry = settings.GlobalNewsletterQuotaExpiry.Value.AddMonths(plan.DurationInMonths);
                    }
                    
                    TempData["success"] = $"Successfully purchased {plan.Name}. {plan.Quota} emails added to Global Wallet!";
                }
                else
                {
                    // Payment was successful, update Personal wallet
                    var wallet = await _context.MisNewsletterWallets.FirstOrDefaultAsync(w => w.UserId == transaction.UserId);
                    if (wallet == null)
                    {
                        wallet = new MisNewsletterWallet
                        {
                            UserId = transaction.UserId,
                            EmailQuotaBalance = transaction.Quota,
                            ExpiryDate = DateTime.UtcNow.AddMonths(plan.DurationInMonths)
                        };
                        _context.MisNewsletterWallets.Add(wallet);
                    }
                    else
                    {
                        wallet.EmailQuotaBalance += transaction.Quota;
                        
                        var newExpiry = DateTime.UtcNow.AddMonths(plan.DurationInMonths);
                        if (wallet.ExpiryDate == null || wallet.ExpiryDate < DateTime.UtcNow)
                        {
                            wallet.ExpiryDate = newExpiry;
                        }
                        else
                        {
                            wallet.ExpiryDate = wallet.ExpiryDate.Value.AddMonths(plan.DurationInMonths);
                        }
                    }

                    // Log Transaction for user history
                    var wTx = new MisNewsletterWalletTransaction
                    {
                        MisNewsletterWalletId = wallet.Id,
                        Amount = transaction.Quota,
                        TransactionType = JurayUniversal.Domain.Enum.Enum.MisWalletTransactionType.Credit,
                        Description = $"Purchased Plan: {plan.Name}",
                        Date = DateTime.UtcNow
                    };
                    _context.MisNewsletterWalletTransactions.Add(wTx);
                    
                    TempData["success"] = $"Successfully purchased {plan.Name}. {plan.Quota} emails added to your wallet!";
                }

                await _context.SaveChangesAsync();
            }
            else if (!isSuccess)
            {
                TempData["error"] = "Payment verification failed or was not completed.";
            }

            return RedirectBasedOnType(transaction);
        }
        
        private IActionResult RedirectBasedOnType(MisNewsletterPlanTransaction transaction)
        {
            if (transaction.IsGlobalPurchase)
            {
                return Redirect("/MisNewsletter/Dashboard/Plans/Index");
            }
            return RedirectToPage("./Index");
        }
    }
}
