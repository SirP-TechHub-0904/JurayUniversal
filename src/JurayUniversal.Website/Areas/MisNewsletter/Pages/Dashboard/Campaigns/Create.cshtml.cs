using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Campaigns
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
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

        [BindProperty]
        public string ManualEmails { get; set; }

        public SelectList Lists { get; set; }

        public bool IsEmailConfigured { get; set; } = true;

        public async Task<IActionResult> OnGetAsync(long? id, long? duplicateId)
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings == null || !settings.EnableZeptoMail || string.IsNullOrWhiteSpace(settings.ZeptoMailApiKey))
            {
                IsEmailConfigured = false;
            }

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
            else if (duplicateId.HasValue)
            {
                var oldCampaign = await _context.MisNewsletterCampaigns
                    .Include(c => c.Recipients)
                    .FirstOrDefaultAsync(c => c.Id == duplicateId.Value && c.UserId == userId);
                
                if (oldCampaign == null) return NotFound();

                Campaign = new MisNewsletterCampaign
                {
                    Subject = oldCampaign.Subject,
                    HtmlBody = oldCampaign.HtmlBody
                };

                if (oldCampaign.Recipients != null && oldCampaign.Recipients.Any())
                {
                    var emailStrings = oldCampaign.Recipients.Select(r => 
                        string.IsNullOrWhiteSpace(r.Name) ? r.EmailAddress : $"{r.Name}, {r.EmailAddress}"
                    );
                    ManualEmails = string.Join("\n", emailStrings);
                }
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

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Please fill in all required fields.";
                var userLists = await _context.MisNewsletterLists
                    .Where(l => l.UserId == userId)
                    .Select(l => new { l.Id, l.Name })
                    .ToListAsync();
                Lists = new SelectList(userLists, "Id", "Name");
                return Page();
            }

            List<MisNewsletterContact> finalContacts = new List<MisNewsletterContact>();

            // Parse Manual Emails First
            if (!string.IsNullOrWhiteSpace(ManualEmails))
            {
                var lines = ManualEmails.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var cleanLine = line.Trim();
                    if (string.IsNullOrWhiteSpace(cleanLine)) continue;

                    string name = "Valued Member";
                    string email = cleanLine;

                    // Basic check for Name, email@domain.com format
                    if (cleanLine.Contains(" ") || cleanLine.Contains("<")) 
                    {
                        var parts = cleanLine.Split(new[] { ' ', '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                        var foundEmail = parts.FirstOrDefault(p => p.Contains("@"));
                        if (foundEmail != null)
                        {
                            email = foundEmail.Trim();
                            name = cleanLine.Replace(email, "").Replace("<", "").Replace(">", "").Trim(',').Trim();
                            if(string.IsNullOrWhiteSpace(name)) name = "Valued Member";
                        }
                    }

                    if(email.Contains("@"))
                    {
                        finalContacts.Add(new MisNewsletterContact { Name = name, Email = email });
                    }
                }
            }
            else if (SelectedListId > 0)
            {
                var list = await _context.MisNewsletterLists
                    .Include(l => l.Contacts)
                    .FirstOrDefaultAsync(l => l.Id == SelectedListId && l.UserId == userId);

                if (list != null && list.Contacts != null)
                {
                    finalContacts = list.Contacts.Where(c => c.IsActive).ToList();
                }
            }

            if (!finalContacts.Any())
            {
                TempData["error"] = "No recipients found. Please select a valid mailing list or enter manual emails.";
                return RedirectToPage();
            }

            // Remove duplicates by email
            finalContacts = finalContacts.GroupBy(c => c.Email).Select(g => g.First()).ToList();

            int requiredQuota = finalContacts.Count;

            if (command == "SendNow" || command == "Schedule")
            {
                var settings = await _context.MisSettings.FirstOrDefaultAsync();
                
                if (settings == null || !settings.EnableZeptoMail || string.IsNullOrWhiteSpace(settings.ZeptoMailApiKey))
                {
                    TempData["error"] = "Cannot send or schedule campaign. Email configuration (ZeptoMail) is missing in MIS Settings.";
                    IsEmailConfigured = false;
                    var userLists = await _context.MisNewsletterLists
                        .Where(l => l.UserId == userId)
                        .Select(l => new { l.Id, l.Name })
                        .ToListAsync();
                    Lists = new SelectList(userLists, "Id", "Name");
                    return Page();
                }

                // Verify Quota
                bool isAdmin = User.IsInRole("mSuperAdmin"); 
                
                var userWallet = await _context.MisNewsletterWallets.FirstOrDefaultAsync(w => w.UserId == userId);

                bool hasEnoughQuota = false;
                bool usingGlobal = false;

                if (userWallet != null && userWallet.EmailQuotaBalance >= requiredQuota && (!userWallet.ExpiryDate.HasValue || userWallet.ExpiryDate.Value >= DateTime.UtcNow))
                {
                    hasEnoughQuota = true;
                }
                else if (User.IsInRole("mSuperAdmin") || User.IsInRole("mAdmin"))
                {
                    if (settings != null && settings.GlobalNewsletterQuota >= requiredQuota && (!settings.GlobalNewsletterQuotaExpiry.HasValue || settings.GlobalNewsletterQuotaExpiry.Value >= DateTime.UtcNow))
                    {
                        hasEnoughQuota = true;
                        usingGlobal = true;
                    }
                }

                if (!hasEnoughQuota)
                {
                    TempData["error"] = $"Insufficient Quota or Expired Wallet. You need {requiredQuota} emails to send to this list.";
                    return RedirectToPage();
                }

                // Deduct Quota
                if (usingGlobal)
                {
                    settings.GlobalNewsletterQuota -= requiredQuota;
                }
                else
                {
                    userWallet.EmailQuotaBalance -= requiredQuota;
                    var wTx = new MisNewsletterWalletTransaction
                    {
                        MisNewsletterWalletId = userWallet.Id,
                        Amount = requiredQuota,
                        TransactionType = JurayUniversal.Domain.Enum.Enum.MisWalletTransactionType.Debit,
                        Description = $"Sent Campaign: {Campaign.Subject}",
                        Date = DateTime.UtcNow
                    };
                    _context.MisNewsletterWalletTransactions.Add(wTx);
                }
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

                    await _context.SaveChangesAsync();
                    Campaign = existing;
                }
            }
            else
            {
                // Create new
                Campaign.UserId = userId;
                Campaign.TotalRecipients = finalContacts.Count;
                
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
                foreach (var contact in finalContacts)
                {
                    _context.MisNewsletterRecipients.Add(new MisNewsletterRecipient
                    {
                        MisNewsletterCampaignId = Campaign.Id,
                        Name = contact.Name,
                        EmailAddress = contact.Email,
                        UserId = userId
                    });
                }
                await _context.SaveChangesAsync();
            }

            TempData["success"] = $"Campaign saved successfully as {Campaign.Status}.";
            return RedirectToPage("./Index");
        }
    }
}
