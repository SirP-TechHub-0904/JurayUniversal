using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.Wallet
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisWallet UserWallet { get; set; } = new MisWallet();
        public IList<MisWalletTransaction> Transactions { get; set; } = new List<MisWalletTransaction>();

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

            Transactions = await _context.MisWalletTransactions
                .Where(t => t.MisWalletId == wallet.Id || t.UserId == user.Id)
                .OrderByDescending(t => t.CreatedAt)
                .Take(50)
                .ToListAsync();

            return Page();
        }
    }
}
