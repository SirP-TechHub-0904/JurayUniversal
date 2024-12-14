using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.QAs
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Directing,Management")]

    public class CourseListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CourseListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get; set; }

        public async Task OnGetAsync()
        {
            Course = await _context.Courses
                .Include(x => x.CourseCategory)
                .Include(x => x.SubThemes).OrderByDescending(x => x.Year).ToListAsync();
        }
    }
}
