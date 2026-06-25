using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Platform
{
    [Authorize]
    public class MyProfileModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public MyProfileModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public MisAlumniProfile AlumniProfile { get; set; }

        public string AlumniAlias { get; set; } = "Alumni";

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            AlumniProfile = await _context.MisAlumniProfiles
                .FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id && p.VerificationStatus == "Verified");

            if (AlumniProfile == null)
            {
                // Not an alumni, or pending
                return NotFound("You do not have an active Alumni profile.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existing = await _context.MisAlumniProfiles
                .FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id && p.VerificationStatus == "Verified");

            if (existing == null)
            {
                return NotFound("Alumni profile not found.");
            }

            existing.ExitYear = AlumniProfile.ExitYear;
            existing.CurrentProfession = AlumniProfile.CurrentProfession;
            existing.Company = AlumniProfile.Company;
            existing.LinkedInUrl = AlumniProfile.LinkedInUrl;
            existing.Bio = AlumniProfile.Bio;
            existing.IsAvailableForMentorship = AlumniProfile.IsAvailableForMentorship;

            _context.Attach(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = "Your Alumni Profile was updated successfully.";
            return RedirectToPage("./MyProfile");
        }
    }
}
