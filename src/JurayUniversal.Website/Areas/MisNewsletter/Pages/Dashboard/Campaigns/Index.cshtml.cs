using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Campaigns
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<MisNewsletterCampaign> Campaigns { get; set; } = new List<MisNewsletterCampaign>();

        public async Task<IActionResult> OnGetAsync(string status = null)
        {
            var userId = _userManager.GetUserId(User);
            var query = _context.MisNewsletterCampaigns.Where(c => c.UserId == userId);

            if (!string.IsNullOrEmpty(status) && System.Enum.TryParse(typeof(JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus), status, true, out var parsedStatus))
            {
                query = query.Where(c => c.Status == (JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus)parsedStatus);
            }

            Campaigns = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostResendFailedAsync(long id)
        {
            var userId = _userManager.GetUserId(User);
            var campaign = await _context.MisNewsletterCampaigns
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (campaign == null) return NotFound();

            var failedLogs = await _context.MisNewsletterEmailLogs
                .Where(l => l.CampaignId == id && l.Status == JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Failed)
                .ToListAsync();

            if (failedLogs.Any())
            {
                foreach (var log in failedLogs)
                {
                    log.Status = JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Pending;
                    log.RetryCount = 0;
                }
                
                // Also update campaign status if it was completely failed
                if (campaign.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Failed)
                {
                    campaign.Status = JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Sending;
                }

                await _context.SaveChangesAsync();
                TempData["success"] = $"{failedLogs.Count} failed email(s) have been queued for resending.";
            }
            else
            {
                TempData["error"] = "No failed emails found for this campaign.";
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var userId = _userManager.GetUserId(User);
            var campaign = await _context.MisNewsletterCampaigns.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            
            if (campaign == null) return NotFound();

            if (campaign.Status == JurayUniversal.Domain.Enum.Enum.MisNewsletterCampaignStatus.Sending)
            {
                TempData["error"] = "Cannot delete a campaign that is currently sending.";
                return RedirectToPage();
            }

            _context.MisNewsletterCampaigns.Remove(campaign);
            await _context.SaveChangesAsync();

            TempData["success"] = "Campaign deleted successfully.";
            return RedirectToPage();
        }
    }
}
