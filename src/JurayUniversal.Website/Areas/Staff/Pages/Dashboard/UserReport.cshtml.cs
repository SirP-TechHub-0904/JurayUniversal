using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class UserReportModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public UserReportModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IQueryable<Report> Report { get; set; }
        public IQueryable<string> Events { get; set; }
        public string Desc { get; set; }
        public string PreviousWeek { get; set; }
        public string PreviousWeekTitle { get; set; }
        public string NextWeek { get; set; }
        public string NextWeekTitle { get; set; }
        public string Title { get; set; }

        public async Task OnGetAsync(string searchdate = null)
        {
            var user = await _userManager.GetUserAsync(User);
            DateTime querydate = DateTime.Today;
            DateTime checkdate = DateTime.Today;

            DateTime startOfWeek = querydate.AddDays(-1 * Convert.ToInt32(querydate.DayOfWeek)).AddDays(1);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            if (searchdate == null)
            {

                Report = from s in _context.Report.Include(x => x.User).OrderBy(x => x.Date)
                             .Where(x => x.UserId == user.Id)
                                  .Where(ob => startOfWeek <= ob.Date && ob.Date < endOfWeek)
                             select s;
                searchdate = DateTime.UtcNow.Date.ToString();
                checkdate = DateTime.UtcNow;
            }
            else
            {
                querydate = DateTime.Parse(searchdate).Date;
                startOfWeek = querydate.AddDays(-1 * Convert.ToInt32(querydate.DayOfWeek)).AddDays(1);
                endOfWeek = startOfWeek.AddDays(6);
                Report = from s in _context.Report.Include(x => x.User).OrderBy(x => x.Date)
                             .Where(x => x.UserId == user.Id)
                                 .Where(ob => startOfWeek <= ob.Date && ob.Date < endOfWeek)
                             select s;
                searchdate = querydate.Date.ToString("ddd dd, MMM, yyyy");
                checkdate = querydate;
            }
            int gdhd = Report.Count();
            TempData["date"] = searchdate;





            DateTime mondayOfLastWeek = checkdate.AddDays(-(int)checkdate.DayOfWeek - 6);
            DateTime mondayOfNextWeek = checkdate.AddDays(-(int)checkdate.DayOfWeek + 8);
            PreviousWeek = mondayOfLastWeek.Date.ToString("yyyy-MM-dd");
            NextWeek = mondayOfNextWeek.Date.ToString("yyyy-MM-dd");
            PreviousWeekTitle = "Prev " + mondayOfLastWeek.Date.ToString("dd MMMM") + " to " + mondayOfLastWeek.Date.AddDays(4).ToString("dd MMMM");
            NextWeekTitle = "Next " + mondayOfNextWeek.Date.ToString("dd MMMM") + " to " + mondayOfNextWeek.Date.AddDays(4).ToString("dd MMMM");
            Title = startOfWeek.ToString("dd") + " - " + endOfWeek.ToString("dd MMMM yyyy");
        }

    }
}
