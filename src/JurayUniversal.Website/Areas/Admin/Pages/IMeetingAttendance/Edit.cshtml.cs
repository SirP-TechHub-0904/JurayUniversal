using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.Admin.Pages.IMeetingAttendance
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Secretary")]

    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MeetingAttendance MeetingAttendance { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MeetingAttendance = await _context.MeetingAttendances
                .Include(m => m.Meeting)
                .Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);

            if (MeetingAttendance == null)
            {
                return NotFound();
            }
           ViewData["MeetingId"] = new SelectList(_context.Meetings, "Id", "Id");
           ViewData["UserId"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MeetingAttendance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingAttendanceExists(MeetingAttendance.Id))
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

        private bool MeetingAttendanceExists(long id)
        {
            return _context.MeetingAttendances.Any(e => e.Id == id);
        }
    }
}
