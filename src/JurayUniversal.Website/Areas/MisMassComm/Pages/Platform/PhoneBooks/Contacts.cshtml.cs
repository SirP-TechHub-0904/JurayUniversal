using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.PhoneBooks
{
    public class ContactsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ContactsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisPhoneBook PhoneBook { get; set; } = null!;
        public IList<MisContact> Contacts { get; set; } = new List<MisContact>();

        [BindProperty]
        public MisContact NewContact { get; set; } = new MisContact();

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var pb = await _context.MisPhoneBooks.FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var pb = await _context.MisPhoneBooks.FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var contact = await _context.MisContacts
                .Include(c => c.MisPhoneBook)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.MisPhoneBook.UserId == user.Id);
            
            if (contact != null)
            {
                contact.IsActive = !contact.IsActive;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = id });
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(long contactId, long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var contact = await _context.MisContacts
                .Include(c => c.MisPhoneBook)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.MisPhoneBook.UserId == user.Id);
            
            if (contact != null)
            {
                _context.MisContacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = id });
        }
    }
}
