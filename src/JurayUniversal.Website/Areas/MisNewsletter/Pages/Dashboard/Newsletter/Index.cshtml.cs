using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Newsletter
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,SubAdmin")]
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<NewsLetter> NewsLetters { get; set; }
        public List<string> ExportedEmails { get; set; } = new List<string>();

        [BindProperty]
        public List<long> SelectedIds { get; set; } = new List<long>();

        public async Task OnGetAsync()
        {
            NewsLetters = await _context.NewsLetter.OrderByDescending(x => x.Date).ToListAsync();
            
            bool isNewsletterFeatureActive = await _context.MisFeatures
                .AnyAsync(x => x.ModuleFolder == "MisNewsletter" && x.Status != JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Disabled);
            
            ViewData["IsNewsletterEnabled"] = isNewsletterFeatureActive;

            var userId = _userManager.GetUserId(User);
            var nlList = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Name == "Website Newsletter" && l.UserId == userId);
            
            if (nlList != null)
            {
                ExportedEmails = await _context.MisNewsletterContacts
                    .Where(c => c.MisNewsletterListId == nlList.Id)
                    .Select(c => c.Email)
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            if (SelectedIds == null || !SelectedIds.Any())
            {
                TempData["error"] = "No items selected for export.";
                return RedirectToPage();
            }

            var userId = _userManager.GetUserId(User);
            var nlList = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Name == "Website Newsletter" && l.UserId == userId);
            
            if (nlList == null)
            {
                nlList = new MisNewsletterList { Name = "Website Newsletter", UserId = userId, CreatedAt = DateTime.UtcNow };
                _context.MisNewsletterLists.Add(nlList);
                await _context.SaveChangesAsync();
            }

            var selectedSubscribers = await _context.NewsLetter.Where(r => SelectedIds.Contains(r.Id)).ToListAsync();
            int exportedCount = 0;

            foreach (var sub in selectedSubscribers)
            {
                if (!string.IsNullOrEmpty(sub.Email))
                {
                    bool exists = await _context.MisNewsletterContacts.AnyAsync(c => c.MisNewsletterListId == nlList.Id && c.Email == sub.Email);
                    if (!exists)
                    {
                        _context.MisNewsletterContacts.Add(new MisNewsletterContact
                        {
                            MisNewsletterListId = nlList.Id,
                            Email = sub.Email,
                            Name = sub.Email, // Subscriptions don't have names by default, use email
                            DateAdded = DateTime.UtcNow
                        });
                        exportedCount++;
                    }
                }
            }

            await _context.SaveChangesAsync();
            TempData["success"] = $"Successfully exported {exportedCount} subscribers.";

            return RedirectToPage();
        }
    }
}
