using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly UserManager<Profile> _userManager;

        public LedgerModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public MisFinanceCategory Category { get; set; }
        public List<MisFinanceTransaction> PaidTransactions { get; set; } = new();
        public List<Profile> DefaulterUsers { get; set; } = new();
        public List<SelectListItem> UserOptions { get; set; } = new();

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
            PaidUserCount = PaidTransactions.Select(t => t.UserId).Where(uid => !string.IsNullOrEmpty(uid)).Distinct().Count();

            // Fetch users excluding SuperAdmin role
            var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var superAdminIds = superAdmins.Select(u => u.Id).ToHashSet();

            var eligibleUsers = await _context.Users
                .Where(u => !superAdminIds.Contains(u.Id))
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();

            UserOptions = eligibleUsers.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.FirstName} {u.LastName} ({u.Email})".Trim()
            }).ToList();

            if (Category.IsCompulsory)
            {
                // Determine current billing period
                var (periodStart, periodEnd, periodLabel) = GetCurrentPeriod(Category.Frequency);
                CurrentPeriodLabel = periodLabel;

                var paidUserIds = PaidTransactions
                    .Where(t => t.PaidAt.HasValue && t.PaidAt.Value >= periodStart && t.PaidAt.Value < periodEnd && !string.IsNullOrEmpty(t.UserId))
                    .Select(t => t.UserId)
                    .Distinct()
                    .ToHashSet();

                PaidUserCount = paidUserIds.Count;
                DefaulterUsers = eligibleUsers.Where(u => !paidUserIds.Contains(u.Id)).ToList();
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
            long CategoryId, string SelectedUserId, decimal Amount, string Notes)
        {
            var category = await _context.MisFinanceCategories.FindAsync(CategoryId);
            if (category == null) return NotFound();

            string donorName = "Manual Payment";
            string donorEmail = null;

            if (!string.IsNullOrEmpty(SelectedUserId))
            {
                var user = await _context.Users.FindAsync(SelectedUserId);
                if (user != null)
                {
                    donorName = $"{user.FirstName} {user.LastName}".Trim();
                    donorEmail = user.Email;
                }
            }

            var tx = new MisFinanceTransaction
            {
                MisFinanceCategoryId = CategoryId,
                UserId = SelectedUserId,
                DonorName = donorName,
                DonorEmail = donorEmail,
                Amount = Amount,
                Notes = Notes,
                PaymentProvider = "Manual / Offline",
                TransactionReference = $"MANUAL-{DateTime.UtcNow.Ticks}",
                Status = PaymentStatus.Success,
                PaidAt = DateTime.UtcNow
            };

            _context.MisFinanceTransactions.Add(tx);
            await _context.SaveChangesAsync();
            StatusMessage = $"Manual payment of ₦{Amount:N2} recorded for {donorName} successfully.";
            return RedirectToPage(new { id = CategoryId });
        }
    }
}
