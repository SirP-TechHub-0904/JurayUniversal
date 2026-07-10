using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.PhoneBooks
{
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CreateModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            PhoneBook.UserId = user.Id;
            PhoneBook.CreatedAt = DateTime.UtcNow;

            _context.MisPhoneBooks.Add(PhoneBook);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Phone Book created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
