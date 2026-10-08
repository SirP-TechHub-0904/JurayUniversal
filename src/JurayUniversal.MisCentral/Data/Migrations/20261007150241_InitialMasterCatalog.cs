using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JurayUniversal.MisCentral.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMasterCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterFeatureCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCompulsory = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterFeatureCatalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterTenantCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrimaryDomain = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomDomain = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApiUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DatabaseName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTenantCatalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterTenantFeatureSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterTenantCatalogId = table.Column<int>(type: "int", nullable: false),
                    FeatureKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FeatureName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EnabledAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrantedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTenantFeatureSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTenantFeatureSubscriptions_MasterTenantCatalogs_MasterTenantCatalogId",
                        column: x => x.MasterTenantCatalogId,
                        principalTable: "MasterTenantCatalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MasterFeatureCatalogs",
                columns: new[] { "Id", "Category", "Description", "FeatureKey", "IconClass", "IsActive", "IsCompulsory", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, "Core & Administration", "Centralized identity governance, membership directories, and performance indicators.", "MisUserManagement", "bi-people-fill", true, true, "User Directory & KPIs", 1 },
                    { 2, "Core & Administration", "Collaboration groups, board workspaces, discussions, and file sharing.", "MisGroups", "bi-diagram-3-fill", true, false, "Committees & Workgroups", 2 },
                    { 3, "Reports & Surveys", "Automated structured period reporting, workflow approvals, and audit trails.", "MisReports", "bi-clock-history", true, false, "Interval Reporting Hub", 3 },
                    { 4, "Reports & Surveys", "No-code dynamic questionnaire builder with response aggregation.", "MisForms", "bi-ui-checks-grid", true, false, "Dynamic Form Builder & Surveys", 4 },
                    { 5, "Financials & Grants", "Dues tracking, payment verification, statements, and revenue tracking.", "MisFinances", "bi-wallet2", true, false, "Dues, Levies & Contributions", 5 },
                    { 6, "Financials & Grants", "Event registration, pass issuing, ticketing, and attendee validation.", "MisTicketing", "bi-ticket-perforated-fill", true, false, "Events & Ticket Engine", 6 },
                    { 7, "Financials & Grants", "Beneficiary verification, milestone approvals, and disbursement tracking.", "MisGrants", "bi-award-fill", true, false, "Grant & Endowment Tracking", 7 },
                    { 8, "Financials & Grants", "Project phase management, proposal reviews, and progress tracking.", "MisProposals", "bi-kanban-fill", true, false, "Proposal & Project Pipeline", 8 },
                    { 9, "Operations & Logistics", "Asset register, maintenance tracking, driver logs, and logistics.", "MisFleet", "bi-truck", true, false, "Fleet & Asset Inventory", 9 },
                    { 10, "Operations & Logistics", "Job openings, applicant screening, scoring, and candidate hiring pipeline.", "MisRecruitment", "bi-person-badge-fill", true, false, "Talent & Staff Recruitment", 10 },
                    { 11, "Engagement & Communication", "Secure cryptographic ballot management, candidate setup, and realtime results.", "MisVoting", "bi-check2-square", true, false, "E-Voting & Polls", 11 },
                    { 12, "Engagement & Communication", "Alumni networking hub, mentor matchmaking, and career bulletin.", "MisAlumni", "bi-mortarboard-fill", true, false, "Alumni Directory & Mentorship", 12 },
                    { 13, "Engagement & Communication", "Direct SMS communication, automated broadcast dispatches, and logs.", "MisMassComm", "bi-broadcast-pin", true, false, "Broadcast SMS & Push Alerts", 13 },
                    { 14, "Engagement & Communication", "Rich HTML newsletter templates, automated email list campaigns, and analytics.", "MisNewsletter", "bi-envelope-paper-heart-fill", true, false, "Email Campaigns & Newsletters", 14 },
                    { 15, "Operations & Logistics", "Task boards, sprint backlogs, assignments, and milestone checklists.", "MisProjects", "bi-list-task", true, false, "Sprint Tasks & Milestones", 15 },
                    { 16, "Core & Administration", "Embedded documentation, step-by-step operations guides, and SOP manuals.", "MisManual", "bi-book-half", true, false, "System Guides & Manuals", 16 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterTenantCatalogs_PrimaryDomain",
                table: "MasterTenantCatalogs",
                column: "PrimaryDomain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterTenantCatalogs_TenantKey",
                table: "MasterTenantCatalogs",
                column: "TenantKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterTenantFeatureSubscriptions_MasterTenantCatalogId",
                table: "MasterTenantFeatureSubscriptions",
                column: "MasterTenantCatalogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterFeatureCatalogs");

            migrationBuilder.DropTable(
                name: "MasterTenantFeatureSubscriptions");

            migrationBuilder.DropTable(
                name: "MasterTenantCatalogs");
        }
    }
}
