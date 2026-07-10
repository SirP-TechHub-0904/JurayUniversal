using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard
{
    [Authorize]
    public class ResultsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ResultsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisElection Election { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public string PieChartDataJson { get; set; }
        public string TimeSeriesDataJson { get; set; }
        public string ExportTableDataJson { get; set; }
        public string FilteredRangeString { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Election = await _context.MisElections
                .Include(x => x.MisVoteCategories)
                    .ThenInclude(c => c.MisCandidates)
                        .ThenInclude(c => c.MisUserVotes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Election == null)
            {
                return NotFound();
            }

            var pieData = new Dictionary<long, object>();
            var timeSeriesData = new Dictionary<long, object>();
            var exportData = new List<object>();

            foreach (var category in Election.MisVoteCategories.OrderBy(c => c.SortOrder))
            {
                // Filter votes dynamically based on date range
                foreach (var candidate in category.MisCandidates)
                {
                    if (candidate.MisUserVotes != null)
                    {
                        var votesQuery = candidate.MisUserVotes.AsEnumerable();
                        if (StartDate.HasValue)
                        {
                            votesQuery = votesQuery.Where(v => v.DateCast >= StartDate.Value);
                        }
                        if (EndDate.HasValue)
                        {
                            votesQuery = votesQuery.Where(v => v.DateCast <= EndDate.Value);
                        }
                        candidate.MisUserVotes = votesQuery.ToList();
                    }
                }

                var candidates = category.MisCandidates.OrderByDescending(c => c.MisUserVotes?.Count() ?? 0).ToList();
                
                // Pie Chart Data
                pieData[category.Id] = new {
                    labels = candidates.Select(c => c.Title).ToArray(),
                    series = candidates.Select(c => c.MisUserVotes?.Count() ?? 0).ToArray()
                };

                // Time Series Data (Cumulative votes over time)
                var candidateSeriesList = new List<object>();
                foreach (var candidate in candidates)
                {
                    var votesOverTime = candidate.MisUserVotes?
                        .OrderBy(v => v.DateCast)
                        .Select((v, index) => new object[] { new DateTimeOffset(v.DateCast).ToUnixTimeMilliseconds(), index + 1 })
                        .ToList() ?? new List<object[]>();

                    candidateSeriesList.Add(new {
                        name = candidate.Title,
                        data = votesOverTime
                    });
                }
                timeSeriesData[category.Id] = candidateSeriesList;

                // Export Data (Raw Table format for Excel)
                var totalVotes = candidates.Sum(c => c.MisUserVotes?.Count() ?? 0);
                foreach (var candidate in candidates)
                {
                    int count = candidate.MisUserVotes?.Count() ?? 0;
                    double pct = totalVotes > 0 ? Math.Round((double)count / totalVotes * 100, 2) : 0;
                    exportData.Add(new {
                        Category = category.Title,
                        Candidate = candidate.Title,
                        Votes = count,
                        Percentage = $"{pct}%"
                    });
                }
            }

            PieChartDataJson = JsonSerializer.Serialize(pieData);
            TimeSeriesDataJson = JsonSerializer.Serialize(timeSeriesData);
            ExportTableDataJson = JsonSerializer.Serialize(exportData);

            if (StartDate.HasValue || EndDate.HasValue)
            {
                FilteredRangeString = $"{(StartDate.HasValue ? StartDate.Value.ToString("g") : "Start")} - {(EndDate.HasValue ? EndDate.Value.ToString("g") : "End")}";
            }

            return Page();
        }
    }
}
