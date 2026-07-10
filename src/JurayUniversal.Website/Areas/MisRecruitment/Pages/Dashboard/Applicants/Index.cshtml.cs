using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard.Applicants
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisJobApplicant> MisJobApplicants { get; set; } = new List<MisJobApplicant>();
        public SelectList JobPostingsList { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public long? JobId { get; set; }

        public async Task OnGetAsync()
        {
            var jobs = await _context.MisJobPostings.ToListAsync();
            JobPostingsList = new SelectList(jobs, "Id", "Title");

            var query = _context.MisJobApplicants
                .Include(x => x.MisJobPosting)
                .AsNoTracking();

            if (JobId.HasValue && JobId.Value > 0)
            {
                query = query.Where(x => x.MisJobPostingId == JobId.Value);
            }

            MisJobApplicants = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }
    }
}
