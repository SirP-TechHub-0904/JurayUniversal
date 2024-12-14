using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
    // public class EModeratorResultModel : PageModel
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class EModeratorResultModel : PageModel
    {

        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public EModeratorResultModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Moderator Moderator { get; set; }
        public ParticipantList ParticipantList { get; set; }

        public List<AccountModeratorEvaluation> ModeratorEvaluations { get; set; }

        public async Task<IActionResult> OnGetAsync(long mid, long tid)
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
            Moderator = await _context.Moderators
                .Include(x => x.User)
            .Include(x => x.TimeTable)
                .FirstOrDefaultAsync(m => m.Id == mid && m.TimeTableId == tid);
            ModeratorEvaluations = await _context.AccountModeratorEvaluations
                .Include(x => x.EvaluationQuestion)
                .Include(x => x.Moderator).ThenInclude(x => x.User)
                .Include(x => x.TimeTable)
                .Where(m => m.ModeratorId == mid && m.TimeTableId == tid && m.ParticipantListId == ParticipantList.Id).ToListAsync();
            if (ModeratorEvaluations == null)
            {
                return RedirectToPage("./Evaluation");
            }


            return Page();
        }

        [BindProperty]
        public long TimetableId { get; set; }

        [BindProperty]
        public long ParticipantListId { get; set; }

        [BindProperty]
        public long EvaluationQuestionId { get; set; }

        [BindProperty]
        public long ModeratorId { get; set; }

    }

}
