using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Participants
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class CourseListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CourseListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Course> Course { get; set; }
        public CourseCategory CourseCategory { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            Course = await _context.Courses
                .Include(x=>x.CourseCategory).OrderByDescending(x=>x.Year.Year)
                .Where(x=>x.CourseCategoryId == id)
                .ToListAsync();

            CourseCategory = await _context.CourseCategories.FirstOrDefaultAsync(x=>x.Id == id);
            return Page();
        }
    }
}
