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
        
        [BindProperty(SupportsGet = true)]
        public string? DateRange { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string> ChartLabels { get; set; } = new();
        public List<int> ChartData { get; set; } = new();

        public IList<string> YearLabels { get; set; } = new List<string>();
        public IList<int> YearData { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            if (!string.IsNullOrEmpty(DateRange))
            {
                var dates = DateRange.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParse(dates[0], out DateTime start)) StartDate = start;
                    if (DateTime.TryParse(dates[1], out DateTime end)) EndDate = end.AddDays(1).AddTicks(-1);
                }
            }

            var query = _context.MisAlumniProfiles
                .Include(a => a.UserProfile)
                .AsQueryable();
            var postsQuery = _context.MisAlumniPosts.AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(a => a.CreatedAt >= StartDate.Value && a.CreatedAt <= EndDate.Value);
                postsQuery = postsQuery.Where(p => p.CreatedAt >= StartDate.Value && p.CreatedAt <= EndDate.Value);
            }

            var allProfiles = await query.ToListAsync();

            TotalAlumni = allProfiles.Count;
            PendingVerifications = allProfiles.Count(x => x.VerificationStatus == "Pending");
            AvailableMentors = allProfiles.Count(x => x.IsAvailableForMentorship);
            TotalPosts = await postsQuery.CountAsync(x => x.IsActive);

            RecentAlumni = allProfiles
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .ToList();

            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) totalDays = 365;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));
                
                var dailySum = allProfiles.Count(m => m.CreatedAt.Date == currentDate);
                ChartData.Add(dailySum);
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
