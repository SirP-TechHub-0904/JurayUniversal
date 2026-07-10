using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Dashboard
{
    [Authorize(Policy = "MisGrantsViewer")]
    public class HistoryModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public HistoryModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisGrant> Grants { get; set; } = new List<MisGrant>();

        public async Task OnGetAsync()
        {
            Grants = await _context.MisGrants
                .Include(g => g.Milestones)
                .Include(g => g.MisDonor)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }
    }
}
