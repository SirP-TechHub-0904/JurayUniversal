using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard.Interviews
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ScoreModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ScoreModel(DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(long interviewId, long applicantId, int score, string feedback)
        {
            var interview = await _context.MisJobInterviews.FindAsync(interviewId);
            if (interview == null) return NotFound();

            interview.Score = score;
            interview.AdminFeedback = feedback;

            await _context.SaveChangesAsync();

            TempData["success"] = "Interview score saved successfully!";
            return Redirect($"/MisRecruitment/Dashboard/Applicants/Details?id={applicantId}");
        }
    }
}
