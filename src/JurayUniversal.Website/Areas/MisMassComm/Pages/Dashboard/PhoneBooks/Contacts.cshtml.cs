using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.PhoneBooks
{
    public class ContactsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ContactsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisPhoneBook PhoneBook { get; set; } = null!;
        public IList<MisContact> Contacts { get; set; } = new List<MisContact>();

        [BindProperty]
        public MisContact NewContact { get; set; } = new MisContact();

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var pb = await _context.MisPhoneBooks.FirstOrDefaultAsync(p => p.Id == id && p.UserId == null);
            if (pb == null) return NotFound();
            PhoneBook = pb;

            Contacts = await _context.MisContacts
                .Where(c => c.MisPhoneBookId == id)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync(long id)
        {
            var pb = await _context.MisPhoneBooks.FirstOrDefaultAsync(p => p.Id == id && p.UserId == null);
            if (pb == null) return NotFound();

            if (!string.IsNullOrEmpty(NewContact.PhoneNumber))
            {
                NewContact.MisPhoneBookId = id;
                NewContact.IsActive = true;
                _context.MisContacts.Add(NewContact);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Contact added successfully.";
            }

            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(long contactId, long id)
        {
            var contact = await _context.MisContacts
                .Include(c => c.MisPhoneBook)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.MisPhoneBook.UserId == null);
            
            if (contact != null)
            {
                contact.IsActive = !contact.IsActive;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = id });
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(long contactId, long id)
        {
            var contact = await _context.MisContacts
                .Include(c => c.MisPhoneBook)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.MisPhoneBook.UserId == null);
            
            if (contact != null)
            {
                _context.MisContacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = id });
        }
    }
}
