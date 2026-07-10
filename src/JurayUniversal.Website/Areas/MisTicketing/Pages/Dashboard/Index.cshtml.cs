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

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisEvent> Events { get; set; } = new List<MisEvent>();

        // Analytics Properties
        public int TotalActiveEvents { get; set; }
        public int UpcomingEventsCount { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalTicketRevenue { get; set; }

        public int UsedTicketsCount { get; set; }
        public int UnusedTicketsCount { get; set; }

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

            var eventQuery = _context.MisEvents
                .Include(e => e.TicketTypes)
                .OrderByDescending(e => e.CreatedAt)
                .AsQueryable();

            var ticketQuery = _context.MisEventTickets
                .Include(t => t.MisTicketType)
                .AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                eventQuery = eventQuery.Where(e => e.CreatedAt >= StartDate.Value && e.CreatedAt <= EndDate.Value);
                ticketQuery = ticketQuery.Where(t => t.CreatedAt >= StartDate.Value && t.CreatedAt <= EndDate.Value);
            }

            Events = await eventQuery.ToListAsync();

            var now = DateTime.UtcNow;
            TotalActiveEvents = Events.Count;
            var today = now.Date;
            UpcomingEventsCount = Events.Count(e => e.EndDate != null ? e.EndDate >= today : e.StartDate >= today);

            var allTickets = await ticketQuery.ToListAsync();

            TotalTicketsSold = allTickets.Count(t => t.IsUsed || t.TransactionReference == "FREE" || t.TransactionReference != null); 
            TotalTicketRevenue = allTickets.Sum(t => t.MisTicketType?.Price ?? 0);

            UsedTicketsCount = allTickets.Count(t => t.IsUsed);
            UnusedTicketsCount = allTickets.Count(t => !t.IsUsed);

            // Chart Trend Logic
            var chartStart = StartDate ?? DateTime.UtcNow.Date.AddDays(-29);
            var chartEnd = EndDate ?? DateTime.UtcNow.Date;
            var totalDays = (chartEnd - chartStart).TotalDays;

            if (totalDays > 365) totalDays = 365;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = chartStart.AddDays(i).Date;
                ChartLabels.Add(currentDate.ToString("MMM dd"));
                
                var dailySum = allTickets.Count(t => t.CreatedAt.Date == currentDate);
                ChartData.Add(dailySum);
            }
        }
    }
}
