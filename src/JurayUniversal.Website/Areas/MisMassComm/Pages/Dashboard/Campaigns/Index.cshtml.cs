using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.Campaigns
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisSmsCampaign> Campaigns { get; set; } = new List<MisSmsCampaign>();

        public async Task<IActionResult> OnGetAsync()
        {
            Campaigns = await _context.MisSmsCampaigns
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
