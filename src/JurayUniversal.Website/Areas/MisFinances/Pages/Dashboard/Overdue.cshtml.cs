using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class OverdueModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public OverdueModel(DashboardDbContext context)
        {
            _context = context;
        }

        public List<OverdueCategoryViewModel> OverdueCategories { get; set; } = new();
        public int TotalDefaulters { get; set; }
        public int TotalCompulsoryCategories { get; set; }
        public int FullyPaidCategories { get; set; }

        public async Task OnGetAsync()
        {
            // Get all active, compulsory categories
            var compulsoryCategories = await _context.MisFinanceCategories
                .Where(c => c.IsActive && c.IsCompulsory)
                .ToListAsync();

            TotalCompulsoryCategories = compulsoryCategories.Count;

            // Get all registered users
            var allUsers = await _context.Users.ToListAsync();

            foreach (var cat in compulsoryCategories)
            {
                // Determine the current billing period
                var (periodStart, periodEnd, periodLabel) = GetCurrentPeriod(cat.Frequency);

                // Get users who have a successful payment in this period
                var paidUserIds = await _context.MisFinanceTransactions
                    .Where(t => t.MisFinanceCategoryId == cat.Id
                                && t.Status == PaymentStatus.Success
                                && t.PaidAt.HasValue
                                && t.PaidAt.Value >= periodStart
                                && t.PaidAt.Value < periodEnd)
                    .Select(t => t.UserId)
                    .Distinct()
                    .ToListAsync();

                var defaulters = allUsers.Where(u => !paidUserIds.Contains(u.Id)).ToList();

                var vm = new OverdueCategoryViewModel
                {
                    Category = cat,
                    PeriodLabel = periodLabel,
                    PeriodStart = periodStart,
                    PeriodEnd = periodEnd,
                    PaidCount = paidUserIds.Count,
                    DefaulterCount = defaulters.Count,
                    TotalMembers = allUsers.Count,
                    Defaulters = defaulters,
                    CompliancePercent = allUsers.Count > 0
                        ? (int)Math.Round((double)paidUserIds.Count / allUsers.Count * 100)
                        : 0
                };

                OverdueCategories.Add(vm);
            }

            TotalDefaulters = OverdueCategories.Sum(c => c.DefaulterCount);
            FullyPaidCategories = OverdueCategories.Count(c => c.DefaulterCount == 0);
        }

        private (DateTime start, DateTime end, string label) GetCurrentPeriod(PaymentFrequency frequency)
        {
            var now = DateTime.UtcNow;
            return frequency switch
            {
                PaymentFrequency.Daily => (
                    now.Date,
                    now.Date.AddDays(1),
                    now.ToString("MMMM dd, yyyy")),

                PaymentFrequency.Weekly => (
                    now.Date.AddDays(-(int)now.DayOfWeek),
                    now.Date.AddDays(7 - (int)now.DayOfWeek),
                    $"Week of {now.Date.AddDays(-(int)now.DayOfWeek):MMM dd} – {now.Date.AddDays(6 - (int)now.DayOfWeek):MMM dd, yyyy}"),

                PaymentFrequency.Monthly => (
                    new DateTime(now.Year, now.Month, 1),
                    new DateTime(now.Year, now.Month, 1).AddMonths(1),
                    now.ToString("MMMM yyyy")),

                PaymentFrequency.Yearly => (
                    new DateTime(now.Year, 1, 1),
                    new DateTime(now.Year + 1, 1, 1),
                    now.Year.ToString()),

                // OneTime — all time
                _ => (
                    DateTime.MinValue,
                    DateTime.MaxValue,
                    "All Time")
            };
        }
    }

    public class OverdueCategoryViewModel
    {
        public MisFinanceCategory Category { get; set; }
        public string PeriodLabel { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int PaidCount { get; set; }
        public int DefaulterCount { get; set; }
        public int TotalMembers { get; set; }
        public List<Profile> Defaulters { get; set; } = new();
        public int CompliancePercent { get; set; }
    }
}
