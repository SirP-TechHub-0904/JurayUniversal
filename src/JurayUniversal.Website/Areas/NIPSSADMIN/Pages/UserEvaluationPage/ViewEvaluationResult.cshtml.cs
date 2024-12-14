using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.UserEvaluationPage
{
    public class ViewEvaluationResultModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ViewEvaluationResultModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<AccountModeratorEvaluation> AccountModeratorEvaluations { get; set; }
        public Moderator Moderator { get; set; }

        public List<ParticipantEvaluationViewModel> ParticipantEvaluations { get; set; }
        public Dictionary<string, Dictionary<int, int>> QuestionResponseCounts { get; set; }
        public async Task<IActionResult> OnGetAsync(long id, long tid)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Moderator = await _context.Moderators
                .Include(x => x.User)
                .Include(x => x.TimeTable)
                .FirstOrDefaultAsync(x=>x.Id == id);


            AccountModeratorEvaluations = await _context.AccountModeratorEvaluations
                .Include(x => x.TimeTable)
                .Include(x => x.EvaluationQuestion)
                .Include(x => x.ParticipantList).ThenInclude(x => x.User)
                .Where(m => m.ModeratorId == id && m.TimeTableId == tid).ToListAsync();

            var groupedEvaluations = AccountModeratorEvaluations
           .GroupBy(e => e.ParticipantListId)
           .Select(g => new ParticipantEvaluationViewModel
           {
               ParticipantId = g.Key ?? 0,
               ParticipantName = g.First().ParticipantList.User.FullnameX, // Assuming you have a Participant navigation property
               Responses = g.ToDictionary(e => e.EvaluationQuestion.AbbreviatedQuestion, e => e.Response)
           })
           .ToList();

            ParticipantEvaluations = groupedEvaluations;
            QuestionResponseCounts = AccountModeratorEvaluations
           .GroupBy(e => e.EvaluationQuestion.AbbreviatedQuestion)
           .ToDictionary(
               g => g.Key,
               g => g.GroupBy(e => e.Response).ToDictionary(e => e.Key, e => e.Count())
           );
            return Page();
        }

    }
    public class ParticipantEvaluationViewModel
    {
        public long ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public Dictionary<string, int> Responses { get; set; }
    }
    public class ModeratorEvaluationViewModel
    {
        public long Id { get; set; }
        public long ModeratorId { get; set; }
        public string ParticipantName { get; set; }
        public Dictionary<string, int> Responses { get; set; }
    }
}
