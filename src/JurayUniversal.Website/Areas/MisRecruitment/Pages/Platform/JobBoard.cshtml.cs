using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Platform
{
    public class JobBoardModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public JobBoardModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public long? JobId { get; set; }

        public IList<MisJobPosting> ActiveJobs { get; set; } = new List<MisJobPosting>();
        public MisJobPosting SelectedJob { get; set; }

        public async Task OnGetAsync()
        {
            ActiveJobs = await _context.MisJobPostings
                .Where(j => j.Status == Domain.Enum.Enum.JobStatus.Published)
                .OrderByDescending(j => j.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            if (ActiveJobs.Any())
            {
                if (JobId.HasValue)
                {
                    SelectedJob = ActiveJobs.FirstOrDefault(x => x.Id == JobId.Value) ?? ActiveJobs.First();
                }
                else
                {
                    SelectedJob = ActiveJobs.First();
                }
            }
        }
    }
}
