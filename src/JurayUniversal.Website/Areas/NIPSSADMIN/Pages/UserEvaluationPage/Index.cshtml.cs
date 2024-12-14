using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.UserEvaluationPage
{
     [Microsoft.AspNetCore.Authorization.Authorize]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
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

        public string All { get; set; }
        public long PID { get; set; }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchdate = null, string all = "all", long id = 0)
        {
            if(id == 0)
            {
                return NotFound();
            }

           Course = await _context.Courses
                .Include(x => x.CourseCategory).OrderByDescending(x => x.Year.Year)
                .FirstOrDefaultAsync(x => x.CourseCategoryId == id);
            if (Course == null)
            {
                return NotFound();
            }
            Moderators = _context.Moderators.Include(x => x.TimeTable)
                .Include(x => x.User)
                .Where(m => m.TimeTable.CourseId == Course.Id && m.TimeTable.IsLecture == true).AsQueryable();

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
                    return RedirectToPage("/Index");
                }
            }

            TempData["sharedate"] = givenDate.Date.ToString("dd MMMM yyyy");

            if (!string.IsNullOrEmpty(searchdate))
            {
                TempData["date"] = givenDate.ToString("ddd dd MMM, yyyy");
            }
            if (all == "all" && string.IsNullOrEmpty(searchdate))
            {
                Moderators = Moderators
               .AsQueryable();

                All = "All Moderators";
            }
            else
            {
                Moderators = Moderators
                    .Where(ob => ob.TimeTable.Date.Date == givenDate.Date)
                    .AsQueryable();
            }

            Title = givenDate.ToString("dddd") + " - " + givenDate.ToString("dd MMMM yyyy");

            return Page();
        }


    }

}
