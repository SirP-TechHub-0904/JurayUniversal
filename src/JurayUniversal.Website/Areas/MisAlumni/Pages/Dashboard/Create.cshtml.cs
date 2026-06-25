using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Dashboard
{
    [Authorize(Policy = "MisAlumniEditor")]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CreateModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public MisAlumniProfile MisAlumniProfile { get; set; }

        public string AlumniAlias { get; set; } = "Alumni";

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var users = await _userManager.Users.OrderBy(u => u.FirstName).ToListAsync();
            ViewData["UserProfileId"] = new SelectList(users, "Id", "Email");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            if (!ModelState.IsValid)
            {
                var users = await _userManager.Users.OrderBy(u => u.FirstName).ToListAsync();
                ViewData["UserProfileId"] = new SelectList(users, "Id", "Email");
                return Page();
            }

            // Check if already an alumni
            var existing = await _context.MisAlumniProfiles.FirstOrDefaultAsync(a => a.UserProfileId == MisAlumniProfile.UserProfileId);
            if (existing != null)
            {
                ModelState.AddModelError(string.Empty, "This user is already registered in the Alumni network.");
                var users = await _userManager.Users.OrderBy(u => u.FirstName).ToListAsync();
                ViewData["UserProfileId"] = new SelectList(users, "Id", "Email");
                return Page();
            }

            MisAlumniProfile.VerificationStatus = "Verified"; // Admins adding it directly bypass pending
            
            _context.MisAlumniProfiles.Add(MisAlumniProfile);
            await _context.SaveChangesAsync();

            TempData["success"] = $"Successfully flagged user as an {AlumniAlias}.";

            return RedirectToPage("./List");
        }
    }
}
