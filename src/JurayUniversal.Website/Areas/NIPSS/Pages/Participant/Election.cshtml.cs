using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
     [Microsoft.AspNetCore.Authorization.Authorize]

    public class ElectionModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ElectionModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<Evoting> Evoting { get; set; }

        public async Task OnGetAsync()
        {
            Evoting = await _context.Evotings
                .Include(v => v.Course).ToListAsync();
        }
    }
}
