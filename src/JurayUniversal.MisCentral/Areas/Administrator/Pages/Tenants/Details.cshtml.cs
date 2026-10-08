using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Tenants
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MasterTenantCatalog Tenant { get; set; } = default!;
        public List<MasterTenantFeatureSubscription> SubscribedFeatures { get; set; } = new();

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
            SubscribedFeatures = tenant.FeatureSubscriptions.ToList();

            return Page();
        }
    }
}
