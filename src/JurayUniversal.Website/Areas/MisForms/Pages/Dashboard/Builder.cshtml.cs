using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Dashboard
{
    [Authorize]
    public class BuilderModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public BuilderModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisForm Form { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Form = await _context.MisForms.FirstOrDefaultAsync(m => m.Id == id);

            if (Form == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateSchemaAsync(long id, [FromBody] UpdateSchemaRequest request)
        {
            var form = await _context.MisForms.FindAsync(id);
            if (form == null) return NotFound();

            form.SchemaJson = request.SchemaJson;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostTogglePublishAsync(long id)
        {
            var form = await _context.MisForms.FindAsync(id);
            if (form == null) return NotFound();

            form.IsPublished = !form.IsPublished;
            await _context.SaveChangesAsync();

            TempData["success"] = form.IsPublished ? "Form has been published successfully!" : "Form has been unpublished.";
            return RedirectToPage("./Index");
        }

        public class UpdateSchemaRequest
        {
            public string SchemaJson { get; set; }
        }
    }
}
