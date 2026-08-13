using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisManagement.Pages
{
    [Authorize(Roles = "mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisFeature> MisFeatures { get; set; } = new List<MisFeature>();

        public async Task<IActionResult> OnGetAsync()
        {
            MisFeatures = await _context.MisFeatures.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(long id)
        {
            var feature = await _context.MisFeatures.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            if (feature.Status == JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Disabled)
            {
                feature.Status = JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Active;
                
                // Set initial billing cycle if they have a non-free plan
                if (feature.BillingDuration != JurayUniversal.Domain.Enum.Enum.FeatureBillingDuration.Free && feature.CostPerDuration > 0)
                {
                    feature.NextBillingDate = feature.BillingDuration == JurayUniversal.Domain.Enum.Enum.FeatureBillingDuration.Monthly 
                        ? System.DateTime.UtcNow.AddMonths(1) 
                        : (feature.BillingDuration == JurayUniversal.Domain.Enum.Enum.FeatureBillingDuration.Yearly 
                            ? System.DateTime.UtcNow.AddYears(1) 
                            : System.DateTime.UtcNow);
                            
                    // Generate Initial Invoice
                    var invoice = new MisFeatureInvoice
                    {
                        MisFeatureId = feature.Id,
                        AmountDue = feature.CostPerDuration,
                        InvoiceDate = System.DateTime.UtcNow,
                        DueDate = System.DateTime.UtcNow.AddDays(feature.GracePeriodDays),
                        IsPaid = false
                    };
                    _context.MisFeatureInvoices.Add(invoice);
                }
            }
            else
            {
                feature.Status = JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Disabled;
                feature.NextBillingDate = null;
            }

            _context.Entry(feature).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = $"Feature '{feature.FeatureName}' has been {(feature.Status == JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Active ? "Enabled" : "Disabled")}.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEnablePresetAsync(string moduleFolder, string name, string description)
        {
            if (string.IsNullOrEmpty(moduleFolder) || string.IsNullOrEmpty(name))
                return BadRequest();

            // Check if it already exists
            var existing = await _context.MisFeatures.FirstOrDefaultAsync(x => x.ModuleFolder == moduleFolder);
            if (existing != null)
                return RedirectToPage();

            var feature = new MisFeature
            {
                FeatureName = name,
                ModuleFolder = moduleFolder,
                Description = description ?? "",
                BillingDuration = JurayUniversal.Domain.Enum.Enum.FeatureBillingDuration.Monthly,
                CostPerDuration = 0,
                GracePeriodDays = 0,
                Status = JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Active,
                NextBillingDate = null
            };

            _context.MisFeatures.Add(feature);
            await _context.SaveChangesAsync();

            TempData["success"] = $"Feature '{name}' has been successfully enabled!";
            return RedirectToPage();
        }

        public class PredefinedFeature
        {
            public string ModuleFolder { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string Link { get; set; }
        }

        public static readonly List<PredefinedFeature> AvailableFeatures = new List<PredefinedFeature>
        {
            new PredefinedFeature { ModuleFolder = "MisUserManagement", Name = "User Management", Icon = "bi bi-people-fill", Description = "Manage user directory, categories, and KPIs.", Category = "core", Link = "/MisUserManagement/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisGroups", Name = "Committees & Groups", Icon = "bi bi-diagram-3-fill", Description = "Manage committees and working groups.", Category = "operations", Link = "/MisGroups/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisAlumni", Name = "Alumni Network Portal", Icon = "bi bi-mortarboard-fill", Description = "Alumni network and portal management.", Category = "operations", Link = "/MisAlumni/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisReports", Name = "Dynamic Interval Reports", Icon = "bi bi-clock-history", Description = "Create and schedule dynamic interval reports.", Category = "reports", Link = "/MisReports/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisForms", Name = "Form Builder & Surveys", Icon = "bi bi-ui-checks", Description = "Create custom surveys, registration forms, and feedback modules.", Category = "tech", Link = "/MisForms/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisFinances", Name = "Dues & Donations", Icon = "bi bi-cash-coin", Description = "Complete guide for managing compulsory dues and voluntary donations.", Category = "financials", Link = "/MisFinances/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisTicketing", Name = "Event & Ticket Engine", Icon = "bi bi-ticket-perforated-fill", Description = "Event management and ticket engine.", Category = "operations", Link = "/MisTicketing/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisGrants", Name = "Grant Ledger & Funding", Icon = "bi bi-bank", Description = "Track awarded external funding and milestones.", Category = "financials", Link = "/MisGrants/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisProposals", Name = "Proposal Pipeline", Icon = "bi bi-file-earmark-text-fill", Description = "Internal pitches and external grant proposals.", Category = "operations", Link = "/MisProposals/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisRecruitment", Name = "Recruitment & ATS", Icon = "bi bi-person-plus-fill", Description = "Job applications and applicant tracking.", Category = "core", Link = "/MisRecruitment/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisVoting", Name = "Elections & E-Voting", Icon = "bi bi-box-seam", Description = "Platform elections and e-voting system.", Category = "operations", Link = "/MisVoting/Dashboard/Elections/Index" },
            new PredefinedFeature { ModuleFolder = "MisFleet", Name = "Fleet & Equipment Tracker", Icon = "bi bi-building-gear", Description = "Company asset ledger and fleet management.", Category = "operations", Link = "/MisFleet/Dashboard/Equipment/Index" },
            new PredefinedFeature { ModuleFolder = "MisProjects", Name = "Project and Task Board", Icon = "bi bi-list-task", Description = "Project and task boards.", Category = "operations", Link = "/MisProjects/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisMassComm", Name = "SMS Mass Comm", Icon = "bi bi-chat-text-fill", Description = "Mass broadcast via SMS.", Category = "tech", Link = "/MisMassComm/Dashboard/Index" },
            new PredefinedFeature { ModuleFolder = "MisNewsletter", Name = "Email Newsletter", Icon = "bi bi-send-fill", Description = "Newsletter subscriptions and campaigns.", Category = "tech", Link = "/MisNewsletter/Dashboard/Wallet/Index" }
        };
    }
}
