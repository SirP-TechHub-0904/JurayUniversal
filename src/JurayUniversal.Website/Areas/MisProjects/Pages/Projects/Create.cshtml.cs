using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Projects
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisProject MisProject { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MisProject.Status = Domain.Enum.Enum.MisProjectStatus.Active;
            
            _context.MisProjects.Add(MisProject);
            await _context.SaveChangesAsync();

            TempData["success"] = "Project created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
