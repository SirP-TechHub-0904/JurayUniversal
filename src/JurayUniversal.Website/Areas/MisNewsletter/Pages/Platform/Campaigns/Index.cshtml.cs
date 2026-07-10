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

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Campaigns
{
    [Authorize]
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

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Campaigns = await _context.MisNewsletterCampaigns
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Page();
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

