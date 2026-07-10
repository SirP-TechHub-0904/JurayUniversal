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
    public class CreateSprintModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateSprintModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisProject MisProject { get; set; }

        [BindProperty]
        public MisProjectSprint MisProjectSprint { get; set; }

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
            ModelState.Remove("MisProjectSprint.MisProject");
            ModelState.Remove("MisProjectSprint.Tasks");

            if (!ModelState.IsValid)
            {
                MisProject = await _context.MisProjects.FirstOrDefaultAsync(m => m.Id == MisProjectSprint.MisProjectId);
                return Page();
            }

            MisProjectSprint.Status = Domain.Enum.Enum.MisSprintStatus.Planned;
            
            _context.MisProjectSprints.Add(MisProjectSprint);
            await _context.SaveChangesAsync();

            TempData["success"] = "Sprint created successfully!";
            return RedirectToPage("./Index", new { projectId = MisProjectSprint.MisProjectId });
        }
    }
}
