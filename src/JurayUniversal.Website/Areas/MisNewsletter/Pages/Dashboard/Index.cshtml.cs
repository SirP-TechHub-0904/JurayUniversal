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

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public decimal TotalSpent { get; set; }
        public int TotalSent { get; set; }
        public int PendingCampaigns { get; set; }
        public int FailedDeliveries { get; set; }

        public List<decimal> MonthlyVolumeSent { get; set; } = new List<decimal>(new decimal[12]);
        public List<decimal> MonthlyVolumeFailed { get; set; } = new List<decimal>(new decimal[12]);
        public List<string> YearlyLabels { get; set; } = new List<string>();
        public List<decimal> YearlySpend { get; set; } = new List<decimal>();

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            var queryStart = startDate ?? DateTime.MinValue;
            var queryEnd = endDate ?? DateTime.MaxValue;
            if (endDate.HasValue) queryEnd = endDate.Value.AddDays(1).AddTicks(-1);

            // Metric Cards
            TotalSpent = await _context.MisNewsletterPlanTransactions
                .Where(t => t.IsGlobalPurchase == true && t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success && t.CreatedAt >= queryStart && t.CreatedAt <= queryEnd)
                .SumAsync(t => t.Amount);

            var campaigns = await _context.MisNewsletterCampaigns
                .Where(c => c.CreatedAt >= queryStart && c.CreatedAt <= queryEnd)
                .ToListAsync();

            TotalSent = campaigns.Where(c => c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Completed || c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Sending).Sum(c => c.TotalRecipients);
            PendingCampaigns = campaigns.Count(c => c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Pending || c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Scheduled);
            FailedDeliveries = campaigns.Where(c => c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Failed).Sum(c => c.TotalRecipients); // Simplified logic for failed count

            // Monthly Volume Chart (Current Year)
            int currentYear = DateTime.UtcNow.Year;
            var thisYearCampaigns = await _context.MisNewsletterCampaigns
                .Where(c => c.CreatedAt.Year == currentYear)
                .ToListAsync();

            for (int month = 1; month <= 12; month++)
            {
                var monthCampaigns = thisYearCampaigns.Where(c => c.CreatedAt.Month == month).ToList();
                MonthlyVolumeSent[month - 1] = monthCampaigns.Where(c => c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Completed || c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Sending).Sum(c => c.TotalRecipients);
                MonthlyVolumeFailed[month - 1] = monthCampaigns.Where(c => c.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Failed).Sum(c => c.TotalRecipients);
            }

            // Yearly Spend Chart
            var allPurchases = await _context.MisNewsletterPlanTransactions
                .Where(t => t.IsGlobalPurchase == true && t.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success)
                .ToListAsync();

            var spendByYear = allPurchases
                .GroupBy(t => t.CreatedAt.Year)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var yearGroup in spendByYear)
            {
                YearlyLabels.Add(yearGroup.Key.ToString());
                YearlySpend.Add(yearGroup.Sum(t => t.Amount));
            }

            return Page();
        }
    }
}