using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard.Schemas
{
    [Authorize(Policy = "MisReportsEditor")]
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisReportSchema Schema { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Schema = await _context.MisReportSchemas.FindAsync(id);

            if (Schema == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(Schema).State = EntityState.Modified;
            
            // To ensure CreatedAt doesn't get overwritten
            _context.Entry(Schema).Property(x => x.CreatedAt).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchemaExists(Schema.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["success"] = "Report Schema updated successfully.";
            return RedirectToPage("./Index");
        }

        private bool SchemaExists(long id)
        {
            return _context.MisReportSchemas.Any(e => e.Id == id);
        }
    }
}
