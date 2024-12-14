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
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Directing,Management")]

    public class StudyGroupsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public StudyGroupsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudyGroupCategory StudyGroupCategory { get; set; }
        [BindProperty]
        public string StaffId { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudyGroupCategory = await _context.StudyGroupCategory
                .Include(x => x.StudyGroups)
                .ThenInclude(x => x.User)
                .Include(s => s.Course).FirstOrDefaultAsync(m => m.Id == id);

            if (StudyGroupCategory == null)
            {
                return NotFound();
            }
            // Fetch data and map it to DTO
            var listsDS = _context.StaffManagers
                .Include(x => x.User)

                .Where(x => x.User.Role == "Directing" || x.User.Role == "Staff" || x.User.Role == "Management")
                .OrderBy(x => x.StaffId)
                .Select(x => new StaffManagerDto
                {
                    UserId = x.User.Id,
                    Fullname = x.User.FullnameX
                })
                .ToList();

            var StudyGroupCategoryUserId = StudyGroupCategory.StudyGroups.Select(x => x.UserId).AsEnumerable();

            var slsectStaffId = listsDS.Where(x => !StudyGroupCategoryUserId.Contains(x.UserId));
            // Assign data to ViewData
            ViewData["DirectingStaff"] = new SelectList(slsectStaffId, "UserId", "Fullname");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            StudyGroup sngroup = new StudyGroup();
            
            sngroup.UserId = StaffId;
            var staff = await _context.StaffManagers.Include(x=>x.User).FirstOrDefaultAsync(x=>x.UserId == StaffId);
            if (staff != null)
            {
                sngroup.Position = staff.User.PositionOrRank;
            }


            sngroup.StudyGroupCategoryId = StudyGroupCategory.Id;
            _context.StudyGroups.Add(sngroup);
            await _context.SaveChangesAsync();
            try
            {
                TempData["success"] = "successful";
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupCategoryExists(StudyGroupCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./StudyGroups", new { id = StudyGroupCategory.Id });
        }

        private bool StudyGroupCategoryExists(long id)
        {
            return _context.StudyGroupCategory.Any(e => e.Id == id);
        }
    }
    // Define a DTO class
    public class StaffManagerDto
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
    }
}
