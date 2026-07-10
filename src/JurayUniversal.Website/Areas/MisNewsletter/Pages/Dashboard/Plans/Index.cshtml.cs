using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Plans
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public JurayUniversal.Domain.Models.MisSetting Settings { get; set; }
        public IList<MisNewsletterPlan> AvailablePlans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (Settings == null)
            {
                Settings = new JurayUniversal.Domain.Models.MisSetting();
            }

            AvailablePlans = await _context.MisNewsletterPlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Amount)
                .ToListAsync();

            return Page();
        }
    }
}
