using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisNewsletterWallet UserWallet { get; set; }
        public IList<MisNewsletterPlan> AvailablePlans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            UserWallet = await _context.MisNewsletterWallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            
            // If they don't have a wallet yet, they just have 0 balance
            if (UserWallet == null)
            {
                UserWallet = new MisNewsletterWallet { UserId = user.Id, EmailQuotaBalance = 0 };
            }

            AvailablePlans = await _context.MisNewsletterPlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Amount)
                .ToListAsync();

            return Page();
        }
    }
}

