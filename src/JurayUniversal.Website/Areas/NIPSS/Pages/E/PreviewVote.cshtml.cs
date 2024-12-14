using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.E
{
    [Authorize]
    public class PreviewVoteModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public PreviewVoteModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public long VotingId { get; set; }


        public Evoting Evoting { get; set; }
        public List<SelectedVoteDto> SelectedVotes { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
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
            if (TempData["SelectedVotes"] != null)
            {

                // Deserialize SelectedVotes from TempData
                var json = TempData["SelectedVotes"] as string;
                SelectedVotes = JsonConvert.DeserializeObject<List<SelectedVoteDto>>(json);
                TempData["PostSelectedVotes"] = json;
            }
            else
            {
                TempData["error"] = "Uanble to Validate Voted. Try again";
                return RedirectToPage("./Index", new { id = id });
            }
            VotingId = Evoting.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (TempData["PostSelectedVotes"] != null)
            {
                // Retrieve SelectedVotes from TempData
                var json = TempData["PostSelectedVotes"] as string;
                var selectedVotes = JsonConvert.DeserializeObject<List<SelectedVoteDto>>(json);

                foreach (var vts in selectedVotes)
                {
                    var getuser = await _context.UserVotes.FirstOrDefaultAsync(x => x.VoteCondidateId == vts.CandidateId && x.VoterUserId == user.Id);
                    if (getuser == null)
                    {
                        UserVote userVote = new UserVote();
                        userVote.VoterUserId = user.Id;
                        userVote.VoteCondidateId = vts.CandidateId;
                        userVote.VoteCategoryId = vts.CategoryId;
                        userVote.EncryptCode = XGenerateRandomAlphaNumericUniqueId(6);
                        _context.UserVotes.Add(userVote);
                    }
                }
                await _context.SaveChangesAsync();

            }
            else
            {
                TempData["error"] = "Uanble to Validate Voted. Try again";
                return RedirectToPage("./Index", new { id = VotingId });
            }

            TempData["completevotetoken"] = "VotingCompleted";
            return RedirectToPage("./VotingComplete", new { id = VotingId }); // Redirect back to the same page after submission
        }
        static string XGenerateRandomAlphaNumericUniqueId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}