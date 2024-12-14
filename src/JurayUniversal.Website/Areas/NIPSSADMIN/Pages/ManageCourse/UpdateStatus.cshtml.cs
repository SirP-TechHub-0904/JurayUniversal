using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.ManageCourse
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class UpdateStatusModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UpdateStatusModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var category = await _context.CourseCategories.FirstOrDefaultAsync(x => x.Id == Course.CourseCategoryId);
            if (Course.CourseStatus == Domain.Enum.Enum.CourseStatus.Active)
            {
                //ccheck if there is any active course
                var activecourse = await _context.Courses.FirstOrDefaultAsync(x => x.CourseCategoryId == category.Id && x.CourseStatus == Domain.Enum.Enum.CourseStatus.Active);
                if (activecourse == null)
                {
                    var updatecourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == Course.Id);
                    updatecourse.CourseStatus = Domain.Enum.Enum.CourseStatus.Active;
                    _context.Attach(updatecourse).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Successful";
                    return RedirectToPage("./Details", new { id = Course.Id });
                }
                TempData["error"] = "There is an active course. Kindly change its status to concluded or upcoming";
            }
            else
            {
                var updatecourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == Course.Id);
                updatecourse.CourseStatus = Course.CourseStatus;
                _context.Attach(updatecourse).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["success"] = "Successful";
                return RedirectToPage("./Details", new { id = Course.Id });
            }

            return Page();

        }

        private bool CourseExists(long id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }

}
