using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
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

        public IQueryable<Report> Report { get; set; }
        public IQueryable<Attendance> Attendance { get; set; }
        public Setting Setting { get; set; }
        public IList<XProjectTask> XProjectTask { get; set; }

        public async Task OnGetAsync(string searchdate = null)
        {
            var user = await _userManager.GetUserAsync(User);
            DateTime querydate = DateTime.Today;
            DateTime checkdate = DateTime.Today;
             
            // Get the first day of the current month
            DateTime firstDayOfMonth = new DateTime(querydate.Year, querydate.Month, 1);

            // Get the last day of the current month
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            if (searchdate == null)
            {

                Report = from s in _context.Report 
                         .Where(x=>x.UserId == user.Id)

                                  .Where(ob => firstDayOfMonth <= ob.Date && ob.Date < lastDayOfMonth)
                           select s;

                Attendance = from s in _context.Attendances.Where(x => x.UserId == user.Id)
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
                Report = from s in _context.Report.Where(x => x.UserId == user.Id)
                                  .Where(ob => firstDayOfMonth <= ob.Date && ob.Date < lastDayOfMonth)
                           select s;

                Attendance = from s in _context.Attendances.Where(x => x.UserId == user.Id)
                                 .Where(ob => firstDayOfMonth <= ob.Date && ob.Date < lastDayOfMonth)
                         select s;
                searchdate = querydate.Date.ToString("ddd dd, MMM, yyyy");
                checkdate = querydate;
            }
             
            TempData["date"] = searchdate;
            TempData["title"] = querydate.Date.ToString("MMMM yyyy"); ;
            Setting = await _context.Settings.FirstOrDefaultAsync();


            //
            XProjectTask = await _context.XProjectTasks
                .Include(x=>x.XProjectSection).ThenInclude(x=>x.XProject)
                .Include(x => x.ApprovedBy)
                .Include(x => x.TestedBy)
                .Include(x => x.User)
                .Where(x=>x.UserId.Contains(user.Id) || x.TestedById.Contains(user.Id) || x.ApprovedById.Contains(user.Id))
                .ToListAsync();

        }

    }
}
