using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Dashboard
{
    [Authorize(Policy = "MisAlumniEditor")]
    public class ListModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ListModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisAlumniProfile> AlumniProfiles { get; set; }
        public string AlumniAlias { get; set; } = "Alumni";

        [BindProperty(SupportsGet = true)]
        public string Status { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var query = _context.MisAlumniProfiles
                .Include(a => a.UserProfile)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(a => a.VerificationStatus == Status);
            }

            AlumniProfiles = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostVerifyAsync(long id)
        {
            var profile = await _context.MisAlumniProfiles.FindAsync(id);
            if (profile != null)
            {
                profile.VerificationStatus = "Verified";
                await _context.SaveChangesAsync();
                TempData["success"] = "Alumni status verified successfully.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var profile = await _context.MisAlumniProfiles.FindAsync(id);
            if (profile != null)
            {
                _context.MisAlumniProfiles.Remove(profile);
                await _context.SaveChangesAsync();
                TempData["success"] = "Alumni profile removed successfully.";
            }
            return RedirectToPage();
        }
    }
}
