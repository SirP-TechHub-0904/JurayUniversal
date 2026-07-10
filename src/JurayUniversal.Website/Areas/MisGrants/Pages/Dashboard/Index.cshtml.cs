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

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Dashboard
{
    [Authorize(Policy = "MisGrantsViewer")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisGrant> Grants { get; set; } = new List<MisGrant>();

        public int TotalGrants { get; set; }
        public int ActiveGrantsCount { get; set; }
        public int ClosedGrantsCount { get; set; }
        public decimal TotalAwardAmount { get; set; }

        public List<string> ChartLabels { get; set; } = new List<string>();
        public List<decimal> ChartData { get; set; } = new List<decimal>();

        public async Task OnGetAsync()
        {
            Grants = await _context.MisGrants
                .Include(g => g.Milestones)
                .Include(g => g.MisDonor)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

            TotalGrants = Grants.Count;
            ActiveGrantsCount = Grants.Count(g => g.IsActive);
            ClosedGrantsCount = TotalGrants - ActiveGrantsCount;
            TotalAwardAmount = Grants.Sum(g => g.TotalAwardAmount);

            // Generate 6-Month Funding Trend Data
            for (int i = 5; i >= 0; i--)
            {
                var targetMonth = DateTime.UtcNow.AddMonths(-i);
                var startOfMonth = new DateTime(targetMonth.Year, targetMonth.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                ChartLabels.Add(targetMonth.ToString("MMM yyyy"));

                var monthTotal = Grants
                    .Where(g => g.StartDate >= startOfMonth && g.StartDate <= endOfMonth)
                    .Sum(g => g.TotalAwardAmount);

                ChartData.Add(monthTotal);
            }
        }
    }
}
