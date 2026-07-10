using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Projects
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisProject> Projects { get; set; } = new List<MisProject>();

        public async Task OnGetAsync()
        {
            Projects = await _context.MisProjects
                .Include(p => p.Sprints)
                .Include(p => p.Tasks)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
