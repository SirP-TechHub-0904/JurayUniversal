using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Tenants
{
    public class FeaturesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FeaturesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MasterTenantCatalog Tenant { get; set; } = default!;

        public class FeatureStateItem
        {
            public int FeatureId { get; set; }
            public string FeatureKey { get; set; } = string.Empty;
            public string FeatureName { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public bool IsEnabled { get; set; }
        }

        [BindProperty]
        public List<FeatureStateItem> FeaturesList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.MasterTenantCatalogs
                .Include(t => t.FeatureSubscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tenant == null)
            {
                return NotFound();
            }

            Tenant = tenant;

            var allFeatures = await _context.MasterFeatureCatalogs
                .OrderBy(f => f.Name)
                .ToListAsync();

            FeaturesList = allFeatures.Select(f =>
            {
                var sub = tenant.FeatureSubscriptions.FirstOrDefault(s => s.FeatureKey.Equals(f.FeatureKey, StringComparison.OrdinalIgnoreCase));
                return new FeatureStateItem
                {
                    FeatureId = f.Id,
                    FeatureKey = f.FeatureKey,
                    FeatureName = f.Name,
                    Description = f.Description,
                    IsEnabled = sub != null ? sub.IsEnabled : true // default to enabled if no record exists
                };
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var tenant = await _context.MasterTenantCatalogs
                .Include(t => t.FeatureSubscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tenant == null)
            {
                return NotFound();
            }

            Tenant = tenant;

            foreach (var item in FeaturesList)
            {
                var existingSub = tenant.FeatureSubscriptions.FirstOrDefault(s => s.FeatureKey.Equals(item.FeatureKey, StringComparison.OrdinalIgnoreCase));
                if (existingSub != null)
                {
                    existingSub.IsEnabled = item.IsEnabled;
                }
                else
                {
                    tenant.FeatureSubscriptions.Add(new MasterTenantFeatureSubscription
                    {
                        MasterTenantCatalogId = tenant.Id,
                        FeatureKey = item.FeatureKey,
                        FeatureName = item.FeatureName,
                        IsEnabled = item.IsEnabled,
                        EnabledAtUtc = DateTime.UtcNow,
                        GrantedBy = "Central SuperAdmin"
                    });
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Module features successfully updated for " + tenant.OrganizationName;

            return RedirectToPage("./Details", new { id = tenant.Id });
        }
    }
}
