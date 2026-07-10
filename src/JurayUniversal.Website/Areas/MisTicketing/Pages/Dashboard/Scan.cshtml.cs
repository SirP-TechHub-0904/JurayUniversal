using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Dashboard
{
    [Authorize]
    public class ScanModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ScanModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisEvent MisEvent { get; set; }

        [BindProperty]
        public string QrCode { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisEvent = await _context.MisEvents.FindAsync(id);
            if (MisEvent == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            MisEvent = await _context.MisEvents.FindAsync(id);
            if (MisEvent == null) return NotFound();

            if (string.IsNullOrWhiteSpace(QrCode))
            {
                TempData["error"] = "Please enter or scan a valid QR Code.";
                return Page();
            }

            // Find ticket matching the QR Code that belongs to THIS event
            var ticket = await _context.MisEventTickets
                .Include(t => t.MisTicketType)
                .FirstOrDefaultAsync(t => t.QrCodeText == QrCode && t.MisTicketType.MisEventId == id);

            if (ticket == null)
            {
                TempData["error"] = "INVALID TICKET! This QR code does not belong to this event.";
                return Page();
            }

            if (ticket.IsUsed)
            {
                TempData["error"] = $"TICKET ALREADY USED! Checked in on {ticket.ScannedAt?.ToString("f")}";
                return Page();
            }

            // Validate and Mark as used
            ticket.IsUsed = true;
            ticket.ScannedAt = DateTime.UtcNow;
            
            var user = await _userManager.GetUserAsync(User);
            ticket.ScannedByUserId = user?.Id;

            await _context.SaveChangesAsync();

            TempData["success"] = $"SUCCESS! Valid {ticket.MisTicketType.Name} ticket for {ticket.GuestName ?? "Guest"}.";
            ModelState.Clear();
            QrCode = string.Empty;

            return Page();
        }
    }
}
