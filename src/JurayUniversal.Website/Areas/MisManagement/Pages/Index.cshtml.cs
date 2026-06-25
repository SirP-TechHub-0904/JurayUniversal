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
            public string Name { get; set; }
            public string ModuleFolder { get; set; }
            public string Icon { get; set; }
            public string Description { get; set; }
        }

        public static readonly List<PredefinedFeature> AvailableFeatures = new List<PredefinedFeature>
        {
            new PredefinedFeature { ModuleFolder = "MisUserManagement", Name = "User Management", Icon = "fa-users", Description = "Manage user directory, categories, and KPIs." },
            new PredefinedFeature { ModuleFolder = "MisGroups", Name = "Committees & Groups", Icon = "fa-sitemap", Description = "Manage committees and working groups." },
            new PredefinedFeature { ModuleFolder = "MisAlumni", Name = "Alumni Network Portal", Icon = "fa-graduation-cap", Description = "Alumni network and portal management." },
            new PredefinedFeature { ModuleFolder = "MisReports", Name = "Dynamic Interval Reports", Icon = "fa-clock", Description = "Create and schedule dynamic interval reports." },
            new PredefinedFeature { ModuleFolder = "MisSurveys", Name = "Custom Surveys & Forms", Icon = "fa-poll-h", Description = "Form builder and survey analytics." },
            new PredefinedFeature { ModuleFolder = "MisManual", Name = "Interactive User Manual", Icon = "fa-book-open", Description = "Platform interactive user manual." },
            new PredefinedFeature { ModuleFolder = "MisDues", Name = "Dues & Subscriptions", Icon = "fa-file-invoice-dollar", Description = "Manage platform dues and member subscriptions." },
            new PredefinedFeature { ModuleFolder = "MisTicketing", Name = "Event & Ticket Engine", Icon = "fa-ticket-alt", Description = "Event management and ticket engine." },
            new PredefinedFeature { ModuleFolder = "MisDonations", Name = "Contributions & Donations", Icon = "fa-hand-holding-usd", Description = "Manage contributions and donations." },
            new PredefinedFeature { ModuleFolder = "MisGrants", Name = "Grant & Proposal Pipelines", Icon = "fa-briefcase", Description = "Grant applications and proposal pipelines." },
            new PredefinedFeature { ModuleFolder = "MisRecruitment", Name = "Recruitment & ATS", Icon = "fa-user-plus", Description = "Job applications and applicant tracking." },
            new PredefinedFeature { ModuleFolder = "MisVoting", Name = "Elections & E-Voting", Icon = "fa-vote-yea", Description = "Platform elections and e-voting system." },
            new PredefinedFeature { ModuleFolder = "MisAssets", Name = "Asset Trackers", Icon = "fa-building", Description = "Company asset ledger and fleet management." },
            new PredefinedFeature { ModuleFolder = "MisProjects", Name = "Project & Task Boards", Icon = "fa-tasks", Description = "Project and task boards." },
            new PredefinedFeature { ModuleFolder = "MisChat", Name = "Chats & Channels", Icon = "fa-comments", Description = "Direct messaging and team group channels." },
            new PredefinedFeature { ModuleFolder = "MisMassComm", Name = "Mass Broadcast (Email/SMS)", Icon = "fa-bullhorn", Description = "Mass broadcast via email and SMS." },
            new PredefinedFeature { ModuleFolder = "MisAI", Name = "BYOK AI Studio", Icon = "fa-brain", Description = "Bring your own key AI studio." },
            new PredefinedFeature { ModuleFolder = "MisHelpdesk", Name = "Helpdesk Support Tickets", Icon = "fa-life-ring", Description = "Helpdesk support ticketing system." },
            new PredefinedFeature { ModuleFolder = "MisWhistleblowing", Name = "Anonymous Whistleblowing", Icon = "fa-user-secret", Description = "Anonymous whistleblowing." },
            new PredefinedFeature { ModuleFolder = "MisDMS", Name = "Document Vault (DMS)", Icon = "fa-folder-open", Description = "Document management system and vault." },
            new PredefinedFeature { ModuleFolder = "MisLMS", Name = "Learning Portal (LMS)", Icon = "fa-university", Description = "Learning management system portal." },
            new PredefinedFeature { ModuleFolder = "MisVisitorManagement", Name = "Visitor & Gate Security", Icon = "fa-id-badge", Description = "Visitor management and gate security." }
        };
    }
}
