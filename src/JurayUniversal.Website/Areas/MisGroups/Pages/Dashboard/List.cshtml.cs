using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Dashboard
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ListModel(DashboardDbContext context)
        {
            _context = context;
        }

        public string GroupAlias { get; set; } = "Group";

        public IList<MisGroup> MisGroups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
            {
                GroupAlias = settings.GroupAlias;
            }

            MisGroups = await _context.MisGroups
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
