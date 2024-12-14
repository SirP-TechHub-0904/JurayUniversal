using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Persistence.EF.SQL.Migrations;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class EvaluationModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public EvaluationModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //public List<Moderator> Moderators { get; set; }

        public IQueryable<Moderator> Moderators { get; set; }
        public IQueryable<string> TimeTables { get; set; }
        public string Desc { get; set; }
        public string PreviousWeek { get; set; }
        public string PreviousWeekTitle { get; set; }
        public string NextWeek { get; set; }
        public string NextWeekTitle { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }
        public string Subtitle { get; set; }
        public string MainTitle { get; set; }

        public string All {  get; set; }
        public long PID {  get; set; }

        public async Task<IActionResult> OnGetAsync(string searchdate = null, string all = "all")
        {
            var user = await _userManager.GetUserAsync(User);
            var participant = await _context.ParticipantLists
                .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

            if (participant == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Evaluation");
            }
            PID = participant.Id;
            Moderators = _context.Moderators.Include(x => x.TimeTable)
                .Include(x => x.User)
                .Where(m => m.TimeTable.CourseId == participant.CourseId && m.TimeTable.IsLecture == true).AsQueryable();

            DateTime givenDate = DateTime.Today;
            if (!string.IsNullOrEmpty(searchdate))
            {
                try
                {
                    givenDate = DateTime.Parse(searchdate);
                }
                catch (FormatException)
                {
                    TempData["error"] = "Invalid date format";
                    return RedirectToPage("/Evaluation");
                }
            }

            TempData["sharedate"] = givenDate.Date.ToString("dd MMMM yyyy");

            if (!string.IsNullOrEmpty(searchdate))
            {
                TempData["date"] = givenDate.ToString("ddd dd MMM, yyyy");
            }
            if(all == "all" && string.IsNullOrEmpty(searchdate))
            {
                Moderators = Moderators               
               .AsQueryable();

                All = "All Moderators";
            }
            else { 
            Moderators = Moderators
                .Where(ob => ob.TimeTable.Date.Date == givenDate.Date)
                .AsQueryable();
            }

            Title = givenDate.ToString("dddd") + " - " + givenDate.ToString("dd MMMM yyyy");

            return Page();
        }


    }

}
