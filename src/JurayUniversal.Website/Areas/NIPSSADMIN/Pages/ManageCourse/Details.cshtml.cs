using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.ManageCourse
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Directing,Management")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }
        [BindProperty]
        public CourseSubTheme CourseSubTheme { get; set; }

        [BindProperty]
        public StudyGroupCategory StudyGroupCategory { get; set; }
        public List<StudyGroupCategory> StudyGroupCategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses.Include(x=>x.CourseCategory)
                .Include(x=>x.SubThemes)
                .Include(x=>x.StudyGroupCategories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

        
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            

            _context.CourseSubThemes.Add(CourseSubTheme);
            await _context.SaveChangesAsync();
            TempData["success"] = "Added";
            return RedirectToPage("./Details", new {id = CourseSubTheme.CourseId});
        }

        public async Task<IActionResult> OnPostDeleteAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CourseSubTheme = await _context.CourseSubThemes.FindAsync(id);

            if (CourseSubTheme != null)
            {
                _context.CourseSubThemes.Remove(CourseSubTheme);
                await _context.SaveChangesAsync();
                TempData["success"] = "Deleted";
                return RedirectToPage("./Details", new { id = CourseSubTheme.CourseId });
            }
            TempData["error"] = "Unable to Delete";
            return RedirectToPage("./Details", new { id = CourseSubTheme.CourseId });
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostStudyGroupAsync()
        {


            _context.StudyGroupCategory.Add(StudyGroupCategory);
            await _context.SaveChangesAsync();
            TempData["success"] = "Added";
            return RedirectToPage("./Details", new { id = StudyGroupCategory.CourseId });
        }

        public async Task<IActionResult> OnPostStudyGroupDeleteAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudyGroupCategory = await _context.StudyGroupCategory.FindAsync(id);

            if (StudyGroupCategory != null)
            {
                _context.StudyGroupCategory.Remove(StudyGroupCategory);
                await _context.SaveChangesAsync();
                TempData["success"] = "Deleted";
                return RedirectToPage("./Details", new { id = StudyGroupCategory.CourseId });
            }
            TempData["error"] = "Unable to Delete";
            return RedirectToPage("./Details", new { id = StudyGroupCategory.CourseId });
        }
    }
}
