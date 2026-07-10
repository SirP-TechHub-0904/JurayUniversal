using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Platform
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<MisForm> Forms { get; set; }
        public Dictionary<long, int> UserSubmissionCounts { get; set; } = new Dictionary<long, int>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get all published forms
            Forms = await _context.MisForms
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var userResponses = await _context.MisFormResponses
                    .Where(r => r.UserProfileId == userId)
                    .GroupBy(r => r.MisFormId)
                    .Select(g => new { FormId = g.Key, Count = g.Count() })
                    .ToListAsync();

                foreach (var res in userResponses)
                {
                    UserSubmissionCounts[res.FormId] = res.Count;
                }
            }

            return Page();
        }
    }
}
