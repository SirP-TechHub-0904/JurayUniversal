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

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public int TotalSchemas { get; set; }
        public int TotalSubmissionsAllTime { get; set; }
        public int SubmissionsLast7Days { get; set; }
        public int OnTimeRate7Days { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DateRange { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<int> OnTimeData { get; set; } = new List<int>();
        public List<int> LateData { get; set; } = new List<int>();
        public List<string> ChartLabels { get; set; } = new List<string>();

        // New Analytics Properties
        public IList<MisReportSubmission> RecentSubmissions { get; set; } = new List<MisReportSubmission>();
        public IList<string> SchemaLabels { get; set; } = new List<string>();
        public IList<int> SchemaData { get; set; } = new List<int>();
        
        public IList<dynamic> TopReporters { get; set; } = new List<dynamic>();

        public async Task<IActionResult> OnGetAsync()
        {
            TotalSchemas = await _context.MisReportSchemas.CountAsync(x => x.IsActive);
            
            if (!string.IsNullOrEmpty(DateRange))
            {
                var dates = DateRange.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParse(dates[0], out DateTime start)) StartDate = start;
                    if (DateTime.TryParse(dates[1], out DateTime end)) EndDate = end.AddDays(1).AddTicks(-1);
                }
            }

            var query = _context.MisReportSubmissions
                .Include(s => s.UserProfile)
                .Include(s => s.MisReportSchema)
                .AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(s => s.SubmittedAt >= StartDate.Value && s.SubmittedAt <= EndDate.Value);
            }

            var filteredSubmissions = await query.ToListAsync();

            TotalSubmissionsAllTime = filteredSubmissions.Count;
            SubmissionsLast7Days = TotalSubmissionsAllTime;

            if (SubmissionsLast7Days > 0)
            {
                int onTimeCount = filteredSubmissions.Count(s => s.Status == "OnTime");
                OnTimeRate7Days = (int)Math.Round((double)onTimeCount / SubmissionsLast7Days * 100);
            }

            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) totalDays = 365;

            // Prepare Chart Data
            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));

                var subsForDay = filteredSubmissions.Where(s => s.SubmittedAt.Date == currentDate).ToList();
                OnTimeData.Add(subsForDay.Count(s => s.Status == "OnTime"));
                LateData.Add(subsForDay.Count(s => s.Status == "Late"));
            }

            // 1. Recent Submissions
            RecentSubmissions = await _context.MisReportSubmissions
                .Include(s => s.UserProfile)
                .Include(s => s.MisReportSchema)
                .OrderByDescending(s => s.SubmittedAt)
                .Take(5)
                .ToListAsync();

            // 2. Schema Distribution
            var schemaGroupings = filteredSubmissions
                .GroupBy(s => s.MisReportSchema?.Name)
                .Select(g => new { Title = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            foreach(var item in schemaGroupings)
            {
                SchemaLabels.Add(item.Title ?? "Unknown");
                SchemaData.Add(item.Count);
            }

            // 3. Top Reporters
            var topGroupings = filteredSubmissions
                .GroupBy(s => new { s.UserProfile?.FirstName, s.UserProfile?.LastName })
                .Select(g => new { Name = g.Key.FirstName + " " + g.Key.LastName, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            foreach(var item in topGroupings)
            {
                TopReporters.Add(new { Name = item.Name, Count = item.Count });
            }

            return Page();
        }
    }
}
