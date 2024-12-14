using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
       [Authorize]
    public class SoftDetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public SoftDetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public XProject XProject { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            XProject = await _context.XProjects
                .Include(x => x.Sections)
         .ThenInclude(x => x.Tasks)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (XProject == null)
            {
                return NotFound();
            }
            string uid = _userManager.GetUserId(HttpContext.User);
            var listUsers = XProject.Sections.SelectMany(x => x.Tasks)
                                    .SelectMany(t => new[] { t.UserId, t.TestedById, t.ApprovedById }).Distinct()
                                    .ToList();
            if (User.IsInRole("mSuperAdmin") || User.IsInRole("SubAdmin") || listUsers.Contains(uid))
            {


                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }


        public async Task<IActionResult> OnPostUpdateStatus(ProjectTaskStatus TaskStatus, long id)
        {
            var taskcheck = await _context.XProjectTasks.FirstOrDefaultAsync(x => x.Id == id);
            taskcheck.Status = TaskStatus;
            _context.Attach(taskcheck).State = EntityState.Modified;


            await _context.SaveChangesAsync();


            var prj = await _context.XProjectSections.FirstOrDefaultAsync(x => x.Id == taskcheck.XProjectSectionId);


            return RedirectToPage("./Details", new { id = prj.XProjectId });
        }

    }
}
