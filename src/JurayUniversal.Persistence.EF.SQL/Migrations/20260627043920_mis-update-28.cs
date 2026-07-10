using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonorName",
                table: "MisGrants");

            migrationBuilder.AddColumn<string>(
                name: "ExecutionDuration",
                table: "MisProposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectEndDate",
                table: "MisProposals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectStartDate",
                table: "MisProposals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MisDonorId",
                table: "MisGrants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MisDonors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisDonors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisProposalPhases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisProposalId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisProposalPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisProposalPhases_MisProposals_MisProposalId",
                        column: x => x.MisProposalId,
                        principalTable: "MisProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 51L, 7L, "Scan guest QR codes for fast event check-in.", "QR Check-In Scanner", "/MisTicketing/Dashboard/Scan" },
                    { 52L, 7L, "Detailed view of an upcoming event including ticket availability.", "Event Details", "/MisTicketing/Platform/EventDetails" },
                    { 53L, 7L, "Finalize your ticket purchase and review your order.", "Checkout", "/MisTicketing/Platform/Checkout" },
                    { 54L, 7L, "Browse all upcoming events and secure your tickets.", "Upcoming Events", "/MisTicketing/Platform/Index" },
                    { 55L, 6L, "A complete administrative history of all system transactions.", "Transaction History", "/MisFinances/Dashboard/TransactionHistory" },
                    { 56L, 6L, "Detailed receipt and status of a specific payment.", "Transaction Details", "/MisFinances/Dashboard/TransactionDetails" },
                    { 57L, 6L, "Financial analytics dashboard tracking revenue and payments.", "Financial Analytics", "/MisFinances/Dashboard/Index" },
                    { 58L, 6L, "A history of all personal payments and contributions.", "My Payment History", "/MisFinances/Platform/TransactionHistory" },
                    { 59L, 1L, "Create a new organizational category.", "Create Category", "/MisUserManagement/Dashboard/Categories/Create" },
                    { 60L, 1L, "Modify an existing category.", "Edit Category", "/MisUserManagement/Dashboard/Categories/Edit" },
                    { 61L, 1L, "Remove a category permanently.", "Delete Category", "/MisUserManagement/Dashboard/Categories/Delete" },
                    { 62L, 4L, "Define a new required report and its deadlines.", "Create Schema", "/MisReports/Dashboard/Schemas/Create" },
                    { 63L, 4L, "Modify an existing report template.", "Edit Schema", "/MisReports/Dashboard/Schemas/Edit" },
                    { 64L, 4L, "Detailed view of an individual staff submission.", "Submission Details", "/MisReports/Dashboard/Details" }
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[,]
                {
                    { 8L, "MisSetting", "Global configuration and terminology settings for the MIS environment.", "System Configuration Guide" },
                    { 9L, "MisManagement", "Billing parameters, feature toggles, and overall module lifecycle management.", "Feature & Billing Guide" },
                    { 10L, "MisAuth", "Security preferences including Two-Factor Authentication (2FA), Authenticator App setup, and Recovery Codes.", "Security & Auth Guide" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 109L, "Scan the QR code displayed on the guest's ticket.", "QR Scanner", "Scanner", 51L, "GlobalEditor" },
                    { 110L, "Proceed to buy tickets for this event.", "Get Tickets Button", "Button", 52L, "" },
                    { 111L, "Confirm order details and proceed to payment.", "Pay Now Button", "Button", 53L, "" },
                    { 112L, "Browse upcoming events.", "Events List", "List", 54L, "" },
                    { 113L, "View the global ledger of all system transactions.", "Transaction Log", "Table", 55L, "GlobalViewer" },
                    { 114L, "Detailed receipt of the transaction.", "Receipt Details", "Card", 56L, "GlobalViewer" },
                    { 115L, "High-level overview of revenue streams.", "Financial Analytics", "Dashboard", 57L, "GlobalViewer" },
                    { 116L, "Your personal record of payments.", "My Transaction Log", "Table", 58L, "" },
                    { 117L, "Create the category.", "Save Category", "Button", 59L, "GlobalEditor" },
                    { 118L, "Save the modifications.", "Update Category", "Button", 60L, "GlobalEditor" },
                    { 119L, "Confirm deletion of category.", "Delete Confirm", "Button", 61L, "SystemAdmin" },
                    { 120L, "Save the new schema definition.", "Save Schema", "Button", 62L, "GlobalEditor" },
                    { 121L, "Update schema rules.", "Update Schema", "Button", 63L, "GlobalEditor" },
                    { 122L, "View the specific data submitted in this report.", "Submission Body", "Card", 64L, "GlobalViewer" }
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 65L, 8L, "Customize global terminology and application behavior.", "Global Configuration", "/MisSetting/Dashboard/Index" },
                    { 66L, 9L, "Update billing and lifecycle settings for a feature.", "Edit Feature Config", "/MisManagement/Edit" },
                    { 67L, 9L, "Register a new feature and define billing.", "Configure New Feature", "/MisManagement/Create" },
                    { 68L, 10L, "Overview of Two-Factor Authentication status.", "2FA Status", "/MisAuth/Auth/MisTwoFactorAuthentication" },
                    { 69L, 10L, "View emergency backup codes.", "Recovery Codes", "/MisAuth/Auth/MisShowRecoveryCodes" },
                    { 70L, 10L, "Reset the keys used by your authenticator app.", "Reset Authenticator", "/MisAuth/Auth/MisResetAuthenticator" },
                    { 71L, 10L, "Generate new emergency backup codes.", "Generate Recovery Codes", "/MisAuth/Auth/MisGenerateRecoveryCodes" },
                    { 72L, 10L, "Scan a QR code to link your authenticator app.", "Enable Authenticator", "/MisAuth/Auth/MisEnableAuthenticator" },
                    { 73L, 10L, "Turn off two-factor authentication.", "Disable 2FA", "/MisAuth/Auth/MisDisable2fa" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 123L, "Apply global terminology updates.", "Save Configuration", "Button", 65L, "SystemAdmin" },
                    { 124L, "Save changes to feature configuration.", "Save Config", "Button", 66L, "SystemAdmin" },
                    { 125L, "Create the new feature.", "Register Feature", "Button", 67L, "SystemAdmin" },
                    { 126L, "View the current 2FA status and available options.", "2FA Dashboard", "Card", 68L, "" },
                    { 127L, "Store these codes safely.", "Recovery Codes List", "List", 69L, "" },
                    { 128L, "Invalidate the current app keys.", "Reset Keys", "Button", 70L, "" },
                    { 129L, "Create a new set of backup codes.", "Generate Codes", "Button", 71L, "" },
                    { 130L, "Verify the app configuration.", "Enable App", "Button", 72L, "" },
                    { 131L, "Disable 2FA entirely.", "Disable 2FA", "Button", 73L, "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisGrants_MisDonorId",
                table: "MisGrants",
                column: "MisDonorId");

            migrationBuilder.CreateIndex(
                name: "IX_MisProposalPhases_MisProposalId",
                table: "MisProposalPhases",
                column: "MisProposalId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisGrants_MisDonors_MisDonorId",
                table: "MisGrants",
                column: "MisDonorId",
                principalTable: "MisDonors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisGrants_MisDonors_MisDonorId",
                table: "MisGrants");

            migrationBuilder.DropTable(
                name: "MisDonors");

            migrationBuilder.DropTable(
                name: "MisProposalPhases");

            migrationBuilder.DropIndex(
                name: "IX_MisGrants_MisDonorId",
                table: "MisGrants");

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 109L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 110L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 111L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 112L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 113L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 114L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 115L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 116L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 117L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 118L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 119L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 120L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 121L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 122L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 123L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 124L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 125L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 126L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 127L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 128L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 129L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 130L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 131L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 55L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 57L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 60L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 61L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 62L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 63L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 64L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 65L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 66L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 67L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 68L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 69L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 70L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 73L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DropColumn(
                name: "ExecutionDuration",
                table: "MisProposals");

            migrationBuilder.DropColumn(
                name: "ProjectEndDate",
                table: "MisProposals");

            migrationBuilder.DropColumn(
                name: "ProjectStartDate",
                table: "MisProposals");

            migrationBuilder.DropColumn(
                name: "MisDonorId",
                table: "MisGrants");

            migrationBuilder.AddColumn<string>(
                name: "DonorName",
                table: "MisGrants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
