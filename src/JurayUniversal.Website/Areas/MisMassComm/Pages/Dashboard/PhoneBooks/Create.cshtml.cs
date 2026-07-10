using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.PhoneBooks
{
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisPhoneBook PhoneBook { get; set; } = new MisPhoneBook();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PhoneBook.UserId = null; // Global Phone Book
            PhoneBook.CreatedAt = DateTime.UtcNow;

            _context.MisPhoneBooks.Add(PhoneBook);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Global Phone Book created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
