using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RouteKey",
                value: "/MisUserManagement/Dashboard/Detail");

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 5L, 1L, "Manage the various categories, departments, or cohorts that users can be grouped into.", "Category Directory", "/MisUserManagement/Dashboard/Categories/Index" },
                    { 6L, 1L, "Define the core metrics and criteria by which users in the organization are evaluated.", "KPI Indicators Settings", "/MisUserManagement/Dashboard/KpiSettings" },
                    { 7L, 1L, "The evaluation panel where managers rate an individual user's performance against the established KPIs for a specific period.", "KPI Evaluation Panel", "/MisUserManagement/Dashboard/KpiAssign" },
                    { 8L, 1L, "A competitive or analytical view ranking the overall performance scores of all evaluated users during a specific period.", "KPI Leaderboard", "/MisUserManagement/Dashboard/KpiResults" },
                    { 9L, 1L, "Your personal performance dashboard. Review your evaluations, scores, and manager remarks over time.", "My KPI Scores", "/MisUserManagement/Platform/MyKpis" },
                    { 10L, 1L, "The security control center for a user. Strictly control what modules they can access and their exact permission level.", "Manage Access Permissions", "/MisUserManagement/Dashboard/Roles" },
                    { 11L, 1L, "A transparent, read-only view of your active access rights and permission levels across all platform modules.", "My Permissions Matrix", "/MisUserManagement/Platform/MyRoles" },
                    { 12L, 1L, "Keep your account secure by changing your password regularly and enabling Two-Factor Authentication (2FA).", "Security Settings & 2FA", "/MisUserManagement/Platform/ChangePassword" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 13L, "Creates a new category grouping that users can be assigned to.", "Add New Category", "Button", 5L, "GlobalEditor" },
                    { 14L, "Quickly find a category by its title or description.", "Search Category", "Input", 5L, "GlobalViewer" },
                    { 15L, "Define a new performance metric (e.g., 'Punctuality', 'Sales Targets') and its maximum possible score.", "Create Indicator", "Button", 6L, "SystemAdmin" },
                    { 16L, "Modify or remove an existing performance indicator.", "Manage Indicators", "Action", 6L, "SystemAdmin" },
                    { 17L, "Input the numerical score achieved by the user for each active KPI.", "Score Inputs", "Form", 7L, "GlobalEditor" },
                    { 18L, "Save the evaluation to the user's permanent performance record.", "Submit Evaluation", "Button", 7L, "GlobalEditor" },
                    { 19L, "Filter the leaderboard to view rankings for a specific month, quarter, or year.", "Select Evaluation Period", "Dropdown", 8L, "GlobalViewer" },
                    { 20L, "Displays the cumulative score for each user alongside manager remarks.", "Leaderboard Table", "Table", 8L, "GlobalViewer" },
                    { 21L, "A historical list of every performance review you have received since joining.", "Evaluation History", "List", 9L, "" },
                    { 22L, "Select a platform module (e.g. Asset Management) and assign a permission level (e.g. GlobalViewer, RecordCreator, SystemAdmin).", "Grant Permission", "Form", 10L, "SystemAdmin" },
                    { 23L, "Instantly strip this user's access to a specific module.", "Revoke Role", "Button", 10L, "SystemAdmin" },
                    { 24L, "Displays a grid of every feature you can access and exactly what actions you are authorized to take.", "Permissions Matrix", "Table", 11L, "" },
                    { 25L, "Change your login password. You must know your current password to set a new one.", "Update Password", "Form", 12L, "" },
                    { 26L, "Scan the QR code with Google Authenticator or Authy to add a secondary security layer to your account.", "Setup 2FA", "QR Code", 12L, "" },
                    { 27L, "Enter the 6-digit code from your app to finalize the 2FA setup.", "Verify & Enable", "Button", 12L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RouteKey",
                value: "/MisUserManagement/Dashboard/Profile");
        }
    }
}
