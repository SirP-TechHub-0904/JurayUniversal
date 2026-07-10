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
    public class PreviewModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public PreviewModel(DashboardDbContext context)
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
    }
}
