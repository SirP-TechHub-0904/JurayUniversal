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

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.ManageCourse
{
    public class UpdateStudyGroupAccountModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UpdateStudyGroupAccountModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudyGroup StudyGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudyGroup = await _context.StudyGroups
                .Include(s => s.StudyGroupCategory)
                .Include(s => s.User).FirstOrDefaultAsync(m => m.Id == id);

            if (StudyGroup == null)
            {
                return NotFound();
            }
           ViewData["StudyGroupCategoryId"] = new SelectList(_context.StudyGroupCategory, "Id", "Title");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var xStudyGroup = await _context.StudyGroups.FirstOrDefaultAsync(m => m.Id == StudyGroup.Id);
            xStudyGroup.Position = StudyGroup.Position;
            _context.Attach(xStudyGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return RedirectToPage("./StudyGroups", new {id = xStudyGroup.StudyGroupCategoryId});
        }

        private bool StudyGroupExists(long id)
        {
            return _context.StudyGroups.Any(e => e.Id == id);
        }
    }
}
