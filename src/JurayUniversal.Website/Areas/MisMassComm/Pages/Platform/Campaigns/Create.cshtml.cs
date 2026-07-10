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

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.Campaigns
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
        public string? AdditionalNumbers { get; set; }
        
        [BindProperty]
        public string ActionType { get; set; } = "SendNow"; // SendNow, Schedule, Draft

        public IList<MisPhoneBook> AvailablePhoneBooks { get; set; } = new List<MisPhoneBook>();
        public decimal CurrentBalance { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            AvailablePhoneBooks = await _context.MisPhoneBooks.Where(p => p.UserId == user.Id).ToListAsync();
            var wallet = await _context.MisWallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            CurrentBalance = wallet?.Balance ?? 0m;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            AvailablePhoneBooks = await _context.MisPhoneBooks.Where(p => p.UserId == user.Id).ToListAsync();
            var wallet = await _context.MisWallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            CurrentBalance = wallet?.Balance ?? 0m;

            if (string.IsNullOrEmpty(Campaign.Message) || string.IsNullOrEmpty(Campaign.SenderId))
            {
                ModelState.AddModelError("", "Message and Sender ID are required.");
                return Page();
            }

            // Collect all unique phone numbers
            var phoneNumbers = new HashSet<string>();

            if (SelectedPhoneBooks != null && SelectedPhoneBooks.Count > 0)
            {
                var contacts = await _context.MisContacts
                    .Where(c => SelectedPhoneBooks.Contains(c.MisPhoneBookId) && c.IsActive)
                    .Select(c => c.PhoneNumber)
                    .ToListAsync();
                    
                foreach (var c in contacts) if (!string.IsNullOrEmpty(c)) phoneNumbers.Add(c);
            }

            if (!string.IsNullOrWhiteSpace(AdditionalNumbers))
            {
                var addNums = AdditionalNumbers.Split(new[] { ',', '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var n in addNums) phoneNumbers.Add(n.Trim());
            }

            if (phoneNumbers.Count == 0 && ActionType != "Draft")
            {
                ModelState.AddModelError("", "Please select at least one Phone Book or enter additional numbers.");
                return Page();
            }

            // Cost Calculation (Simple approximation: 1 unit per 160 chars per number)
            int messageLength = Campaign.Message.Length;
            int pages = (int)Math.Ceiling(messageLength / 160.0);
            decimal costPerSms = 4.0m; // Example fixed cost per page. Should be pulled from settings.
            decimal totalCost = phoneNumbers.Count * pages * costPerSms;

            if (ActionType != "Draft" && CurrentBalance < totalCost)
            {
                ModelState.AddModelError("", $"Insufficient Wallet Balance. This campaign costs approx. {totalCost:N2}. Your balance is {CurrentBalance:N2}.");
                return Page();
            }

            // Save Campaign
            Campaign.UserId = user.Id;
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
                
                // Debit wallet upfront for scheduled campaigns
                wallet.Balance -= totalCost;
                _context.MisWalletTransactions.Add(new MisWalletTransaction { MisWalletId = wallet.Id, UserId = user.Id, Amount = totalCost, TransactionType = Domain.Enum.Enum.MisWalletTransactionType.Debit, Description = $"Reserved for Scheduled Campaign: {Campaign.Subject}" });
            }
            else // SendNow
            {
                Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Sending;
                
                // Debit wallet
                wallet.Balance -= totalCost;
                _context.MisWalletTransactions.Add(new MisWalletTransaction { MisWalletId = wallet.Id, UserId = user.Id, Amount = totalCost, TransactionType = Domain.Enum.Enum.MisWalletTransactionType.Debit, Description = $"Sent Campaign: {Campaign.Subject}" });
            }

            _context.MisSmsCampaigns.Add(Campaign);
            await _context.SaveChangesAsync();

            // Save Recipients
            var recipientsList = phoneNumbers.Select(p => new MisSmsRecipient
            {
                MisSmsCampaignId = Campaign.Id,
                PhoneNumber = p,
                DeliveryStatus = Domain.Enum.Enum.MisSmsDeliveryStatus.Pending
            }).ToList();

            _context.MisSmsRecipients.AddRange(recipientsList);
            await _context.SaveChangesAsync();

            // Execute immediately if SendNow
            if (ActionType == "SendNow")
            {
                string commaSeparatedContacts = string.Join(",", phoneNumbers);
                var sendResult = await _smsService.SendCampaignSmsAsync(commaSeparatedContacts, Campaign.Message, Campaign.SenderId);
                
                if (sendResult == SmsSendResult.Success)
                {
                    Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Completed;
                    foreach (var rec in recipientsList) rec.DeliveryStatus = Domain.Enum.Enum.MisSmsDeliveryStatus.Sent;
                    TempData["SuccessMessage"] = "Campaign sent successfully!";
                }
                else if (sendResult == SmsSendResult.InsufficientKudiBalance)
                {
                    Campaign.Status = Domain.Enum.Enum.MisSmsCampaignStatus.Pending;
                    // Pending implies it will be retried by the background scheduler
                    TempData["SuccessMessage"] = "Campaign submitted but placed in Pending due to system gateway delays. It will be sent automatically once resolved.";
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
                TempData["SuccessMessage"] = "Campaign scheduled successfully! It will be sent automatically.";
            }
            else
            {
                TempData["SuccessMessage"] = "Campaign saved as Draft.";
            }

            return RedirectToPage("./Index");
        }
    }
}
