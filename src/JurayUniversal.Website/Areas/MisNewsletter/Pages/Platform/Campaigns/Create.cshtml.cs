using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Campaigns
{
    [Authorize]
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
        public MisNewsletterCampaign Campaign { get; set; }

        [BindProperty]
        public long SelectedListId { get; set; }

        public SelectList Lists { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var userId = _userManager.GetUserId(User);

            var userLists = await _context.MisNewsletterLists
                .Where(l => l.UserId == userId)
                .Select(l => new { l.Id, l.Name })
                .ToListAsync();

            Lists = new SelectList(userLists, "Id", "Name");

            if (id.HasValue)
            {
                Campaign = await _context.MisNewsletterCampaigns.FirstOrDefaultAsync(c => c.Id == id.Value && c.UserId == userId);
                if (Campaign == null) return NotFound();
            }
            else
            {
                Campaign = new MisNewsletterCampaign();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string command)
        {
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid || SelectedListId == 0)
            {
                TempData["error"] = "Please fill in all required fields and select a mailing list.";
                var userLists = await _context.MisNewsletterLists
                    .Where(l => l.UserId == userId)
                    .Select(l => new { l.Id, l.Name })
                    .ToListAsync();
                Lists = new SelectList(userLists, "Id", "Name");
                return Page();
            }

            var list = await _context.MisNewsletterLists
                .Include(l => l.Contacts)
                .FirstOrDefaultAsync(l => l.Id == SelectedListId && l.UserId == userId);

            if (list == null)
            {
                TempData["error"] = "Selected mailing list not found.";
                return RedirectToPage();
            }

            if (list.Contacts == null || !list.Contacts.Any())
            {
                TempData["error"] = "Selected mailing list has no contacts. Please add contacts before sending a campaign.";
                return RedirectToPage();
            }

            if (Campaign.Id > 0)
            {
                // Update existing
                var existing = await _context.MisNewsletterCampaigns.FirstOrDefaultAsync(c => c.Id == Campaign.Id && c.UserId == userId);
                if (existing != null)
                {
                    existing.Subject = Campaign.Subject;
                    existing.HtmlBody = Campaign.HtmlBody;
                    existing.ScheduledDate = Campaign.ScheduledDate;
                    
                    if (command == "SendNow")
                    {
                        existing.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Pending;
                    }
                    else if (command == "Schedule")
                    {
                        existing.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Scheduled;
                    }
                    else
                    {
                        existing.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Draft;
                    }

                    // For now, we recreate recipients. In a real scenario, you'd only do this when actually sending/scheduling.
                    await _context.SaveChangesAsync();
                    Campaign = existing;
                }
            }
            else
            {
                // Create new
                Campaign.UserId = userId;
                Campaign.TotalRecipients = list.Contacts.Count;
                
                if (command == "SendNow")
                {
                    Campaign.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Pending;
                }
                else if (command == "Schedule")
                {
                    Campaign.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Scheduled;
                }
                else
                {
                    Campaign.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Draft;
                }

                _context.MisNewsletterCampaigns.Add(Campaign);
                await _context.SaveChangesAsync(); // get ID

                // Add recipients
                foreach (var contact in list.Contacts)
                {
                    _context.MisNewsletterRecipients.Add(new MisNewsletterRecipient
                    {
                        MisNewsletterCampaignId = Campaign.Id,
                        Name = contact.Name,
                        EmailAddress = contact.Email
                    });
                }
                await _context.SaveChangesAsync();
            }

            TempData["success"] = $"Campaign saved successfully as {Campaign.Status}.";
            return RedirectToPage("./Index");
        }
    }
}

