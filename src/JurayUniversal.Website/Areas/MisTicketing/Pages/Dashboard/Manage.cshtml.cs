using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Dashboard
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ManageModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisEvent MisEvent { get; set; }
        
        [BindProperty]
        public MisTicketType NewTicket { get; set; }

        public IList<MisTicketType> TicketTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisEvent = await _context.MisEvents
                .Include(e => e.MisFinanceCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (MisEvent == null)
            {
                return NotFound();
            }

            TicketTypes = await _context.MisTicketTypes
                .Where(t => t.MisEventId == id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddTicketAsync(long id)
        {
            // ModelState.Remove("NewTicket.MisEvent");
            // ModelState.Remove("NewTicket.Tickets");
            // if (!ModelState.IsValid)
            // {
            //     return RedirectToPage(new { id = id });
            // }
            NewTicket.MisEventId = id;
            NewTicket.IsActive = true;
            NewTicket.SoldCount = 0;

            _context.MisTicketTypes.Add(NewTicket);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Ticket Type added successfully!";
            return RedirectToPage(new { id = id });
        }
        
        public async Task<IActionResult> OnPostDeleteTicketAsync(long ticketId, long eventId)
        {
            var ticket = await _context.MisTicketTypes.FindAsync(ticketId);
            if (ticket != null && ticket.SoldCount == 0) // Only allow delete if no one bought it
            {
                _context.MisTicketTypes.Remove(ticket);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Ticket Type deleted.";
            }
            else if (ticket != null)
            {
                TempData["ErrorMessage"] = "Cannot delete a ticket type that already has sales. You can only deactivate it.";
            }

            return RedirectToPage(new { id = eventId });
        }
    }
}
