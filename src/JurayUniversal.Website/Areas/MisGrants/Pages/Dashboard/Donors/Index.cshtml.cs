using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Dashboard.Donors
{
    [Authorize(Policy = "MisGrantsViewer")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisDonor> Donors { get; set; } = new List<MisDonor>();

        public async Task OnGetAsync()
        {
            Donors = await _context.MisDonors
                .Include(d => d.Grants)
                .ToListAsync();
        }
    }
}
