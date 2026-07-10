using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Wallet
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, Microsoft.AspNetCore.Identity.UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public JurayUniversal.Domain.Models.MisSetting Settings { get; set; }
        public IList<MisNewsletterWallet> UserWallets { get; set; } = new List<MisNewsletterWallet>();
        public IList<MisNewsletterPlanTransaction> GlobalPurchases { get; set; } = new List<MisNewsletterPlanTransaction>();
        public IList<MisNewsletterWalletTransaction> QuotaShares { get; set; } = new List<MisNewsletterWalletTransaction>();
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList UserList { get; set; }

        [BindProperty]
        public string SelectedUserId { get; set; }
        
        [BindProperty]
        public int ShareAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            UserWallets = await _context.MisNewsletterWallets
                .Include(w => w.User)
                .OrderByDescending(w => w.EmailQuotaBalance)
                .ToListAsync();

            GlobalPurchases = await _context.MisNewsletterPlanTransactions
                .Include(p => p.MisNewsletterPlan)
                .Where(p => p.IsGlobalPurchase == true)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            QuotaShares = await _context.MisNewsletterWalletTransactions
                .Include(t => t.MisNewsletterWallet)
                .ThenInclude(w => w.User)
                .Where(t => t.Description == "Quota Shared by Admin")
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var users = await _userManager.Users.ToListAsync();
            UserList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(users.Select(u => new { u.Id, DisplayName = $"{u.FullName} ({u.Email})" }), "Id", "DisplayName");

            return Page();
        }

        public async Task<IActionResult> OnPostShareQuotaAsync()
        {
            if (string.IsNullOrEmpty(SelectedUserId) || ShareAmount <= 0)
            {
                TempData["error"] = "Invalid share amount or user selected.";
                return RedirectToPage();
            }

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings == null) return NotFound();

            if (settings.GlobalNewsletterQuota < ShareAmount)
            {
                TempData["error"] = "Insufficient Global Quota. Please buy more quota first.";
                return RedirectToPage();
            }

            // Deduct from Global
            settings.GlobalNewsletterQuota -= ShareAmount;

            // Add to User
            var userWallet = await _context.MisNewsletterWallets.FirstOrDefaultAsync(w => w.UserId == SelectedUserId);
            if (userWallet == null)
            {
                userWallet = new MisNewsletterWallet
                {
                    UserId = SelectedUserId,
                    EmailQuotaBalance = ShareAmount,
                    ExpiryDate = settings.GlobalNewsletterQuotaExpiry // Inherit same duration
                };
                _context.MisNewsletterWallets.Add(userWallet);
            }
            else
            {
                userWallet.EmailQuotaBalance += ShareAmount;
                // Extend their duration to match the admin's global expiry if it's longer
                if (settings.GlobalNewsletterQuotaExpiry > userWallet.ExpiryDate)
                {
                    userWallet.ExpiryDate = settings.GlobalNewsletterQuotaExpiry;
                }
            }

            await _context.SaveChangesAsync();

            // Log Transaction
            var transaction = new MisNewsletterWalletTransaction
            {
                MisNewsletterWalletId = userWallet.Id,
                Amount = ShareAmount,
                TransactionType = JurayUniversal.Domain.Enum.Enum.MisWalletTransactionType.Credit,
                Description = "Quota Shared by Admin",
                Date = DateTime.UtcNow
            };
            _context.MisNewsletterWalletTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["success"] = $"Successfully shared {ShareAmount} emails with the user.";
            return RedirectToPage();
        }
    }
}
