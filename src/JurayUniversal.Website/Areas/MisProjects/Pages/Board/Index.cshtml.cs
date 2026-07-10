using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Board
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
        public MisProjectSprint ActiveSprint { get; set; }
        public IList<MisProjectTask> Tasks { get; set; } = new List<MisProjectTask>();

        public async Task<IActionResult> OnGetAsync(long projectId)
        {
            Project = await _context.MisProjects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (Project == null)
            {
                return NotFound();
            }

            // Find Active Sprint
            ActiveSprint = await _context.MisProjectSprints
                .AsNoTracking()
                .Where(s => s.MisProjectId == projectId && s.Status == Domain.Enum.Enum.MisSprintStatus.Active)
                .FirstOrDefaultAsync();

            if (ActiveSprint != null)
            {
                Tasks = await _context.MisProjectTasks
                    .Include(t => t.AssignedToUser)
                    .AsNoTracking()
                    .Where(t => t.MisProjectSprintId == ActiveSprint.Id)
                    .ToListAsync();
            }
            else
            {
                // Fallback: If no active sprint, maybe just show backlog or empty board
                Tasks = new List<MisProjectTask>();
            }

            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostUpdateStatusAsync(long taskId, Domain.Enum.Enum.MisTaskStatus newStatus)
        {
            var task = await _context.MisProjectTasks.FindAsync(taskId);
            if (task == null) return new JsonResult(new { success = false, message = "Task not found" });

            task.Status = newStatus;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
