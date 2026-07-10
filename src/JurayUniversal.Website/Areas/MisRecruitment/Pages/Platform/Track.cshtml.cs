using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Platform
{
    [Authorize(AuthenticationSchemes = "ApplicantAuth")]
    public class TrackModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public TrackModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisApplicantAccount Account { get; set; }
        public List<MisJobApplicant> Applications { get; set; } = new List<MisJobApplicant>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (long.TryParse(userIdString, out long accountId))
            {
                Account = await _context.MisApplicantAccounts.FindAsync(accountId);
                if (Account != null)
                {
                    Applications = await _context.MisJobApplicants
                        .Include(a => a.MisJobPosting)
                        .Where(a => a.MisApplicantAccountId == accountId)
                        .OrderByDescending(a => a.CreatedAt)
                        .AsNoTracking()
                        .ToListAsync();
                }
            }

            if (Account == null)
            {
                return RedirectToPage("./Auth/Login");
            }

            return Page();
        }
    }
}
