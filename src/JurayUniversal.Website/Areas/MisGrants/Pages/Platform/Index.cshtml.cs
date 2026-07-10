using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Platform
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisGrant> ActiveGrants { get; set; } = new List<MisGrant>();

        public async Task OnGetAsync()
        {
            ActiveGrants = await _context.MisGrants
                .Include(g => g.MisDonor)
                .Include(g => g.Milestones)
                .Where(g => g.IsActive)
                .OrderByDescending(g => g.StartDate)
                .ToListAsync();
        }
    }
}
