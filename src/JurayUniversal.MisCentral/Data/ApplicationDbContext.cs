using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MasterTenantCatalog> MasterTenantCatalogs { get; set; } = null!;
        public DbSet<MasterTenantFeatureSubscription> MasterTenantFeatureSubscriptions { get; set; } = null!;
        public DbSet<MasterFeatureCatalog> MasterFeatureCatalogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MasterTenantCatalog>(entity =>
            {
                entity.HasIndex(e => e.TenantKey).IsUnique();
                entity.HasIndex(e => e.PrimaryDomain).IsUnique();
            });

            builder.Entity<MasterFeatureCatalog>().HasData(
                new MasterFeatureCatalog { Id = 1, FeatureKey = "MisUserManagement", Name = "User Directory & KPIs", Category = "Core & Administration", IconClass = "bi-people-fill", IsCompulsory = true, IsActive = true, SortOrder = 1, Description = "Centralized identity governance, membership directories, and performance indicators." },
                new MasterFeatureCatalog { Id = 2, FeatureKey = "MisGroups", Name = "Committees & Workgroups", Category = "Core & Administration", IconClass = "bi-diagram-3-fill", IsCompulsory = false, IsActive = true, SortOrder = 2, Description = "Collaboration groups, board workspaces, discussions, and file sharing." },
                new MasterFeatureCatalog { Id = 3, FeatureKey = "MisReports", Name = "Interval Reporting Hub", Category = "Reports & Surveys", IconClass = "bi-clock-history", IsCompulsory = false, IsActive = true, SortOrder = 3, Description = "Automated structured period reporting, workflow approvals, and audit trails." },
                new MasterFeatureCatalog { Id = 4, FeatureKey = "MisForms", Name = "Dynamic Form Builder & Surveys", Category = "Reports & Surveys", IconClass = "bi-ui-checks-grid", IsCompulsory = false, IsActive = true, SortOrder = 4, Description = "No-code dynamic questionnaire builder with response aggregation." },
                new MasterFeatureCatalog { Id = 5, FeatureKey = "MisFinances", Name = "Dues, Levies & Contributions", Category = "Financials & Grants", IconClass = "bi-wallet2", IsCompulsory = false, IsActive = true, SortOrder = 5, Description = "Dues tracking, payment verification, statements, and revenue tracking." },
                new MasterFeatureCatalog { Id = 6, FeatureKey = "MisTicketing", Name = "Events & Ticket Engine", Category = "Financials & Grants", IconClass = "bi-ticket-perforated-fill", IsCompulsory = false, IsActive = true, SortOrder = 6, Description = "Event registration, pass issuing, ticketing, and attendee validation." },
                new MasterFeatureCatalog { Id = 7, FeatureKey = "MisGrants", Name = "Grant & Endowment Tracking", Category = "Financials & Grants", IconClass = "bi-award-fill", IsCompulsory = false, IsActive = true, SortOrder = 7, Description = "Beneficiary verification, milestone approvals, and disbursement tracking." },
                new MasterFeatureCatalog { Id = 8, FeatureKey = "MisProposals", Name = "Proposal & Project Pipeline", Category = "Financials & Grants", IconClass = "bi-kanban-fill", IsCompulsory = false, IsActive = true, SortOrder = 8, Description = "Project phase management, proposal reviews, and progress tracking." },
                new MasterFeatureCatalog { Id = 9, FeatureKey = "MisFleet", Name = "Fleet & Asset Inventory", Category = "Operations & Logistics", IconClass = "bi-truck", IsCompulsory = false, IsActive = true, SortOrder = 9, Description = "Asset register, maintenance tracking, driver logs, and logistics." },
                new MasterFeatureCatalog { Id = 10, FeatureKey = "MisRecruitment", Name = "Talent & Staff Recruitment", Category = "Operations & Logistics", IconClass = "bi-person-badge-fill", IsCompulsory = false, IsActive = true, SortOrder = 10, Description = "Job openings, applicant screening, scoring, and candidate hiring pipeline." },
                new MasterFeatureCatalog { Id = 11, FeatureKey = "MisVoting", Name = "E-Voting & Polls", Category = "Engagement & Communication", IconClass = "bi-check2-square", IsCompulsory = false, IsActive = true, SortOrder = 11, Description = "Secure cryptographic ballot management, candidate setup, and realtime results." },
                new MasterFeatureCatalog { Id = 12, FeatureKey = "MisAlumni", Name = "Alumni Directory & Mentorship", Category = "Engagement & Communication", IconClass = "bi-mortarboard-fill", IsCompulsory = false, IsActive = true, SortOrder = 12, Description = "Alumni networking hub, mentor matchmaking, and career bulletin." },
                new MasterFeatureCatalog { Id = 13, FeatureKey = "MisMassComm", Name = "Broadcast SMS & Push Alerts", Category = "Engagement & Communication", IconClass = "bi-broadcast-pin", IsCompulsory = false, IsActive = true, SortOrder = 13, Description = "Direct SMS communication, automated broadcast dispatches, and logs." },
                new MasterFeatureCatalog { Id = 14, FeatureKey = "MisNewsletter", Name = "Email Campaigns & Newsletters", Category = "Engagement & Communication", IconClass = "bi-envelope-paper-heart-fill", IsCompulsory = false, IsActive = true, SortOrder = 14, Description = "Rich HTML newsletter templates, automated email list campaigns, and analytics." },
                new MasterFeatureCatalog { Id = 15, FeatureKey = "MisProjects", Name = "Sprint Tasks & Milestones", Category = "Operations & Logistics", IconClass = "bi-list-task", IsCompulsory = false, IsActive = true, SortOrder = 15, Description = "Task boards, sprint backlogs, assignments, and milestone checklists." },
                new MasterFeatureCatalog { Id = 16, FeatureKey = "MisManual", Name = "System Guides & Manuals", Category = "Core & Administration", IconClass = "bi-book-half", IsCompulsory = false, IsActive = true, SortOrder = 16, Description = "Embedded documentation, step-by-step operations guides, and SOP manuals." }
            );
        }
    }
}
