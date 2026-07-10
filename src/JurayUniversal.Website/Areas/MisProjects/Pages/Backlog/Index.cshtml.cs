using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Backlog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisProject Project { get; set; }
        public IList<MisProjectSprint> Sprints { get; set; } = new List<MisProjectSprint>();
        public IList<MisProjectTask> BacklogTasks { get; set; } = new List<MisProjectTask>();

        public async Task<IActionResult> OnGetAsync(long projectId)
        {
            Project = await _context.MisProjects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (Project == null)
            {
                return NotFound();
            }

            Sprints = await _context.MisProjectSprints
                .Include(s => s.Tasks)
                .AsNoTracking()
                .Where(s => s.MisProjectId == projectId)
                .OrderBy(s => s.StartDate)
                .ToListAsync();

            BacklogTasks = await _context.MisProjectTasks
                .Include(t => t.AssignedToUser)
                .AsNoTracking()
                .Where(t => t.MisProjectId == projectId && t.MisProjectSprintId == null)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostStartSprintAsync(long sprintId, long projectId)
        {
            var sprint = await _context.MisProjectSprints.FindAsync(sprintId);
            if (sprint != null)
            {
                sprint.Status = Domain.Enum.Enum.MisSprintStatus.Active;
                await _context.SaveChangesAsync();
                TempData["success"] = "Sprint started! Tasks are now visible on the Active Board.";
            }
            return RedirectToPage(new { projectId = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAssignTaskAsync(long taskId, long? sprintId)
        {
            var task = await _context.MisProjectTasks.FindAsync(taskId);
            if (task == null) return new JsonResult(new { success = false, message = "Task not found" });

            task.MisProjectSprintId = sprintId;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
