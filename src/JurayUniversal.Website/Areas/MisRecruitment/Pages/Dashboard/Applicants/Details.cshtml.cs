using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard.Applicants
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

        public MisJobApplicant MisJobApplicant { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MisJobApplicant = await _context.MisJobApplicants
                .Include(m => m.MisJobPosting)
                .Include(m => m.Interviews)
                .Include(m => m.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (MisJobApplicant == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEmployAsync(long id)
        {
            var applicant = await _context.MisJobApplicants.FirstOrDefaultAsync(m => m.Id == id);
            if (applicant == null) return NotFound();

            if (applicant.Status == JurayUniversal.Domain.Enum.Enum.ApplicationStatus.Employed)
            {
                TempData["error"] = "Candidate is already employed.";
                return RedirectToPage(new { id });
            }

            var existingUser = await _userManager.FindByEmailAsync(applicant.Email);
            if (existingUser != null)
            {
                TempData["error"] = $"Cannot employ candidate. A system user with the email '{applicant.Email}' already exists.";
                return RedirectToPage(new { id });
            }

            var newProfile = new Profile
            {
                UserName = applicant.Email,
                Email = applicant.Email,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                PhoneNumber = applicant.Phone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newProfile, (System.Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD") ?? ""));

            if (result.Succeeded)
            {
                applicant.Status = JurayUniversal.Domain.Enum.Enum.ApplicationStatus.Employed;
                await _context.SaveChangesAsync();

                TempData["success"] = $"Candidate successfully employed! A new system account was created for {applicant.FirstName} with default password: [REDACTED_BOOTSTRAP_PASSWORD].";
            }
            else
            {
                TempData["error"] = "Failed to create user account: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(long applicantId, JurayUniversal.Domain.Enum.Enum.ApplicationStatus status)
        {
            var applicant = await _context.MisJobApplicants.FindAsync(applicantId);
            if (applicant == null) return NotFound();

            applicant.Status = status;
            await _context.SaveChangesAsync();

            TempData["success"] = $"Applicant status updated to {status}";
            return RedirectToPage(new { id = applicantId });
        }
    }
}
