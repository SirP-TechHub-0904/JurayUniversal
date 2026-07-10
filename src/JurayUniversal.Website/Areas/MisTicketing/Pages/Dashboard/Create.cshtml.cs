using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Dashboard
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisEvent MisEvent { get; set; }

        public IActionResult OnGet()
        {
            var now = DateTime.Now;
            MisEvent = new MisEvent
            {
                // Today's date, rounded to current hour/minute with zero seconds
                StartDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ModelState.Remove("MisEvent.FinanceCategory");
            // ModelState.Remove("MisEvent.TicketTypes");
            // ModelState.Remove("MisEvent.CheckIns");
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            // 1. Create the associated Finance Category for tracking revenue
            var financeCategory = new MisFinanceCategory
            {
                Title = $"Event: {MisEvent.Title}",
                Description = $"Revenue for event: {MisEvent.Title}",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsCompulsory = false,
                Frequency = JurayUniversal.Domain.Enum.Enum.PaymentFrequency.OneTime,
                FixedAmount = null // Ticket types define their own dynamic amounts
            };
            
            _context.MisFinanceCategories.Add(financeCategory);
            await _context.SaveChangesAsync(); // Save to get the ID

            // 2. Save the Event and link the finance category
            MisEvent.MisFinanceCategoryId = financeCategory.Id;
            MisEvent.CreatedAt = DateTime.UtcNow;

            _context.MisEvents.Add(MisEvent);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Event successfully created! You can now add Ticket Types.";
            return RedirectToPage("./Manage", new { id = MisEvent.Id });
        }
    }
}
