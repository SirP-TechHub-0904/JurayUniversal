using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.PhoneBooks
{
    public class ImportModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ImportModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisPhoneBook PhoneBook { get; set; } = null!;

        [BindProperty]
        public string? BulkNumbers { get; set; }

        [BindProperty]
        public IFormFile? CsvFile { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var pb = await _context.MisPhoneBooks.FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);
            if (pb == null) return NotFound();
            PhoneBook = pb;

            return Page();
        }

        public async Task<IActionResult> OnPostBulkPasteAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(BulkNumbers))
            {
                var numbers = Regex.Split(BulkNumbers, @"\s+|,|;|\n|\r")
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Select(n => n.Trim())
                    .Distinct()
                    .ToList();

                foreach (var num in numbers)
                {
                    // basic check
                    if (num.Length >= 8)
                    {
                        var c = new MisContact { MisPhoneBookId = id, PhoneNumber = num, IsActive = true };
                        _context.MisContacts.Add(c);
                    }
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully imported {numbers.Count} contacts.";
            }

            return RedirectToPage("./Contacts", new { id = id });
        }

        public async Task<IActionResult> OnPostCsvAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (CsvFile != null && CsvFile.Length > 0)
            {
                int count = 0;
                using (var reader = new StreamReader(CsvFile.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        var line = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var parts = line.Split(',');
                            var phone = parts.Length > 0 ? parts[0].Trim() : "";
                            var name = parts.Length > 1 ? parts[1].Trim() : "";

                            if (phone.Length >= 8)
                            {
                                _context.MisContacts.Add(new MisContact { MisPhoneBookId = id, PhoneNumber = phone, Name = name, IsActive = true });
                                count++;
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully imported {count} contacts from CSV.";
            }
            
            return RedirectToPage("./Contacts", new { id = id });
        }
    }
}
