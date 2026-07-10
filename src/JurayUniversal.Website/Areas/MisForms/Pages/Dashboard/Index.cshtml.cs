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

namespace JurayUniversal.Website.Areas.MisForms.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisForm> Forms { get; set; }

        public int TotalForms { get; set; }
        public int ActiveForms { get; set; }
        public int DraftForms { get; set; }
        public int TotalSubmissions { get; set; }
        public double AvgResponses { get; set; }

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

            var query = _context.MisForms
                .Include(f => f.Responses)
                .AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(f => f.CreatedAt >= StartDate.Value && f.CreatedAt <= EndDate.Value);
            }

            Forms = await query
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            TotalForms = Forms.Count;
            ActiveForms = Forms.Count(f => f.IsPublished);
            DraftForms = Forms.Count(f => !f.IsPublished);
            
            TotalSubmissions = Forms.Sum(f => f.Responses?.Count ?? 0);
            AvgResponses = TotalForms > 0 ? Math.Round((double)TotalSubmissions / TotalForms, 1) : 0;

            // Chart Trend Logic
            var allResponses = await _context.MisFormResponses.ToListAsync();
            if (StartDate.HasValue && EndDate.HasValue)
            {
                allResponses = allResponses.Where(r => r.SubmittedAt >= StartDate.Value && r.SubmittedAt <= EndDate.Value).ToList();
            }

            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) totalDays = 365;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));
                
                var dailySum = allResponses.Count(r => r.SubmittedAt.Date == currentDate);
                ChartData.Add(dailySum);
            }
        }
    }
}
