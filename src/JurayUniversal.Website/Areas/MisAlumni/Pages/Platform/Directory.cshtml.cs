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
    public class DirectoryModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DirectoryModel(DashboardDbContext context)
        {
            _context = context;
        }

        public string AlumniAlias { get; set; } = "Alumni";

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool MentorsOnly { get; set; }

        public IList<MisAlumniProfile> Profiles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var query = _context.MisAlumniProfiles
                .Include(p => p.UserProfile)
                .Where(p => p.VerificationStatus == "Verified")
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var lowerSearch = SearchQuery.ToLower();
                query = query.Where(p => 
                    (p.UserProfile.FirstName != null && p.UserProfile.FirstName.ToLower().Contains(lowerSearch)) ||
                    (p.UserProfile.LastName != null && p.UserProfile.LastName.ToLower().Contains(lowerSearch)) ||
                    (p.CurrentProfession != null && p.CurrentProfession.ToLower().Contains(lowerSearch)) ||
                    (p.Company != null && p.Company.ToLower().Contains(lowerSearch))
                );
            }

            if (MentorsOnly)
            {
                query = query.Where(p => p.IsAvailableForMentorship);
            }

            Profiles = await query.OrderByDescending(p => p.ExitYear).ToListAsync();

            return Page();
        }
    }
}
