using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.Wallet
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisSmsTransaction> Transactions { get; set; } = new List<MisSmsTransaction>();
        public decimal CurrentBalance { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            CurrentBalance = settings?.SmsCreditBalance ?? 0m;

            Transactions = await _context.MisSmsTransactions
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
