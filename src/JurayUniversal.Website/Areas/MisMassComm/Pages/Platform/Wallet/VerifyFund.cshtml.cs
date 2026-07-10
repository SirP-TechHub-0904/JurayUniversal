using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.Wallet
{
    public class VerifyFundModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly PaystackService _paystackService;

        public VerifyFundModel(DashboardDbContext context, UserManager<Profile> userManager, PaystackService paystackService)
        {
            _context = context;
            _userManager = userManager;
            _paystackService = paystackService;
        }

        public string? StatusMessage { get; set; }
        public bool IsSuccessful { get; set; }

        public async Task<IActionResult> OnGetAsync(string reference, decimal amt)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (string.IsNullOrEmpty(reference))
            {
                StatusMessage = "Invalid transaction reference.";
                return Page();
            }

            // Verify with Paystack API
            bool isVerified = await _paystackService.VerifyPaymentAsync(reference);

            if (!isVerified)
            {
                StatusMessage = "We could not verify your payment. If you were debited, please contact support.";
                return Page();
            }

            // Check if transaction already exists to prevent duplicate funding
            var existingTx = await _context.MisWalletTransactions.FirstOrDefaultAsync(t => t.Reference == reference);
            if (existingTx != null)
            {
                StatusMessage = "This payment has already been credited to your wallet.";
                IsSuccessful = true;
                return Page();
            }

            var wallet = await _context.MisWallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (wallet == null)
            {
                wallet = new MisWallet { UserId = user.Id, Balance = 0 };
                _context.MisWallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            // Credit Wallet
            wallet.Balance += amt;
            wallet.LastUpdated = DateTime.UtcNow;

            // Log Transaction
            var tx = new MisWalletTransaction
            {
                MisWalletId = wallet.Id,
                UserId = user.Id,
                Amount = amt,
                TransactionType = Domain.Enum.Enum.MisWalletTransactionType.Credit,
                Description = "Funded Wallet via Paystack",
                Reference = reference,
                CreatedAt = DateTime.UtcNow
            };
            _context.MisWalletTransactions.Add(tx);
            
            await _context.SaveChangesAsync();

            StatusMessage = $"Successfully funded your wallet with NGN {amt.ToString("N2")}!";
            IsSuccessful = true;

            return Page();
        }
    }
}
