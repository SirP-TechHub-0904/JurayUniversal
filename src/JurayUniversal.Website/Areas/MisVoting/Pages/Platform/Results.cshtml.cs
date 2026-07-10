using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Platform
{
    [AllowAnonymous]
    public class ResultsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ResultsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisElection Election { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Election = await _context.MisElections
                .Include(x => x.MisVoteCategories)
                    .ThenInclude(c => c.MisCandidates)
                        .ThenInclude(c => c.MisUserVotes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Election == null)
            {
                return NotFound();
            }

            // Only allow viewing results if ShowResults is true AND Election is Completed
            var isCompleted = Election.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Completed;
            if (!Election.ShowResults || !isCompleted)
            {
                TempData["error"] = "Results for this election are not available yet.";
                return RedirectToPage("/Platform/Index", new { area = "MisVoting" });
            }

            return Page();
        }
    }
}
