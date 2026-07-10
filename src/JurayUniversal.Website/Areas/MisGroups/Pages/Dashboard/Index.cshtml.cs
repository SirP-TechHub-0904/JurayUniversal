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

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public int TotalGroups { get; set; }
        public int ActiveGroups { get; set; }
        public int TotalMemberships { get; set; }
        public int TotalMembers { get; set; }
        public int AvgMembers { get; set; }
        
        public string GroupAlias { get; set; } = "Group";

        [BindProperty(SupportsGet = true)]
        public string? DateRange { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string> ChartLabels { get; set; } = new();
        public List<int> ChartData { get; set; } = new();
        public IList<string> GrowthLabels { get; set; } = new List<string>();
        public IList<int> GrowthData { get; set; } = new List<int>();

        public IList<MisGroup> TopGroups { get; set; } = new List<MisGroup>();
        public IList<MisGroupMember> RecentActivity { get; set; } = new List<MisGroupMember>();
        
        public async Task<IActionResult> OnGetAsync()
        {
            var setting = await _context.MisSettings.FirstOrDefaultAsync();
            if (setting != null && !string.IsNullOrEmpty(setting.GroupAlias))
            {
                GroupAlias = setting.GroupAlias;
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

            var query = _context.MisGroups
                .Include(g => g.Members)
                .AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(g => g.CreatedAt >= StartDate.Value && g.CreatedAt <= EndDate.Value);
            }

            var groups = await query.ToListAsync();

            TotalGroups = groups.Count;
            TotalMembers = groups.Sum(g => g.Members?.Count ?? 0);
            ActiveGroups = groups.Count(g => g.IsActive);
            AvgMembers = TotalGroups > 0 ? (int)Math.Round((double)TotalMembers / TotalGroups) : 0;

            var allMembers = await _context.MisGroupMembers.ToListAsync();
            if (StartDate.HasValue && EndDate.HasValue)
            {
                allMembers = allMembers.Where(m => m.DateJoined >= StartDate.Value && m.DateJoined <= EndDate.Value).ToList();
            }

            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) totalDays = 365;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));
                
                var dailySum = allMembers.Count(m => m.DateJoined.Date == currentDate);
                ChartData.Add(dailySum);
            }

            TopGroups = groups
                .Where(g => g.IsActive)
                .OrderByDescending(g => g.Members.Count(m => m.MembershipStatus == "Active"))
                .Take(5)
                .ToList();

            RecentActivity = await _context.MisGroupMembers
                .Include(m => m.UserProfile)
                .Include(m => m.MisGroup)
                .OrderByDescending(m => m.DateJoined)
                .Take(5)
                .ToListAsync();

            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-5);
            var growthStartDate = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1).ToUniversalTime();

            var recentMemberships = await _context.MisGroupMembers
                .Where(m => m.DateJoined >= growthStartDate)
                .ToListAsync();

            for (int i = 5; i >= 0; i--)
            {
                var monthDate = DateTime.UtcNow.AddMonths(-i);
                GrowthLabels.Add(monthDate.ToString("MMM yyyy"));
                var count = recentMemberships.Count(m => m.DateJoined.Year == monthDate.Year && m.DateJoined.Month == monthDate.Month);
                GrowthData.Add(count);
            }

            return Page();
        }
    }
}
