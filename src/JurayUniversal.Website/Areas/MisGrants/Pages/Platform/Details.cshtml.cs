using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Platform
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisGrant MisGrant { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisGrant = await _context.MisGrants
                .Include(g => g.MisDonor)
                .Include(g => g.Milestones)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (MisGrant == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
