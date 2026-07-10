using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard.Jobs
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisJobPosting> MisJobPostings { get; set; } = new List<MisJobPosting>();

        public async Task OnGetAsync()
        {
            MisJobPostings = await _context.MisJobPostings
                .Include(x => x.Applicants)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
