using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "RequiredRole" },
                values: new object[] { "Opens the registration form to manually onboard a new employee or member into the system.", "RecordCreator" });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "ElementName", "RequiredRole" },
                values: new object[] { "Navigates to the detailed tabular list of all users where you can filter, search, and perform bulk actions.", "Manage Users Button", "GlobalViewer" });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { 3L, "Allows you to slice the demographic and registration data by a specific date range.", "Filter Analytics Date Picker", "Form", 1L, "GlobalViewer" });

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PageDescription", "PageTitle" },
                values: new object[] { "The high-level command center for the User Management module. This page gives you demographic statistics, quick analytics on active/suspended accounts, and shortcuts to manage users.", "User Directory Dashboard" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 2L, 1L, "The comprehensive table view of all users in the organization. It supports deep filtering by status, category, and advanced search.", "Full User Roster", "/MisUserManagement/Dashboard/List" },
                    { 3L, 1L, "The detailed individual record for a specific user. This is where administrators manage a user's specific access rights, private HR notes, and performance scores.", "User 360 Profile (Admin View)", "/MisUserManagement/Dashboard/Profile" },
                    { 4L, 1L, "The self-service portal where a regular user can view their own information, update their password, and check their performance scores.", "My Personal Details", "/MisUserManagement/Platform/MyProfile" }
                });

            migrationBuilder.UpdateData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Overview", "Title" },
                values: new object[] { "This module controls the entire platform's user base. It allows you to register members, assign them to categories, monitor their Key Performance Indicators (KPIs), and tightly control what features they can access via granular roles.", "User Management & Directory Guide" });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 4L, "Allows you to modify the user's basic profile information, contact details, and category.", "Edit User Icon", "Button", 2L, "GlobalEditor" },
                    { 5L, "Immediately revokes or restores a user's ability to log into the platform.", "Suspend/Activate Account", "Toggle", 2L, "SystemAdmin" },
                    { 6L, "Opens the comprehensive 360-degree view of the user, including their assigned assets, roles, and KPIs.", "View Profile Details", "Button", 2L, "GlobalViewer" },
                    { 7L, "Crucial security function: Grants this specific user access to a platform module (like Ticketing or AI Studio) with a specific permission level (e.g. GlobalViewer or SystemAdmin).", "Assign Feature Role", "Form", 3L, "SystemAdmin" },
                    { 8L, "Allows administrators to attach internal notes to the user's profile. Checking 'Is Private' ensures the user cannot see this note.", "Add HR Note", "Form", 3L, "RecordManager" },
                    { 9L, "Records a performance evaluation score for the user for a specific period.", "Update KPI Score", "Form", 3L, "GlobalEditor" },
                    { 10L, "Redirects you to the security settings to update your login credentials and enable 2-Factor Authentication.", "Change Password", "Button", 4L, "" },
                    { 11L, "Allows you to see your historical performance scores and reviews entered by your managers.", "View My KPIs", "Tab", 4L, "" },
                    { 12L, "Logs you securely out of the platform across all your devices.", "Log Out", "Button", 4L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "RequiredRole" },
                values: new object[] { "Click this button to register a new user in the system.", "GlobalEditor" });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "ElementName", "RequiredRole" },
                values: new object[] { "Allows you to suspend or activate a user account.", "User Status Toggle", "SystemAdmin" });

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PageDescription", "PageTitle" },
                values: new object[] { "The main directory of all users in the system.", "User Directory" });

            migrationBuilder.UpdateData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Overview", "Title" },
                values: new object[] { "Manage all platform users, roles, and KPIs here.", "User Management Guide" });
        }
    }
}
