using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Platform
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisForm> Forms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get all published forms
            Forms = await _context.MisForms
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
