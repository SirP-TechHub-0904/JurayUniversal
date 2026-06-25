using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard.Schemas
{
    [Authorize(Policy = "MisReportsEditor")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisReportSchema> Schemas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Schemas = await _context.MisReportSchemas
                .Include(s => s.Participants)
                .OrderByDescending(s => s.CreatedAt).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(long id)
        {
            var schema = await _context.MisReportSchemas.FindAsync(id);
            if (schema != null)
            {
                schema.IsActive = !schema.IsActive;
                await _context.SaveChangesAsync();
                TempData["success"] = schema.IsActive ? "Schema activated." : "Schema deactivated.";
            }
            return RedirectToPage();
        }
    }
}
