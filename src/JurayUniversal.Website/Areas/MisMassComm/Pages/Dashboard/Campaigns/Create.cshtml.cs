using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.Campaigns
{
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly ISmsSendService _smsService;

        public CreateModel(DashboardDbContext context, UserManager<Profile> userManager, ISmsSendService smsService)
        {
            _context = context;
            _userManager = userManager;
            _smsService = smsService;
        }

        [BindProperty]
        public MisSmsCampaign Campaign { get; set; } = new MisSmsCampaign();

        [BindProperty]
        public List<long> SelectedPhoneBooks { get; set; } = new List<long>();

        [BindProperty]
        public List<string> SelectedUsers { get; set; } = new List<string>();

        [BindProperty]
        public string? AdditionalNumbers { get; set; }
        
        [BindProperty]
        public string ActionType { get; set; } = "SendNow"; 

        public IList<MisPhoneBook> GlobalPhoneBooks { get; set; } = new List<MisPhoneBook>();
        public IList<Profile> SystemUsers { get; set; } = new List<Profile>();
        public decimal CurrentCompanyBalance { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Global phone books belong to no specific user
            GlobalPhoneBooks = await _context.MisPhoneBooks.Where(p => p.UserId == null).ToListAsync();
            SystemUsers = await _userManager.Users.Where(u => !string.IsNullOrEmpty(u.PhoneNumber)).ToListAsync();
            
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            CurrentCompanyBalance = settings?.SmsCreditBalance ?? 0m;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            GlobalPhoneBooks = await _context.MisPhoneBooks.Where(p => p.UserId == null).ToListAsync();
            SystemUsers = await _userManager.Users.Where(u => !string.IsNullOrEmpty(u.PhoneNumber)).ToListAsync();
            
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                ModelState.AddModelError("", "System configuration missing.");
                return Page();
            }
            CurrentCompanyBalance = settings.SmsCreditBalance;

            var adminUser = await _userManager.GetUserAsync(User);
            if (adminUser == null) return NotFound();

            if (string.IsNullOrEmpty(Campaign.Message) || string.IsNullOrEmpty(Campaign.SenderId))
            {
                ModelState.AddModelError("", "Message and Sender ID are required.");
                return Page();
            }

            var phoneNumbers = new HashSet<string>();

            // 1. Phone Books
            if (SelectedPhoneBooks != null && SelectedPhoneBooks.Count > 0)
            {
                var contacts = await _context.MisContacts
                    .Where(c => SelectedPhoneBooks.Contains(c.MisPhoneBookId) && c.IsActive)
                    .Select(c => c.PhoneNumber)
                    .ToListAsync();
                foreach (var c in contacts) if (!string.IsNullOrEmpty(c)) phoneNumbers.Add(c);
            }

            // 2. Specific System Users
            if (SelectedUsers != null && SelectedUsers.Count > 0)
            {
                var users = await _userManager.Users.Where(u => SelectedUsers.Contains(u.Id)).ToListAsync();
                foreach (var u in users) if (!string.IsNullOrEmpty(u.PhoneNumber)) phoneNumbers.Add(u.PhoneNumber);
            }

            // 3. Additional manual numbers
            if (!string.IsNullOrWhiteSpace(AdditionalNumbers))
            {
                var addNums = AdditionalNumbers.Split(new[] { ',', '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var n in addNums) phoneNumbers.Add(n.Trim());
            }

            if (phoneNumbers.Count == 0 && ActionType != "Draft")
            {
                ModelState.AddModelError("", "Please select at least one Phone Book, System User, or enter additional numbers.");
                return Page();
            }

            int messageLength = Campaign.Message.Length;
            int pages = (int)Math.Ceiling(messageLength / 160.0);
            decimal costPerSms = 4.0m; // fixed cost
            decimal totalCost = phoneNumbers.Count * pages * costPerSms;

            if (ActionType != "Draft" && CurrentCompanyBalance < totalCost)
            {
                ModelState.AddModelError("", $"Insufficient Company Wallet Balance. Cost is {totalCost:N2}. Balance is {CurrentCompanyBalance:N2}.");
                return Page();
            }

            Campaign.UserId = adminUser.Id;
            Campaign.CreatedAt = DateTime.UtcNow;
            Campaign.TotalRecipients = phoneNumbers.Count;
            Campaign.CostIncurred = ActionType == "Draft" ? 0 : totalCost;

            if (ActionType == "Draft")
            {
                Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Draft;
            }
            else if (ActionType == "Schedule")
            {
                if (!Campaign.ScheduledDate.HasValue || Campaign.ScheduledDate.Value <= DateTime.UtcNow)
                {
                    ModelState.AddModelError("", "Scheduled Date must be in the future.");
                    return Page();
                }
                Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Scheduled;
                settings.SmsCreditBalance -= totalCost;
            }
            else
            {
                Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Sending;
                settings.SmsCreditBalance -= totalCost;
            }

            _context.MisSmsCampaigns.Add(Campaign);
            await _context.SaveChangesAsync();

            var recipientsList = phoneNumbers.Select(p => new MisSmsRecipient
            {
                MisSmsCampaignId = Campaign.Id,
                PhoneNumber = p,
                DeliveryStatus = Domain.Enum.Enum.MisSmsDeliveryStatus.Pending
            }).ToList();

            _context.MisSmsRecipients.AddRange(recipientsList);
            await _context.SaveChangesAsync();

            if (ActionType == "SendNow")
            {
                string commaSeparatedContacts = string.Join(",", phoneNumbers);
                var sendResult = await _smsService.SendCampaignSmsAsync(commaSeparatedContacts, Campaign.Message, Campaign.SenderId);
                
                if (sendResult == SmsSendResult.Success)
                {
                    Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Completed;
                    foreach (var rec in recipientsList) rec.DeliveryStatus = Domain.Enum.Enum.MisSmsDeliveryStatus.Sent;
                    TempData["SuccessMessage"] = "Global Campaign sent successfully!";
                }
                else if (sendResult == SmsSendResult.InsufficientKudiBalance)
                {
                    Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Pending;
                    TempData["SuccessMessage"] = "Campaign submitted but placed in Pending due to system error. It will auto-retry once resolved.";
                }
                else
                {
                    Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Failed;
                    foreach (var rec in recipientsList) rec.DeliveryStatus = Domain.Enum.Enum.MisSmsDeliveryStatus.Failed;
                    TempData["SuccessMessage"] = "Campaign was submitted but the SMS gateway reported a failure.";
                }
                await _context.SaveChangesAsync();
            }
            else if (ActionType == "Schedule")
            {
                TempData["SuccessMessage"] = "Global Campaign scheduled successfully! It will be sent automatically.";
            }
            else
            {
                TempData["SuccessMessage"] = "Global Campaign saved as Draft.";
            }

            return RedirectToPage("./Index");
        }
    }
}
