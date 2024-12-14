using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class EvaluationResultModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public EvaluationResultModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ParticipantList ParticipantList { get; set; }

        public List<AccountModeratorEvaluation> ModeratorEvaluations { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            ParticipantList = await _context.ParticipantLists.Include(x => x.User)
                .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

            if (ParticipantList == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/Index");
            }

            var allEvaluations = await _context.AccountModeratorEvaluations
                .Include(x => x.Moderator).ThenInclude(x => x.TimeTable)
                .Where(m => m.ParticipantListId == ParticipantList.Id)
                .ToListAsync();

            //if (allEvaluations == null || !allEvaluations.Any())
            //{
            //    return RedirectToPage("./Evaluation");
            //}

            // Group by ModeratorId and TimeTable.Date, and select one evaluation from each group
            ModeratorEvaluations = allEvaluations
                .GroupBy(e => new { e.ModeratorId, Date = e.TimeTable.Date.Date })
                .Select(g => g.First())
                .ToList();

            return Page();
        }
    }
}
