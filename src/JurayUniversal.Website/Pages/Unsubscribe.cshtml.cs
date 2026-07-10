using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Pages
{
    [AllowAnonymous]
    public class UnsubscribeModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public UnsubscribeModel(DashboardDbContext context)
        {
            _context = context;
        }

        public bool Success { get; set; }
        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? token)
        {
            if (!token.HasValue || token.Value == Guid.Empty)
            {
                Success = false;
                return Page();
            }

            // Find the recipient by tracking token
            var recipient = await _context.MisNewsletterRecipients
                .FirstOrDefaultAsync(r => r.TrackingGuid == token.Value);

            if (recipient == null)
            {
                Success = false;
                return Page();
            }

            Email = recipient.EmailAddress;

            // Find all active contacts matching this email across all lists and set IsActive to false
            var contacts = await _context.MisNewsletterContacts
                .Where(c => c.Email == Email && c.IsActive)
                .ToListAsync();

            if (contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contact.IsActive = false;
                }
                
                await _context.SaveChangesAsync();
                Success = true;
            }
            else
            {
                // Either no such email exists, or it's already unsubscribed
                Success = false;
            }

            return Page();
        }
    }
}
