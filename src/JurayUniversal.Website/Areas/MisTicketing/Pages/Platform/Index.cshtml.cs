using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Platform
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

        public IList<MisEventTicket> MyTickets { get; set; }
        public IList<MisEvent> UpcomingEvents { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            MyTickets = await _context.MisEventTickets
                .Include(t => t.MisTicketType)
                    .ThenInclude(tt => tt.MisEvent)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var today = System.DateTime.UtcNow.Date;

            UpcomingEvents = await _context.MisEvents
                .Where(e => e.IsActive && (e.EndDate != null ? e.EndDate >= today : e.StartDate >= today))
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }
    }
}
