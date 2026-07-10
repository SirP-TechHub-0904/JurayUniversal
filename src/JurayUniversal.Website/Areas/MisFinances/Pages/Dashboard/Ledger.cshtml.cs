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
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class LedgerModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public LedgerModel(DashboardDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public MisFinanceCategory Category { get; set; }
        public List<MisFinanceTransaction> PaidTransactions { get; set; } = new();
        public List<Profile> DefaulterUsers { get; set; } = new();
        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PaidUserCount { get; set; }
        public int DefaulterCount { get; set; }

        public string CurrentPeriodLabel { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Category = await _context.MisFinanceCategories.FindAsync(id);
            if (Category == null) return NotFound();

            PaidTransactions = await _context.MisFinanceTransactions
                .Include(t => t.UserProfile)
                .Where(t => t.MisFinanceCategoryId == id && t.Status == PaymentStatus.Success)
                .OrderByDescending(t => t.PaidAt)
                .ToListAsync();

            TotalTransactions = PaidTransactions.Count;
            TotalRevenue = PaidTransactions.Sum(t => t.Amount);
            PaidUserCount = PaidTransactions.Select(t => t.UserId).Distinct().Count();

            if (Category.IsCompulsory)
            {
                // Determine the current billing period based on frequency
                var (periodStart, periodEnd, periodLabel) = GetCurrentPeriod(Category.Frequency);
                CurrentPeriodLabel = periodLabel;

                var paidUserIds = PaidTransactions
                    .Where(t => t.PaidAt.HasValue && t.PaidAt.Value >= periodStart && t.PaidAt.Value < periodEnd)
                    .Select(t => t.UserId)
                    .Distinct()
                    .ToList();

                PaidUserCount = paidUserIds.Count;

                var allUsers = await _context.Users.ToListAsync();
                DefaulterUsers = allUsers.Where(u => !paidUserIds.Contains(u.Id)).ToList();
                DefaulterCount = DefaulterUsers.Count;
            }

            return Page();
        }

        private (DateTime start, DateTime end, string label) GetCurrentPeriod(PaymentFrequency frequency)
        {
            var now = DateTime.UtcNow;
            return frequency switch
            {
                PaymentFrequency.Daily => (
                    now.Date, now.Date.AddDays(1), now.ToString("MMMM dd, yyyy")),
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
                _ => (DateTime.MinValue, DateTime.MaxValue, "All Time")
            };
        }

        public async Task<IActionResult> OnPostRecordManualAsync(
            long CategoryId, string DonorName, string DonorEmail, decimal Amount, string Notes)
        {
            var category = await _context.MisFinanceCategories.FindAsync(CategoryId);
            if (category == null) return NotFound();

            var tx = new MisFinanceTransaction
            {
                MisFinanceCategoryId = CategoryId,
                DonorName = DonorName,
                DonorEmail = DonorEmail,
                Amount = Amount,
                Notes = Notes,
                TransactionReference = $"MANUAL-{System.DateTime.UtcNow.Ticks}",
                Status = PaymentStatus.Success,
                PaidAt = System.DateTime.UtcNow
            };

            _context.MisFinanceTransactions.Add(tx);
            await _context.SaveChangesAsync();
            StatusMessage = $"Manual payment of {Amount:N2} recorded successfully.";
            return RedirectToPage(new { id = CategoryId });
        }
    }
}
