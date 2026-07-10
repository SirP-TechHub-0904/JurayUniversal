using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Lists
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisNewsletterList NewsletterList { get; set; }
        public IList<MisNewsletterContact> Contacts { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var userId = _userManager.GetUserId(User);
            NewsletterList = await _context.MisNewsletterLists
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (NewsletterList == null)
            {
                return NotFound();
            }

            Contacts = await _context.MisNewsletterContacts
                .Where(c => c.MisNewsletterListId == id)
                .OrderByDescending(c => c.DateAdded)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostImportCsvAsync(long id, IFormFile csvFile)
        {
            var userId = _userManager.GetUserId(User);
            var list = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
            
            if (list == null) return NotFound();

            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["error"] = "Please select a valid CSV file.";
                return RedirectToPage(new { id });
            }

            int importedCount = 0;
            int skippedCount = 0;

            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    bool isFirstLine = true;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        if (isFirstLine)
                        {
                            // Try to skip header row if it looks like one
                            if (line.ToLower().Contains("name") || line.ToLower().Contains("email"))
                            {
                                isFirstLine = false;
                                continue;
                            }
                            isFirstLine = false;
                        }

                        var values = line.Split(',');
                        if (values.Length >= 2)
                        {
                            string name = values[0].Trim();
                            string email = values[1].Trim();

                            // Basic email validation
                            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                            {
                                // Check if email already exists in THIS list
                                bool exists = await _context.MisNewsletterContacts
                                    .AnyAsync(c => c.MisNewsletterListId == id && c.Email == email);

                                if (!exists)
                                {
                                    var contact = new MisNewsletterContact
                                    {
                                        MisNewsletterListId = id,
                                        Name = name,
                                        Email = email,
                                        DateAdded = DateTime.UtcNow
                                    };
                                    _context.MisNewsletterContacts.Add(contact);
                                    importedCount++;
                                }
                                else
                                {
                                    skippedCount++;
                                }
                            }
                            else
                            {
                                skippedCount++;
                            }
                        }
                    }
                }

                if (importedCount > 0)
                {
                    await _context.SaveChangesAsync();
                }

                TempData["success"] = $"Imported {importedCount} contacts successfully. Skipped {skippedCount} duplicates or invalid entries.";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error importing CSV: {ex.Message}";
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostAddSingleAsync(long id, string name, string email)
        {
            var userId = _userManager.GetUserId(User);
            var list = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
            
            if (list == null) return NotFound();

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                TempData["error"] = "Invalid email address.";
                return RedirectToPage(new { id });
            }

            bool exists = await _context.MisNewsletterContacts
                .AnyAsync(c => c.MisNewsletterListId == id && c.Email == email);

            if (exists)
            {
                TempData["error"] = "This email is already in the list.";
                return RedirectToPage(new { id });
            }

            var contact = new MisNewsletterContact
            {
                MisNewsletterListId = id,
                Name = name ?? "Subscriber",
                Email = email,
                DateAdded = DateTime.UtcNow
            };

            _context.MisNewsletterContacts.Add(contact);
            await _context.SaveChangesAsync();

            TempData["success"] = "Contact added successfully.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteContactAsync(long id, long contactId)
        {
            var userId = _userManager.GetUserId(User);
            var list = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
            
            if (list == null) return NotFound();

            var contact = await _context.MisNewsletterContacts.FirstOrDefaultAsync(c => c.Id == contactId && c.MisNewsletterListId == id);
            if (contact != null)
            {
                _context.MisNewsletterContacts.Remove(contact);
                await _context.SaveChangesAsync();
                TempData["success"] = "Contact removed.";
            }

            return RedirectToPage(new { id });
        }
    }
}


