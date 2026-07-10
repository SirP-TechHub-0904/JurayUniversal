using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        // Core Metrics
        public decimal CompanyWalletBalance { get; set; }
        public int TotalSmsSentGlobal { get; set; }
        public int TotalActiveContactsGlobal { get; set; }
        public int TotalCampaignsGlobal { get; set; }

        // Delivery Analytics
        public int TotalSmsDelivered { get; set; }
        public int TotalSmsFailed { get; set; }
        public int TotalSmsPending { get; set; }

        // Wallet Analytics
        public decimal TotalDeposits30Days { get; set; }
        public decimal TotalDeductions30Days { get; set; }

        // Recent Activity
        public IList<MisSmsCampaign> RecentCampaigns { get; set; } = new List<MisSmsCampaign>();

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            CompanyWalletBalance = settings?.SmsCreditBalance ?? 0m;

            // Contacts & Campaigns
            TotalActiveContactsGlobal = await _context.MisContacts.CountAsync(c => c.IsActive);
            TotalCampaignsGlobal = await _context.MisSmsCampaigns.CountAsync();

            // Delivery Stats
            TotalSmsDelivered = await _context.MisSmsRecipients.CountAsync(r => r.DeliveryStatus == Domain.Enum.Enum.MisSmsDeliveryStatus.Sent);
            TotalSmsFailed = await _context.MisSmsRecipients.CountAsync(r => r.DeliveryStatus == Domain.Enum.Enum.MisSmsDeliveryStatus.Failed);
            TotalSmsPending = await _context.MisSmsRecipients.CountAsync(r => r.DeliveryStatus == Domain.Enum.Enum.MisSmsDeliveryStatus.Pending);
            TotalSmsSentGlobal = TotalSmsDelivered + TotalSmsFailed + TotalSmsPending; // Total attempted

            // Wallet Stats (Last 30 Days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var recentTxns = await _context.MisSmsTransactions
                .Where(t => t.CreatedAt >= thirtyDaysAgo)
                .ToListAsync();

            TotalDeposits30Days = recentTxns.Where(t => t.TransactionType == Domain.Enum.Enum.MisWalletTransactionType.Credit).Sum(t => t.Amount);
            TotalDeductions30Days = recentTxns.Where(t => t.TransactionType == Domain.Enum.Enum.MisWalletTransactionType.Debit).Sum(t => t.Amount);

            // Recent Campaigns
            RecentCampaigns = await _context.MisSmsCampaigns
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .ToListAsync();

            return Page();
        }
    }
}
