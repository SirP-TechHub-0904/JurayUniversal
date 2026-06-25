using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Platform
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string AlumniAlias { get; set; } = "Alumni";
        public bool IsAlumni { get; set; }

        public IList<MisAlumniPost> RecentPosts { get; set; }
        public IList<MisAlumniProfile> FeaturedMentors { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var profile = await _context.MisAlumniProfiles.FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id);
                IsAlumni = profile != null && profile.VerificationStatus == "Verified";
            }

            if (!IsAlumni)
            {
                return RedirectToPage("./Join");
            }

            RecentPosts = await _context.MisAlumniPosts
                .Include(p => p.UserProfile)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(4)
                .ToListAsync();

            FeaturedMentors = await _context.MisAlumniProfiles
                .Include(p => p.UserProfile)
                .Where(p => p.IsAvailableForMentorship && p.VerificationStatus == "Verified")
                .OrderBy(p => p.Id) // Randomize in production, but order by id for now
                .Take(3)
                .ToListAsync();

            return Page();
        }
    }
}
