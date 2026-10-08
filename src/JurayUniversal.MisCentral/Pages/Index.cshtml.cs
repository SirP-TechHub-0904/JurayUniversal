using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MasterFeatureCatalog> FeaturedModules { get; set; } = new();
        public int TotalTenantsCount { get; set; }
        public int TotalActiveModulesCount { get; set; }

        public async Task OnGetAsync()
        {
            FeaturedModules = await _context.MasterFeatureCatalogs
                .Where(f => f.IsActive)
                .OrderBy(f => f.SortOrder)
                .Take(8)
                .ToListAsync();

            TotalTenantsCount = await _context.MasterTenantCatalogs.CountAsync();
            TotalActiveModulesCount = await _context.MasterTenantFeatureSubscriptions.CountAsync(s => s.IsEnabled);
        }
    }
}
