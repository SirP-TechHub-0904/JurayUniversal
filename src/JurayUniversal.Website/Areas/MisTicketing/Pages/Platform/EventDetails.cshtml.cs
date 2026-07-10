using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Platform
{
    [AllowAnonymous]
    public class EventDetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EventDetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisEvent MisEvent { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisEvent = await _context.MisEvents
                .Include(e => e.TicketTypes)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (MisEvent == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long eventId, long ticketTypeId)
        {
            // Simple validation
            var ticketType = await _context.MisTicketTypes
                .FirstOrDefaultAsync(t => t.Id == ticketTypeId && t.MisEventId == eventId && t.IsActive);

            if (ticketType == null)
            {
                TempData["error"] = "Invalid ticket selection.";
                return RedirectToPage(new { id = eventId });
            }

            // Redirect to Checkout page to handle user details and payment
            return RedirectToPage("./Checkout", new { eventId = eventId, ticketTypeId = ticketTypeId });
        }
    }
}
