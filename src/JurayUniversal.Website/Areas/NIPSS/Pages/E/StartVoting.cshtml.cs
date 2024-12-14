using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.E
{
    [Authorize]
    public class StartVotingModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;

        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        public StartVotingModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public long VotingId { get; set; }
        public Evoting Evoting { get; set; }

        [BindProperty]
        public VoteCategory VoteCategory { get; set; }

        public List<VoteCondidate> VoteCondidates { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Evoting = await _context.Evotings
                .Include(v => v.Course)
                .ThenInclude(v => v.CourseCategory)
                .Include(v => v.VoteCategories)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (Evoting == null)
            {
                return NotFound();
            }

            //
            VoteCondidates = await _context.VoteCondidates
                .Include(x => x.UserVote)
                .Include(x => x.VoteCategory)
                .Where(x => x.VoteCategory.EvotingId == Evoting.Id).ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            var getuser = await _context.UserVotes.FirstOrDefaultAsync(x => x.VoteCategoryId == VoteCondidates.FirstOrDefault().VoteCategoryId && x.VoterUserId == user.Id);
            if (getuser != null)
            {
                TempData["Already"] = "ALREADY VOTED. CLICK THE BUTTON BELLOW TO VIEW RESULTS REALTIME";
                TempData["completevotetoken"] = "VotingCompleted";
                return RedirectToPage("./VotingComplete", new { id = Evoting.Id }); // Redirect back to the same page after submission
            }


                return Page();
        }

         public async Task<IActionResult> OnPostAsync(ResultViewModel model)
        {
 
            Dictionary<long, long> selectedCandidateIds = model.SelectedCandidateIds;
           var SelectedVotes = new List<SelectedVoteDto>();
            // Now you can process the selected candidate IDs as needed
            foreach (var kvp in selectedCandidateIds)
            {
                long voteCategoryId = kvp.Key;
                long selectedCandidateId = kvp.Value;

                //
                SelectedVoteDto addvote = new SelectedVoteDto();
                var category = await _context.VoteCategories.FirstOrDefaultAsync(x=>x.Id == voteCategoryId);
                if (category != null)
                {
                    addvote.CategoryId = category.Id;
                    addvote.Category = category.Title;
                }
                var candidate = await _context.VoteCondidates
                    .Include(x=>x.VoteCategory)
                    .FirstOrDefaultAsync(x=>x.Id == selectedCandidateId);
                if (candidate != null)
                {
                    addvote.CandidateId = candidate.Id;
                    addvote.CandidateUrl = candidate.CandidateImageUrl;
                }

                SelectedVotes.Add(addvote);
                // Process each selected candidate ID, you may want to associate it with its corresponding vote category ID
            }

            // Serialize SelectedVotes to JSON and store it in TempData
            var json = JsonConvert.SerializeObject(SelectedVotes);
            

            TempData["SelectedVotes"] = json;

            return RedirectToPage("./PreviewVote", new { id = VotingId }); // Redirect back to the same page after submission
        }

     }

    public class ResultViewModel
    {
        public long VotingId { get; set; }
        public Dictionary<long, long> SelectedCandidateIds { get; set; }
    }
    public class SelectedVoteDto
    {
        public long CategoryId { get; set;}
        public string Category {  get; set;}
        public long CandidateId { get; set;}
        public string CandidateUrl { get; set;}

    }
}
