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
        
        public string GroupAlias { get; set; } = "Group";

        public IList<MisGroup> TopGroups { get; set; } = new List<MisGroup>();
        public IList<MisGroupMember> RecentActivity { get; set; } = new List<MisGroupMember>();
        
        public IList<string> GrowthLabels { get; set; } = new List<string>();
        public IList<int> GrowthData { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
            {
                GroupAlias = settings.GroupAlias;
            }

            TotalGroups = await _context.MisGroups.CountAsync();
            ActiveGroups = await _context.MisGroups.CountAsync(x => x.IsActive);
            TotalMemberships = await _context.MisGroupMembers.CountAsync();

            TopGroups = await _context.MisGroups
                .Where(g => g.IsActive)
                .OrderByDescending(g => g.Members.Count(m => m.MembershipStatus == "Active"))
                .Take(5)
                .Include(g => g.Members)
                .ToListAsync();

            RecentActivity = await _context.MisGroupMembers
                .Include(m => m.UserProfile)
                .Include(m => m.MisGroup)
                .OrderByDescending(m => m.DateJoined)
                .Take(5)
                .ToListAsync();

            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-5);
            var startDate = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1).ToUniversalTime();

            var recentMemberships = await _context.MisGroupMembers
                .Where(m => m.DateJoined >= startDate)
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
