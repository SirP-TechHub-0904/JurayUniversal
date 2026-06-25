using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class MisReportsWorkspaceHelp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 40L, 4L, "Your dedicated workspace for this specific report schedule. View monthly analytics, check the visual calendar, and submit or edit your reports.", "Report Workspace", "/MisReports/Platform/SchemaDetails" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 41L, 4L, "Detailed view of your filed report submission, detailing times logs, achievements, challenges, and attachments.", "Report Details", "/MisReports/Platform/Details" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 42L, 4L, "Update your submitted report data and active time logs before the daily submission deadline cut-off.", "Edit Report Submission", "/MisReports/Platform/Edit" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 42L);
        }
    }
}
