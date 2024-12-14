using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using System.Text.Encodings.Web;

namespace JurayUniversal.Website.Areas.Admin.Pages.IAttendance
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IQueryable<Attendance> Attendance { get; set; }
        public IQueryable<string> Events { get; set; }
        public string Desc { get; set; }
        public string PreviousWeek { get; set; }
        public string PreviousWeekTitle { get; set; }
        public string NextWeek { get; set; }
        public string NextWeekTitle { get; set; }
        public string Title { get; set; }

        public async Task OnGetAsync(string searchdate = null)
        {
            //DateTime startOfWeek = DateTime.Now.AddDays(-1 * Convert.ToInt32(DateTime.Now.DayOfWeek)).AddDays(1);
            //DateTime endOfWeek = startOfWeek.AddDays(6);
            DateTime querydate = DateTime.Today;
            if (searchdate == null)
            {

                Attendance = from s in _context.Attendances.Include(x=>x.User).OrderByDescending(x => x.Date)
                                 .Where(ob => ob.Date.Date == DateTime.UtcNow.Date)
                             select s;
                searchdate = DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");
            }
            else
            {
                querydate = DateTime.Parse(searchdate).Date;
                 
                Attendance = from s in _context.Attendances.Include(x => x.User).OrderByDescending(x => x.Date)
                                 .Where(ob => ob.Date.Date == querydate.Date)
                             select s;
                searchdate = querydate.Date.ToString("ddd dd MMM, yyyy");
            }
            int gdhd = Attendance.Count();
            TempData["date"] = searchdate;


        }

    }
}
