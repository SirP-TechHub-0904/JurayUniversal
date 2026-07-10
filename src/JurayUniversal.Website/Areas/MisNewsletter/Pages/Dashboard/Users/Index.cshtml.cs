using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Users
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisNewsletterWallet> UserWallets { get; set; } = new List<MisNewsletterWallet>();

        public async Task OnGetAsync()
        {
            UserWallets = await _context.MisNewsletterWallets
                .Include(w => w.User)
                .OrderByDescending(w => w.EmailQuotaBalance)
                .ToListAsync();
        }
    }
}