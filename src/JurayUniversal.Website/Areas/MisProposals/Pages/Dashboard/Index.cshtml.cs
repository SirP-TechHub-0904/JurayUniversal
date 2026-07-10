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

namespace JurayUniversal.Website.Areas.MisProposals.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisProposal> Proposals { get; set; } = new List<MisProposal>();

        public int TotalPitches { get; set; }
        public int UnderReviewCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DateRange { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string> ChartLabels { get; set; } = new();
        public List<int> ChartData { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(DateRange))
            {
                var dates = DateRange.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParse(dates[0], out DateTime start)) StartDate = start;
                    if (DateTime.TryParse(dates[1], out DateTime end)) EndDate = end.AddDays(1).AddTicks(-1);
                }
            }

            var query = _context.MisProposals
                .Include(p => p.UserProfile)
                .AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= StartDate.Value && p.CreatedAt <= EndDate.Value);
            }

            Proposals = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            TotalPitches = Proposals.Count;
            UnderReviewCount = Proposals.Count(p => p.Status == JurayUniversal.Domain.Enum.Enum.ProposalStatus.UnderReview || p.Status == JurayUniversal.Domain.Enum.Enum.ProposalStatus.Submitted);
            ApprovedCount = Proposals.Count(p => p.Status == JurayUniversal.Domain.Enum.Enum.ProposalStatus.Approved);
            RejectedCount = Proposals.Count(p => p.Status == JurayUniversal.Domain.Enum.Enum.ProposalStatus.Rejected);

            // Chart Trend Logic
            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) totalDays = 365;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));
                
                var dailySum = Proposals.Count(p => p.CreatedAt.Date == currentDate);
                ChartData.Add(dailySum);
            }
        }
    }
}
