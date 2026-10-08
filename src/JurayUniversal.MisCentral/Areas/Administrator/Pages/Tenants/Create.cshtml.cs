using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Tenants
{
    public class CreateModel : PageModel
    {
        private readonly ITenantOnboardingService _onboardingService;
        private readonly ApplicationDbContext _centralDbContext;

        public CreateModel(ITenantOnboardingService onboardingService, ApplicationDbContext centralDbContext)
        {
            _onboardingService = onboardingService;
            _centralDbContext = centralDbContext;
        }

        [BindProperty]
        public TenantInputModel Input { get; set; } = new TenantInputModel();

        public List<MasterFeatureCatalog> AvailableFeatures { get; set; } = new List<MasterFeatureCatalog>();

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            AvailableFeatures = await _centralDbContext.MasterFeatureCatalogs
                .OrderBy(f => f.Category)
                .ThenBy(f => f.SortOrder)
                .ToListAsync();

            // Default select all features
            Input.SelectedFeatureKeys = AvailableFeatures.Select(f => f.FeatureKey).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AvailableFeatures = await _centralDbContext.MasterFeatureCatalogs
                    .OrderBy(f => f.Category)
                    .ThenBy(f => f.SortOrder)
                    .ToListAsync();
                return Page();
            }

            var cleanKey = Input.TenantKey.Trim().ToLowerInvariant();
            if (ReservedWorkspaceNames.IsReserved(cleanKey))
            {
                ModelState.AddModelError("Input.TenantKey", $"The tenant key '{cleanKey}' is reserved and cannot be assigned.");
                AvailableFeatures = await _centralDbContext.MasterFeatureCatalogs.ToListAsync();
                return Page();
            }

            var isAvailable = await _onboardingService.IsTenantKeyAvailableAsync(cleanKey);
            if (!isAvailable)
            {
                ModelState.AddModelError("Input.TenantKey", $"The tenant key '{cleanKey}' is already taken. Please choose another unique key.");
                AvailableFeatures = await _centralDbContext.MasterFeatureCatalogs.ToListAsync();
                return Page();
            }

            var request = new TenantOnboardingRequest
            {
                OrganizationName = Input.OrganizationName.Trim(),
                TenantKey = cleanKey,
                AdminFirstName = Input.AdminFirstName.Trim(),
                AdminLastName = Input.AdminLastName.Trim(),
                AdminEmail = Input.AdminEmail.Trim(),
                AdminPassword = Input.AdminPassword,
                PhoneNumber = Input.PhoneNumber,
                CustomDomain = Input.CustomDomain,
                Sector = Input.Sector,
                SelectedFeatureKeys = Input.SelectedFeatureKeys ?? new List<string>()
            };

            var result = await _onboardingService.OnboardNewTenantAsync(request);

            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Tenant '{request.OrganizationName}' successfully provisioned and ready for access!";
                return RedirectToPage("./Index");
            }
            else
            {
                ErrorMessage = result.Message;
                AvailableFeatures = await _centralDbContext.MasterFeatureCatalogs.ToListAsync();
                return Page();
            }
        }

        public class TenantInputModel
        {
            [Required]
            [Display(Name = "Organization / Business Name")]
            [MaxLength(200)]
            public string OrganizationName { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Tenant Subdomain Key")]
            [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Only letters, numbers, and hyphens are allowed.")]
            [MaxLength(60)]
            public string TenantKey { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Admin First Name")]
            [MaxLength(100)]
            public string AdminFirstName { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Admin Last Name")]
            [MaxLength(100)]
            public string AdminLastName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Admin Email")]
            [MaxLength(255)]
            public string AdminEmail { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Admin Password")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
            public string AdminPassword { get; set; } = string.Empty;

            [Phone]
            [Display(Name = "Admin Phone Number")]
            [MaxLength(50)]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Custom Domain (Optional)")]
            [MaxLength(255)]
            public string? CustomDomain { get; set; }

            [Display(Name = "Industry Sector")]
            [MaxLength(100)]
            public string? Sector { get; set; }

            public List<string> SelectedFeatureKeys { get; set; } = new List<string>();
        }
    }
}
