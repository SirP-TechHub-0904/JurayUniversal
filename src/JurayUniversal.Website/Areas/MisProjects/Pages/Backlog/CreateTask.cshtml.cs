using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Backlog
{
    [Authorize]
    public class CreateTaskModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateTaskModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisProject MisProject { get; set; }

        [BindProperty]
        public MisProjectTask MisProjectTask { get; set; }

        public async Task<IActionResult> OnGetAsync(long projectId)
        {
            MisProject = await _context.MisProjects.FirstOrDefaultAsync(m => m.Id == projectId);

            if (MisProject == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remove navigation properties from validation
            ModelState.Remove("MisProjectTask.MisProject");
            ModelState.Remove("MisProjectTask.MisProjectSprint");
            ModelState.Remove("MisProjectTask.AssignedToUser");

            if (!ModelState.IsValid)
            {
                MisProject = await _context.MisProjects.FirstOrDefaultAsync(m => m.Id == MisProjectTask.MisProjectId);
                return Page();
            }

            MisProjectTask.Status = Domain.Enum.Enum.MisTaskStatus.ToDo;
            
            _context.MisProjectTasks.Add(MisProjectTask);
            await _context.SaveChangesAsync();

            TempData["success"] = "Work item added to backlog successfully!";
            return RedirectToPage("./Index", new { projectId = MisProjectTask.MisProjectId });
        }
    }
}
