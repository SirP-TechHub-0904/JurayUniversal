using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class ModeratorEvaluationPageModel : PageModel
    {

        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ModeratorEvaluationPageModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Moderator Moderator { get; set; }
        public ParticipantList ParticipantList { get; set; }

        public List<EvaluationQuestion> EvaluationQuestions { get; set; }

        public async Task<IActionResult> OnGetAsync(long id, long tid)
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
                .FirstOrDefaultAsync(m => m.Id == id && m.TimeTableId == tid);
            if (Moderator == null)
            {
                return RedirectToPage("./Evaluation");
            }

            EvaluationQuestions = await _context.EvaluationQuestions.Where(x => x.Publish == true).ToListAsync();

            return Page();
        }

        [BindProperty]
        public long TimetableId { get; set; }

        [BindProperty]
        public int ParticipantListId { get; set; }

        [BindProperty]
        public long EvaluationQuestionId { get; set; }

        [BindProperty]
        public long ModeratorId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            StringBuilder formInfo = new StringBuilder();
            var evaluationData = new List<(long moderatorId, long timeTableId, int participantListId, long evaluationQuestionId, int response)>();
            // Initialize counters for each status


            foreach (var key in Request.Form.Keys)
            {
                string value = Request.Form[key];
                formInfo.AppendLine($"{key}: {value}");

                // Check if the key starts with "AttendanceResult_"
                if (key.StartsWith("EvaluationResult_"))
                {
                    // Extract the attendance ID from the key
                    if (long.TryParse(key.Substring("EvaluationResult_".Length), out long evaluationQuestionId))
                    {
                        // Get the enum value from the form
                        if (int.TryParse(value, out int xresult))
                        {
                            // Add the extracted attendance ID and status to the list
                            evaluationData.Add((ModeratorId, TimetableId, ParticipantListId, evaluationQuestionId, xresult));

                        }
                        else
                        {
                            // Handle invalid enum value
                            // Perhaps return an error response or log the issue
                        }
                    }
                    else
                    {
                        // Handle invalid attendance ID
                        // Perhaps return an error response or log the issue
                    }
                }
            }

            //check
            var check = await _context.AccountModeratorEvaluations.FirstOrDefaultAsync(x => x.ParticipantListId == ParticipantListId && x.TimeTableId == TimetableId && x.ModeratorId == ModeratorId);
            if (check == null)
            {
                foreach (var (moderatorId, timeTableId, participantListId, evaluationQuestionId, response) in evaluationData)
                {
                    // Check if the ParticipantListId exists in the ParticipantLists table
                    var participantListExists = await _context.ParticipantLists.AnyAsync(pl => pl.Id == participantListId);
                    if (!participantListExists)
                    {
                        // Log or handle the error (e.g., skip the insertion, return an error message)
                        TempData["error"] = $"ParticipantListId {participantListId} does not exist.";
                        continue; // Skip this iteration
                    }

                    AccountModeratorEvaluation nxtest = new AccountModeratorEvaluation();
                    nxtest.ModeratorId = moderatorId;
                    nxtest.TimeTableId = timeTableId;
                    nxtest.ParticipantListId = participantListId;
                    nxtest.EvaluationQuestionId = evaluationQuestionId;
                    nxtest.Response = response;
                    await _context.AccountModeratorEvaluations.AddAsync(nxtest);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Successful";
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred: {ex.Message}";
                }
            }


            return RedirectToPage("./EvaluationResult");
            // Your existing code continues here...
        }
    }
}
