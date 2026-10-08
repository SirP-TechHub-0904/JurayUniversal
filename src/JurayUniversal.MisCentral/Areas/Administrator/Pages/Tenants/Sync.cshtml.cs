using System.Threading.Tasks;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Tenants
{
    public class SyncModel : PageModel
    {
        private readonly ITenantSyncService _tenantSyncService;

        public SyncModel(ITenantSyncService tenantSyncService)
        {
            _tenantSyncService = tenantSyncService;
        }

        public TenantSyncSummary? Summary { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Summary = await _tenantSyncService.SyncAllDatabasesToMasterCatalogAsync();
            return Page();
        }
    }
}
