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
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisNewsletterCampaign Campaign { get; set; }
        public IList<MisNewsletterEmailLog> EmailLogs { get; set; } = new List<MisNewsletterEmailLog>();
        
        public int TotalSent { get; set; }
        public int TotalFailed { get; set; }
        public int TotalPending { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var userId = _userManager.GetUserId(User);
            Campaign = await _context.MisNewsletterCampaigns
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (Campaign == null)
            {
                return NotFound();
            }

            EmailLogs = await _context.MisNewsletterEmailLogs
                .Where(e => e.CampaignId == id)
                .OrderByDescending(e => e.SentDatetime)
                .ToListAsync();

            TotalSent = EmailLogs.Count(e => e.Status == JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Sent);
            TotalFailed = EmailLogs.Count(e => e.Status == JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Failed);
            TotalPending = EmailLogs.Count(e => e.Status == JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Pending);

            return Page();
        }
    }
}
