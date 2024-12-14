using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
     [Microsoft.AspNetCore.Authorization.Authorize]

     public class AccountTimeTableModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public AccountTimeTableModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IQueryable<TimeTable> TimeTable { get; set; }
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

        public async Task OnGetAsync(string date = null, string searchdate = null, string searchdatecancel = null, string id = null, string x = null)
        {
            //name="searchdatecancel" value="cancel"
            IQueryable<TimeTable> evct = from s in _context.TimeTables.OrderByDescending(x => x.Date)
                                             //.Where(x=>x.Date.DayOfWeek == DateTime.UtcNow.DayOfWeek)
                                         select s;
            if (searchdatecancel == "cancel")
            {
                searchdate = null;
            }
            DateTime givenDate = DateTime.Today;

            if (id != null)
            {
                string ddate = id.Insert(2, "/");
                string ddatex = ddate.Insert(6, "/");
                if (ddatex.Contains("single"))
                {
                    ddatex = ddatex.Replace("single", "");
                    date = null;
                    searchdate = ddatex;
                }
                else
                {
                    //week
                    date = ddatex;
                    searchdate = null;
                }
            }
            if (date != null)
            {

                givenDate = DateTime.Parse(date).AddDays(1);
            }

            if (searchdate != null)
            {
                givenDate = DateTime.Parse(searchdate).AddDays(1);
            }

            DateTime startOfWeek = givenDate.AddDays(-1 * Convert.ToInt32(givenDate.DayOfWeek)).AddDays(1);
            DateTime endOfWeek = startOfWeek.AddDays(5);

            TempData["sharedate"] = givenDate.Date.ToString("dd MMMM yyyy");
            var dx = givenDate.ToString("ddMMMyyyy");
            string xdx = dx;

            if (searchdate != null)
            {

                TempData["date"] = givenDate.ToString("ddd dd MMM, yyyy");
                var query = evct
                   .Where(ob => ob.Date.Date == givenDate.Date)
                   .Select(x => x.Date.Date.ToString())
                   .Distinct().AsQueryable();
                var xquery = query.FirstOrDefault();
                var eventinfo = await _context.TimeTables.FirstOrDefaultAsync(x => x.Date.Date.ToString() == xquery);
                if (eventinfo != null)
                {
                    if (eventinfo.Note != null)
                    {
                        Desc = eventinfo.Note ?? "";
                    }
                }
                var sf = query.Contains("yyyy-MM-dd");
                TimeTables = query;
                xdx = xdx + "single";

            }
            else
            {
                var query = evct
                  .Where(ob => startOfWeek <= ob.Date && ob.Date < endOfWeek)
                  .Select(x => x.Date.Date.ToString())
                  .Distinct().AsQueryable();
                var xquery = query.FirstOrDefault();
                var eventinfo = await _context.TimeTables.FirstOrDefaultAsync(x => x.Date.Date.ToString() == xquery);
                if (eventinfo != null)
                {
                    if (eventinfo.Note != null)
                    {
                        Desc = eventinfo.Note ?? "";
                    }
                    MainTitle = eventinfo.Title;
                    Subtitle = eventinfo.SubTitle;
                    Module = eventinfo.Module;
                }
                TimeTables = query;
            }
            var callbackUrl = Url.Page(
                         "/",
                         pageHandler: null,
                         values: new { area = "", /*id = xdx*/ },
                         protocol: Request.Scheme);

            //string mi = $"{HtmlEncoder.Default.Encode(callbackUrl)}";
            //TempData["link"] = mi;


            DateTime mondayOfLastWeek = givenDate.AddDays(-(int)givenDate.DayOfWeek - 6);
            DateTime mondayOfNextWeek = givenDate.AddDays(-(int)givenDate.DayOfWeek + 8);
            PreviousWeek = mondayOfLastWeek.Date.ToString("dd MMMM yyyy");
            NextWeek = mondayOfNextWeek.Date.ToString("dd MMMM yyyy");
            PreviousWeekTitle = "Previous " + mondayOfLastWeek.Date.ToString("dd MMMM") + " to " + mondayOfLastWeek.Date.AddDays(4).ToString("dd MMMM");
            NextWeekTitle = "Next " + mondayOfNextWeek.Date.ToString("dd MMMM") + " to " + mondayOfNextWeek.Date.AddDays(4).ToString("dd MMMM");
            Title = startOfWeek.ToString("dd") + " - " + endOfWeek.ToString("dd MMMM yyyy");

           
        }



        //    public IList<TimeTable> TimeTable { get; set; }

        //    public async Task OnGetAsync()
        //    {

        //   //     DateTime today = DateTime.Today;
        //   //     int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        //   //     var xTimeTable = await _context.TimeTables
        //   //.Include(t => t.Course)
        //   //.Include(t => t.Moderators).ThenInclude(x => x.User)
        //   //.Where(t => t.Date.Date == today)
        //   //.ToListAsync();


        //   //     var yTimeTable = await _context.TimeTables
        //   // .Include(t => t.Course)
        //   // .Include(t => t.Moderators).ThenInclude(x => x.User)
        //   // .Where(t => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(t.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == weekNumber)
        //   // .ToListAsync();


        //   //     TimeTable = await _context.TimeTables
        //   //         .Include(t => t.Course)
        //   //         .Include(t => t.Moderators).ThenInclude(x => x.User)
        //   //         .ToListAsync();
        //    }
        //}
    }
}
