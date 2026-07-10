using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard
{
    [Authorize]
    public class VoteModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<JurayUniversal.Domain.Models.Profile> _userManager;

        public VoteModel(DashboardDbContext context, UserManager<JurayUniversal.Domain.Models.Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Evoting Election { get; set; }
        public bool HasVoted { get; set; }

        [BindProperty]
        public Dictionary<long, long> Selections { get; set; } = new Dictionary<long, long>();

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Election = await _context.Evotings
                .Include(x => x.VoteCategories)
                    .ThenInclude(c => c.VoteCondidates)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Election == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            HasVoted = await _context.UserVotes
                .Include(x => x.VoteCategory)
                .AnyAsync(x => x.VoteCategory.EvotingId == id && x.VoterUserId == userId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            Election = await _context.Evotings
                .Include(x => x.VoteCategories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Election == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            bool hasVoted = await _context.UserVotes
                .Include(x => x.VoteCategory)
                .AnyAsync(x => x.VoteCategory.EvotingId == id && x.VoterUserId == userId);

            if (hasVoted)
            {
                TempData["error"] = "You have already voted in this election. Double voting is strictly prohibited.";
                return RedirectToPage("./Index");
            }

            if (!Election.StartVoting || Election.VotingStatus == Domain.Enum.Enum.VotingStatus.Completed || Election.EndDate < DateTime.UtcNow)
            {
                TempData["error"] = "Voting is currently closed for this election.";
                return RedirectToPage("./Index");
            }

            if (Selections == null || Selections.Count == 0)
            {
                TempData["error"] = "Please select at least one candidate before submitting your ballot.";
                return RedirectToPage(new { id = id });
            }

            var encryptCode = Guid.NewGuid().ToString("N").ToUpper();

            foreach (var selection in Selections)
            {
                var categoryId = selection.Key;
                var candidateId = selection.Value;

                var userVote = new UserVote
                {
                    VoteCategoryId = categoryId,
                    VoteCondidateId = candidateId,
                    VoterUserId = userId,
                    EncryptCode = encryptCode,
                    UserVoteStatus = Domain.Enum.Enum.UserVoteStatus.Verified
                };

                _context.UserVotes.Add(userVote);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = $"Your vote has been securely cast! Your ballot receipt code is: {encryptCode}";
            return RedirectToPage("./Index");
        }
    }
}
