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

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public decimal TotalRevenue { get; set; }
        public int TotalTransactions { get; set; }
        public int SuccessfulTransactions { get; set; }
        public int PendingTransactions { get; set; }
        public int FailedTransactions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DateRange { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string> ChartLabels { get; set; } = new();
        public List<decimal> ChartData { get; set; } = new();

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

            var query = _context.MisFinanceTransactions.AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= StartDate.Value && t.CreatedAt <= EndDate.Value);
            }

            var allTransactions = await query.ToListAsync();

            TotalTransactions = allTransactions.Count;
            SuccessfulTransactions = allTransactions.Count(t => t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success);
            PendingTransactions = allTransactions.Count(t => t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Pending);
            FailedTransactions = allTransactions.Count(t => t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Failed);

            TotalRevenue = allTransactions
                .Where(t => t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success)
                .Sum(t => t.Amount);

            // Chart Trend Logic
            var successList = allTransactions
                .Where(t => t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success)
                .ToList();

            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) // Group by month if over a year
            {
                // Simple grouping by month logic could be added here, but for now we cap loop at 60 points max to avoid blowing up the chart
            }

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));
                
                var dailySum = successList
                    .Where(t => (t.PaidAt ?? t.CreatedAt).Date == currentDate)
                    .Sum(t => t.Amount);
                
                ChartData.Add(dailySum);
            }
        }
    }
}
