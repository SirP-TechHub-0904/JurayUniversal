using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.EVoting
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var courseList = await _context.Courses.Include(x => x.CourseCategory).ToListAsync();
            var oulist = courseList.Select(x => new DropDownListCourse
            {
                Id = x.Id,
                Title = x.CourseCategory.Name + " (" + x.CourseCategory.Abbreviation + " " + x.SecNumber + " - " + x.Year.ToString("yyyy") + ")",
            }).ToList();

            ViewData["CourseId"] = new SelectList(oulist, "Id", "Title");
             
            return Page();
        }

        [BindProperty]
        public Evoting Evoting { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            

            _context.Evotings.Add(Evoting);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
