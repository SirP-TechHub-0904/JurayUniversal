using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Platform
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisReportSubmission Submission { get; set; }
        public bool CanEdit { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Submission = await _context.MisReportSubmissions
                .Include(s => s.MisReportSchema)
                .Include(s => s.TimeLogs)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Submission == null) return NotFound();

            // Verify they own the submission
            if (Submission.UserProfileId != user.Id)
            {
                return Challenge();
            }

            var deadline = Submission.MisReportSchema.DeadlineTime;
            var targetTime = Submission.PeriodDate.Add(deadline);
            CanEdit = DateTime.Now <= targetTime;

            return Page();
        }
    }
}
