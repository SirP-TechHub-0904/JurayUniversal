using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int TotalApplicants { get; set; }
        public int PendingInterviews { get; set; }

        public int CandidatesHired { get; set; }

        // Chart Data
        public List<int> PipelineData { get; set; } = new List<int>();
        public List<string> PipelineLabels { get; set; } = new List<string> { "Applied", "Screening", "Interviewing", "Offered", "Rejected" };
        
        public List<string> JobStatusLabels { get; set; } = new List<string> { "Draft", "Published", "Closed" };
        public List<int> JobStatusData { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            var jobs = await _context.MisJobPostings.AsNoTracking().ToListAsync();
            TotalJobs = jobs.Count;
            ActiveJobs = jobs.Count(j => j.Status == Domain.Enum.Enum.JobStatus.Published);
            
            var applicants = await _context.MisJobApplicants.AsNoTracking().ToListAsync();
            TotalApplicants = applicants.Count;
            CandidatesHired = applicants.Count(a => a.Status == Domain.Enum.Enum.ApplicationStatus.Offered);
            
            PendingInterviews = await _context.MisJobInterviews
                .CountAsync(i => i.InterviewDate >= System.DateTime.UtcNow && i.Score == null);

            // Pipeline Data
            PipelineData = new List<int>
            {
                applicants.Count(a => a.Status == Domain.Enum.Enum.ApplicationStatus.Applied),
                applicants.Count(a => a.Status == Domain.Enum.Enum.ApplicationStatus.CVScreening || a.Status == Domain.Enum.Enum.ApplicationStatus.PhoneScreening || a.Status == Domain.Enum.Enum.ApplicationStatus.BackgroundCheck),
                applicants.Count(a => a.Status == Domain.Enum.Enum.ApplicationStatus.OnlineInterview || a.Status == Domain.Enum.Enum.ApplicationStatus.OfflineInterview),
                applicants.Count(a => a.Status == Domain.Enum.Enum.ApplicationStatus.Offered),
                applicants.Count(a => a.Status == Domain.Enum.Enum.ApplicationStatus.Rejected)
            };

            // Job Status Data
            JobStatusData = new List<int>
            {
                jobs.Count(j => j.Status == Domain.Enum.Enum.JobStatus.Draft),
                jobs.Count(j => j.Status == Domain.Enum.Enum.JobStatus.Published),
                jobs.Count(j => j.Status == Domain.Enum.Enum.JobStatus.Closed)
            };
        }
    }
}
