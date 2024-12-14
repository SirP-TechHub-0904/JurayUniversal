using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.Admin.Pages.ITraining
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IQueryable<Training> Training { get; set; }
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
            // Get the first day of the current month
            DateTime firstDayOfMonth = new DateTime(querydate.Year, querydate.Month, 1);

            // Get the last day of the current month
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            if (searchdate == null)
            {

                Training = from s in _context.Trainings
                           .Include(x => x.Trainner)
                           .Include(x => x.Moderator)

                           .OrderBy(x => x.Date)

                                  .Where(ob => firstDayOfMonth <= ob.Date && ob.Date < lastDayOfMonth)
                           select s;
                searchdate = DateTime.UtcNow.Date.ToString();
                checkdate = DateTime.UtcNow;
            }
            else
            {
                querydate = DateTime.Parse(searchdate).Date;
                // Get the first day of the current month
                firstDayOfMonth = new DateTime(querydate.Year, querydate.Month, 1);

                // Get the last day of the current month
                lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                Training = from s in _context.Trainings.Include(x => x.Trainner).Include(x => x.Moderator).OrderBy(x => x.Date)

                                  .Where(ob => firstDayOfMonth <= ob.Date && ob.Date < lastDayOfMonth)
                           select s;
                searchdate = querydate.Date.ToString("ddd dd, MMM, yyyy");
                checkdate = querydate;
            }
            int gdhd = Training.Count();
            TempData["date"] = searchdate;
            TempData["title"] = querydate.Date.ToString("MMMM yyyy"); ;


        }

    }
}
