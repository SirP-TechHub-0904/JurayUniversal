using JurayUniversal.Domain.Models;
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

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Platform
{
    [AllowAnonymous]
    public class VoteModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public VoteModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisElection Election { get; set; }
        public bool HasVoted { get; set; }
        public bool IsAccredited { get; set; }

        [BindProperty]
        public Dictionary<long, long> Selections { get; set; } = new Dictionary<long, long>();

        public async Task<IActionResult> OnGetAsync(string shortcode)
        {
            Election = await _context.MisElections
                .Include(x => x.MisVoteCategories)
                    .ThenInclude(c => c.MisCandidates)
                .FirstOrDefaultAsync(x => x.ShortCode == shortcode || x.Id.ToString() == shortcode);

            if (Election == null)
            {
                return NotFound();
            }

            bool isPublic = Election.ElectionType == Domain.Enum.Enum.MisElectionType.Public;

            if (!isPublic && !User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            if (isPublic)
            {
                HasVoted = Request.Cookies.ContainsKey($"Juray_Voted_Election_{Election.Id}");
                IsAccredited = true;
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                var voterRecord = await _context.MisElectionVoters
                    .FirstOrDefaultAsync(x => x.MisElectionId == Election.Id && x.UserId == userId);

                HasVoted = voterRecord?.HasVoted ?? false;
                IsAccredited = (Election.ElectionType == Domain.Enum.Enum.MisElectionType.Open) || (voterRecord?.IsAccredited ?? false);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string shortcode)
        {
            Election = await _context.MisElections
                .Include(x => x.MisVoteCategories)
                .FirstOrDefaultAsync(x => x.ShortCode == shortcode || x.Id.ToString() == shortcode);

            if (Election == null)
            {
                return NotFound();
            }

            bool isPublic = Election.ElectionType == Domain.Enum.Enum.MisElectionType.Public;

            if (!isPublic && !User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            bool hasVoted = false;
            bool isAccredited = false;
            string userId = null;
            MisElectionVoter voterRecord = null;

            if (isPublic)
            {
                hasVoted = Request.Cookies.ContainsKey($"Juray_Voted_Election_{Election.Id}");
                isAccredited = true;
            }
            else
            {
                userId = _userManager.GetUserId(User);
                voterRecord = await _context.MisElectionVoters
                    .FirstOrDefaultAsync(x => x.MisElectionId == Election.Id && x.UserId == userId);

                hasVoted = voterRecord?.HasVoted ?? false;
                isAccredited = (Election.ElectionType == Domain.Enum.Enum.MisElectionType.Open) || (voterRecord?.IsAccredited ?? false);
            }

            if (!isAccredited)
            {
                TempData["error"] = "You are not accredited to vote in this Closed Election.";
                return RedirectToPage("/Platform/Index", new { area = "MisVoting" });
            }

            if (hasVoted)
            {
                TempData["error"] = "You have already voted in this election. Double voting is strictly prohibited.";
                return RedirectToPage("/Platform/Index", new { area = "MisVoting" });
            }

            if (Election.VotingStatus != Domain.Enum.Enum.MisVotingStatus.Active || Election.EndDate < DateTime.UtcNow)
            {
                TempData["error"] = "Voting is currently closed for this election.";
                return RedirectToPage("/Platform/Index", new { area = "MisVoting" });
            }

            if (Selections == null || Selections.Count == 0)
            {
                TempData["error"] = "Please select at least one candidate before submitting your ballot.";
                return RedirectToPage(new { shortcode = shortcode });
            }

            if (Election.RequireAllCategories)
            {
                var totalCategories = Election.MisVoteCategories.Count;
                if (Selections.Count < totalCategories)
                {
                    TempData["error"] = "Strict Rule Enforced: You must cast a vote in every category before submitting your ballot.";
                    return RedirectToPage(new { shortcode = shortcode });
                }
            }

            var encryptCode = Guid.NewGuid().ToString("N").ToUpper();

            foreach (var selection in Selections)
            {
                var categoryId = selection.Key;
                var candidateId = selection.Value;

                var userVote = new MisUserVote
                {
                    MisVoteCategoryId = categoryId,
                    MisCandidateId = candidateId,
                    EncryptCode = encryptCode,
                    DateCast = DateTime.UtcNow
                };

                _context.MisUserVotes.Add(userVote);
            }

            if (isPublic)
            {
                Response.Cookies.Append($"Juray_Voted_Election_{Election.Id}", "true", new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            }
            else
            {
                if (voterRecord == null)
                {
                    voterRecord = new MisElectionVoter
                    {
                        MisElectionId = Election.Id,
                        UserId = userId,
                        IsAccredited = true,
                        HasVoted = true
                    };
                    _context.MisElectionVoters.Add(voterRecord);
                }
                else
                {
                    voterRecord.HasVoted = true;
                    _context.MisElectionVoters.Update(voterRecord);
                }
            }

            await _context.SaveChangesAsync();

            TempData["success"] = $"Your vote has been securely and anonymously cast! Your ballot receipt code is: {encryptCode}";
            return RedirectToPage("/Platform/Index", new { area = "MisVoting" });
        }
    }
}
