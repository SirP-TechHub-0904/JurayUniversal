using System.Collections.Generic;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Tenants
{
    public class IndexModel : PageModel
    {
        private readonly ITenantOnboardingService _onboardingService;

        public IndexModel(ITenantOnboardingService onboardingService)
        {
            _onboardingService = onboardingService;
        }

        public List<MasterTenantCatalog> Tenants { get; set; } = new List<MasterTenantCatalog>();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            Tenants = await _onboardingService.GetAllTenantsAsync();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int id, bool isActive)
        {
            var success = await _onboardingService.UpdateTenantStatusAsync(id, isActive);
            if (success)
            {
                SuccessMessage = $"Tenant status successfully updated to {(isActive ? "Active" : "Suspended")}.";
            }
            else
            {
                ErrorMessage = "Failed to update tenant status.";
            }

            return RedirectToPage();
        }
    }
}
