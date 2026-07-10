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
    public class TransferModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public TransferModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public string RecipientEmail { get; set; } = string.Empty;

        public MisWallet UserWallet { get; set; } = new MisWallet();
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var wallet = await _context.MisWallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (wallet == null)
            {
                wallet = new MisWallet { UserId = user.Id, Balance = 0 };
                _context.MisWallets.Add(wallet);
                await _context.SaveChangesAsync();
            }
            UserWallet = wallet;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            await OnGetAsync(); // populate UserWallet

            if (Amount <= 0)
            {
                StatusMessage = "Transfer amount must be greater than 0.";
                return Page();
            }

            if (UserWallet.Balance < Amount)
            {
                StatusMessage = "Insufficient balance for this transfer.";
                return Page();
            }

            var recipient = await _userManager.FindByEmailAsync(RecipientEmail);
            if (recipient == null)
            {
                StatusMessage = "Recipient not found. Please check the email address.";
                return Page();
            }

            if (recipient.Id == user.Id)
            {
                StatusMessage = "You cannot transfer units to yourself.";
                return Page();
            }

            var recipientWallet = await _context.MisWallets.FirstOrDefaultAsync(w => w.UserId == recipient.Id);
            if (recipientWallet == null)
            {
                recipientWallet = new MisWallet { UserId = recipient.Id, Balance = 0 };
                _context.MisWallets.Add(recipientWallet);
            }

            // Debit sender
            UserWallet.Balance -= Amount;
            UserWallet.LastUpdated = DateTime.UtcNow;

            var debitTx = new MisWalletTransaction
            {
                MisWalletId = UserWallet.Id,
                UserId = user.Id,
                Amount = Amount,
                TransactionType = Domain.Enum.Enum.MisWalletTransactionType.Transfer,
                Description = $"Transferred to {recipient.Email}",
                Reference = Guid.NewGuid().ToString("N"),
                CreatedAt = DateTime.UtcNow
            };
            _context.MisWalletTransactions.Add(debitTx);

            // Credit recipient
            recipientWallet.Balance += Amount;
            recipientWallet.LastUpdated = DateTime.UtcNow;

            var creditTx = new MisWalletTransaction
            {
                MisWalletId = recipientWallet.Id,
                UserId = recipient.Id,
                Amount = Amount,
                TransactionType = Domain.Enum.Enum.MisWalletTransactionType.Credit,
                Description = $"Transfer from {user.Email}",
                Reference = debitTx.Reference, // link via reference
                CreatedAt = DateTime.UtcNow
            };
            _context.MisWalletTransactions.Add(creditTx);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully transferred {Amount:N2} to {recipient.Email}.";
            return RedirectToPage("./Index");
        }
    }
}
