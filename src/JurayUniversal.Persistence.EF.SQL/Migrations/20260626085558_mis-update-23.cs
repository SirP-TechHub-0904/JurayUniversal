using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowMultipleSubmissions",
                table: "MisForms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowUserToViewResponses",
                table: "MisForms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 103L, "Enable this switch to restrict form submission only to registered, logged-in users. When enabled, their user profile is permanently linked to their response. If disabled, the form is public to anyone with the link.", "Require User Login Toggle", "Setting", 35L, "GlobalEditor" },
                    { 104L, "Enable this switch to allow users to submit this form/survey multiple times. If disabled, a user can only file one response; subsequent attempts will show a message that they have already submitted.", "Allow Multiple Submissions Toggle", "Setting", 35L, "GlobalEditor" },
                    { 105L, "Enable this switch to allow logged-in users to access their past submissions. Once enabled, users will see a button to browse their submitted responses.", "Allow User to View Responses Toggle", "Setting", 35L, "GlobalEditor" },
                    { 106L, "If you have already submitted a response and the administrator has enabled response viewing, this button will appear. Click it to view a summary of your past answers.", "My Responses Link", "Button", 38L, "" },
                    { 107L, "Appears when you have already submitted a response to a single-submission form. The form is locked and cannot be filled out again.", "Completed Badge", "Badge", 38L, "" }
                });

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 35L,
                column: "PageDescription",
                value: "Initial setup page to define a new form's basic details and configure settings such as user login requirements, multiple submission limits, and user response view permissions.");

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 47L, 5L, "Your personal portal to view all your previous submissions for a specific form/survey, provided the administrator enabled response viewing.", "My Submissions / Responses", "/MisForms/Platform/MyResponses" });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { 108L, "A clear grid table showing the timestamp of each of your submissions along with the answers you provided for each question.", "Submitted Responses List", "Table", 47L, "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 106L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 107L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 108L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DropColumn(
                name: "AllowMultipleSubmissions",
                table: "MisForms");

            migrationBuilder.DropColumn(
                name: "AllowUserToViewResponses",
                table: "MisForms");

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 35L,
                column: "PageDescription",
                value: "Initial setup page to define a new form's basic details like title, description, and whether it requires a user login.");
        }
    }
}
