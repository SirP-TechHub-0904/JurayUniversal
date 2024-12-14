using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.TimeTablePage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Editor")]

    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TimeTable TimeTable { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeTable = await _context.TimeTables
                .Include(t => t.Course).FirstOrDefaultAsync(m => m.Id == id);

            if (TimeTable == null)
            {
                return NotFound();
            }
            var courseList = await _context.Courses.Include(x => x.CourseCategory).ToListAsync();
            var oulist = courseList.Select(x => new DropDownListCourse
            {
                Id = x.Id,
                Title = x.CourseCategory.Name + " (" + x.CourseCategory.Abbreviation + " " + x.SecNumber + " - " + x.Year.ToString("yyyy") + ")",
            }).ToList();

            ViewData["CourseId"] = new SelectList(oulist, "Id", "Title"); 
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            _context.Attach(TimeTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeTableExists(TimeTable.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TimeTableExists(long id)
        {
            return _context.TimeTables.Any(e => e.Id == id);
        }
    }
}
