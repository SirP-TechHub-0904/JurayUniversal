using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 11L, "MisGrants", "Grant Ledger and external funding tracking.", "Grant Tracking Guide" });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 12L, "MisProposals", "Proposal Pipeline for internal pitches and external grant applications.", "Proposal Pipeline Guide" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 74L, 11L, "Analytics dashboard showing all awarded external grants.", "Grant Ledger", "/MisGrants/Dashboard/Index" },
                    { 75L, 11L, "View deep details, milestones, and financials for a specific grant.", "Grant Details", "/MisGrants/Dashboard/Details" },
                    { 76L, 11L, "Form to register or update the details of an awarded grant.", "Manage Grant", "/MisGrants/Dashboard/Manage" },
                    { 77L, 11L, "Directory of all external donors, sponsors, and funding agencies.", "Donors Directory", "/MisGrants/Dashboard/Donors/Index" },
                    { 78L, 11L, "Add or update the profile of a funding organization.", "Manage Donor", "/MisGrants/Dashboard/Donors/Manage" },
                    { 79L, 12L, "Overview of all active internal pitches and submitted grant proposals.", "Proposal Pipeline", "/MisProposals/Dashboard/Index" },
                    { 80L, 12L, "Detailed view of a proposal, including its execution phases and uploaded documents.", "Proposal Details", "/MisProposals/Dashboard/Details" },
                    { 81L, 12L, "Form for an Administrator to draft or edit a new proposal.", "Manage Proposal", "/MisProposals/Dashboard/Manage" },
                    { 82L, 12L, "Your personal dashboard showing proposals and pitches you have submitted.", "My Proposals", "/MisProposals/Platform/Index" },
                    { 83L, 12L, "View the current review status and phases of your pitched proposal.", "My Proposal Details", "/MisProposals/Platform/Details" },
                    { 84L, 12L, "Form for a regular user to submit a new pitch or proposal to management.", "Submit Proposal", "/MisProposals/Platform/Submit" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 132L, "Export the grant ledger to CSV.", "Export Ledger", "Button", 74L, "GlobalViewer" },
                    { 133L, "View or update detailed grant info.", "Manage Grant", "Button", 75L, "GlobalEditor" },
                    { 134L, "Register a new external donor.", "New Donor", "Button", 77L, "GlobalEditor" },
                    { 135L, "Approve a submitted proposal to move it to the execution phase.", "Approve Proposal", "Button", 80L, "SystemAdmin" },
                    { 136L, "Reject a submitted proposal.", "Reject Proposal", "Button", 80L, "SystemAdmin" },
                    { 137L, "Save changes to the proposal draft.", "Save Draft", "Button", 81L, "GlobalEditor" },
                    { 138L, "Submit a new pitch to management.", "Submit Pitch", "Button", 84L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 132L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 133L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 134L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 135L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 136L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 137L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 138L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 76L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 78L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 82L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 83L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 74L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 75L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 77L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 80L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 81L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 84L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 12L);
        }
    }
}
