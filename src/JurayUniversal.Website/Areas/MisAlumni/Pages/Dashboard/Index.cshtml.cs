using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public int TotalAlumni { get; set; }
        public int PendingVerifications { get; set; }
        public int TotalPosts { get; set; }
        public int AvailableMentors { get; set; }
        
        public string AlumniAlias { get; set; } = "Alumni";

        public IList<MisAlumniProfile> RecentAlumni { get; set; } = new List<MisAlumniProfile>();
        
        public IList<string> GrowthLabels { get; set; } = new List<string>();
        public IList<int> GrowthData { get; set; } = new List<int>();

        public IList<string> YearLabels { get; set; } = new List<string>();
        public IList<int> YearData { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            TotalAlumni = await _context.MisAlumniProfiles.CountAsync();
            PendingVerifications = await _context.MisAlumniProfiles.CountAsync(x => x.VerificationStatus == "Pending");
            TotalPosts = await _context.MisAlumniPosts.CountAsync(x => x.IsActive);
            AvailableMentors = await _context.MisAlumniProfiles.CountAsync(x => x.IsAvailableForMentorship);

            RecentAlumni = await _context.MisAlumniProfiles
                .Include(a => a.UserProfile)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .ToListAsync();

            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-5);
            var startDate = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1).ToUniversalTime();

            var recentProfiles = await _context.MisAlumniProfiles
                .Where(m => m.CreatedAt >= startDate)
                .ToListAsync();

            for (int i = 5; i >= 0; i--)
            {
                var monthDate = DateTime.UtcNow.AddMonths(-i);
                GrowthLabels.Add(monthDate.ToString("MMM yyyy"));
                var count = recentProfiles.Count(m => m.CreatedAt.Year == monthDate.Year && m.CreatedAt.Month == monthDate.Month);
                GrowthData.Add(count);
            }

            var yearGroupings = await _context.MisAlumniProfiles
                .GroupBy(a => a.ExitYear)
                .Select(g => new { Year = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            foreach(var item in yearGroupings.OrderBy(g => g.Year))
            {
                YearLabels.Add(item.Year.ToString());
                YearData.Add(item.Count);
            }

            return Page();
        }
    }
}
