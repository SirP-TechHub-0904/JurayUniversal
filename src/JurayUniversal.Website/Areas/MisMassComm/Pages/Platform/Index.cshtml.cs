using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisWallet UserWallet { get; set; } = new MisWallet();
        public int TotalSmsSent { get; set; }
        public int ActiveContacts { get; set; }
        public int TotalCampaigns { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var wallet = await _context.MisWallets.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (wallet == null)
            {
                wallet = new MisWallet { UserId = user.Id, Balance = 0 };
                _context.MisWallets.Add(wallet);
                await _context.SaveChangesAsync();
            }
            UserWallet = wallet;

            TotalSmsSent = await _context.MisSmsRecipients
                .Include(r => r.MisSmsCampaign)
                .Where(r => r.MisSmsCampaign.UserId == user.Id && r.DeliveryStatus == Domain.Enum.Enum.MisSmsDeliveryStatus.Sent)
                .CountAsync();

            ActiveContacts = await _context.MisContacts
                .Include(c => c.MisPhoneBook)
                .Where(c => c.MisPhoneBook.UserId == user.Id && c.IsActive)
                .CountAsync();

            TotalCampaigns = await _context.MisSmsCampaigns
                .Where(c => c.UserId == user.Id)
                .CountAsync();

            return Page();
        }
    }
}
