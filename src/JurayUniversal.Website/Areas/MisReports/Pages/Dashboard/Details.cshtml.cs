using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard
{
    [Authorize(Policy = "MisReportsViewer")]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisReportSubmission Submission { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Submission = await _context.MisReportSubmissions
                .Include(s => s.MisReportSchema)
                .Include(s => s.UserProfile)
                .Include(s => s.TimeLogs)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Submission == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateCommentAsync(long submissionId, string managerComment)
        {
            var submission = await _context.MisReportSubmissions.FindAsync(submissionId);
            if (submission != null)
            {
                submission.ManagerComment = managerComment;
                await _context.SaveChangesAsync();
                TempData["success"] = "Comment updated successfully.";
            }
            return RedirectToPage(new { id = submissionId });
        }
    }
}
