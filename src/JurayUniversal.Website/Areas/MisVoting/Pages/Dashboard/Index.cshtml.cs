using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard
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

        public IList<MisElection> AvailableElections { get; set; } = new List<MisElection>();
        public IList<MisElection> PastElections { get; set; } = new List<MisElection>();

        public async Task OnGetAsync()
        {
            var now = System.DateTime.UtcNow;
            var userId = _userManager.GetUserId(User);

            var allElections = await _context.MisElections
                .Include(x => x.MisElectionVoters)
                .AsNoTracking()
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();

            // Available: Active status AND (Open OR (Closed and Accredited))
            AvailableElections = allElections
                .Where(x => x.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Active && x.EndDate > now)
                .Where(x => x.ElectionType == Domain.Enum.Enum.MisElectionType.Open || 
                           (x.ElectionType == Domain.Enum.Enum.MisElectionType.Closed && x.MisElectionVoters.Any(v => v.UserId == userId && v.IsAccredited)))
                .ToList();

            // Past: Completed status OR past EndDate
            PastElections = allElections
                .Where(x => x.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Completed || x.EndDate <= now)
                .ToList();
        }
    }
}
