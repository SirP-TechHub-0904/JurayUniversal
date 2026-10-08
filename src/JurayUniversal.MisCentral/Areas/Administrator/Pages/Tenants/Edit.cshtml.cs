using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Tenants
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _centralDbContext;

        public EditModel(ApplicationDbContext centralDbContext)
        {
            _centralDbContext = centralDbContext;
        }

        [BindProperty]
        public TenantEditInputModel Input { get; set; } = new TenantEditInputModel();

        public MasterTenantCatalog Tenant { get; set; } = null!;

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tenant = await _centralDbContext.MasterTenantCatalogs.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            Tenant = tenant;
            Input = new TenantEditInputModel
            {
                Id = tenant.Id,
                OrganizationName = tenant.OrganizationName,
                TenantKey = tenant.TenantKey,
                PrimaryDomain = tenant.PrimaryDomain,
                CustomDomain = tenant.CustomDomain,
                AdminEmail = tenant.AdminEmail,
                ContactPhone = tenant.ContactPhone,
                Sector = tenant.Sector,
                IsActive = tenant.IsActive,
                Notes = tenant.Notes
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var t = await _centralDbContext.MasterTenantCatalogs.FindAsync(Input.Id);
                if (t == null) return NotFound();
                Tenant = t;
                return Page();
            }

            var tenant = await _centralDbContext.MasterTenantCatalogs.FindAsync(Input.Id);
            if (tenant == null)
            {
                return NotFound();
            }

            tenant.OrganizationName = Input.OrganizationName.Trim();
            tenant.CustomDomain = string.IsNullOrWhiteSpace(Input.CustomDomain) ? null : Input.CustomDomain.Trim().ToLowerInvariant();
            tenant.AdminEmail = string.IsNullOrWhiteSpace(Input.AdminEmail) ? null : Input.AdminEmail.Trim().ToLowerInvariant();
            tenant.ContactPhone = Input.ContactPhone;
            tenant.Sector = Input.Sector;
            tenant.IsActive = Input.IsActive;
            tenant.Notes = Input.Notes;
            tenant.UpdatedAtUtc = DateTime.UtcNow;

            await _centralDbContext.SaveChangesAsync();

            SuccessMessage = $"Tenant '{tenant.OrganizationName}' successfully updated.";
            return RedirectToPage("./Index");
        }

        public class TenantEditInputModel
        {
            public int Id { get; set; }

            [Required]
            [MaxLength(200)]
            [Display(Name = "Organization Name")]
            public string OrganizationName { get; set; } = string.Empty;

            public string TenantKey { get; set; } = string.Empty;
            public string PrimaryDomain { get; set; } = string.Empty;

            [MaxLength(255)]
            [Display(Name = "Custom Domain")]
            public string? CustomDomain { get; set; }

            [EmailAddress]
            [MaxLength(255)]
            [Display(Name = "Admin Email")]
            public string? AdminEmail { get; set; }

            [MaxLength(50)]
            [Display(Name = "Contact Phone")]
            public string? ContactPhone { get; set; }

            [MaxLength(100)]
            [Display(Name = "Sector")]
            public string? Sector { get; set; }

            [Display(Name = "Active Status")]
            public bool IsActive { get; set; } = true;

            [MaxLength(1000)]
            [Display(Name = "Internal Notes")]
            public string? Notes { get; set; }
        }
    }
}
