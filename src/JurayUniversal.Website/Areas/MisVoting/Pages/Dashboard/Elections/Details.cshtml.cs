using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.Elections
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisElection Election { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Election = await _context.MisElections
                .Include(x => x.MisVoteCategories)
                    .ThenInclude(c => c.MisCandidates)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Election == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostPublishAsync(long id)
        {
            var election = await _context.MisElections
                .Include(x => x.MisVoteCategories)
                    .ThenInclude(c => c.MisCandidates)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (election == null)
            {
                return NotFound();
            }

            bool hasCandidates = election.MisVoteCategories != null && 
                                 election.MisVoteCategories.Any(c => c.MisCandidates != null && c.MisCandidates.Any());

            if (!hasCandidates)
            {
                TempData["error"] = "Cannot publish election: You must add at least one candidate before activating the election.";
                return RedirectToPage(new { id = id });
            }

            election.VotingStatus = Domain.Enum.Enum.MisVotingStatus.Active;
            await _context.SaveChangesAsync();

            TempData["success"] = "Election has been successfully published and is now Active.";
            return RedirectToPage(new { id = id });
        }
    }
}
