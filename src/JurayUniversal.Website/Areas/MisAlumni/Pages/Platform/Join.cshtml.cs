using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Platform
{
    [Authorize]
    public class JoinModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public JoinModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string AlumniAlias { get; set; } = "Alumni";

        [BindProperty]
        public MisAlumniProfile Input { get; set; }

        public MisAlumniProfile ExistingProfile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ExistingProfile = await _context.MisAlumniProfiles.FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id);

            if (ExistingProfile != null && ExistingProfile.VerificationStatus == "Verified")
            {
                // If they are already verified, redirect to index
                return RedirectToPage("./Index");
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
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Double check they don't already have a profile
            var profile = await _context.MisAlumniProfiles.FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id);
            if (profile != null)
            {
                ExistingProfile = profile;
                return Page(); // Already have one, show pending message
            }

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            Input.UserProfileId = currentUser.Id;
            Input.VerificationStatus = "Pending"; // Set to Pending
            
            _context.MisAlumniProfiles.Add(Input);
            await _context.SaveChangesAsync();

            ExistingProfile = Input;
            TempData["success"] = "Your application has been submitted successfully!";
            
            return Page();
        }
    }
}
