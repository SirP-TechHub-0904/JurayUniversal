using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class NewFeatureModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public NewFeatureModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public long ProjectId { get; set; }
        public async Task<IActionResult> OnGet(long id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xXProject = await _context.XProjects.FirstOrDefaultAsync(m => m.Id == id);

            if (xXProject == null)
            {
                return NotFound();
            }
            ProjectId = xXProject.Id;
            return Page();
        }

        [BindProperty]
        public XProjectSection XProjectSection { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
           
            XProjectSection.CreatedDate = DateTime.UtcNow.AddHours(1);
            _context.XProjectSections.Add(XProjectSection);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successfully";

            return RedirectToPage("./Details", new {id = XProjectSection.XProjectId});
        }
    }
}
