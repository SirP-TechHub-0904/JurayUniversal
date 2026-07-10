using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 66L, "Use the left and right arrows to navigate between months. The workspace will reload and display the analytics and calendar for the selected month.", "Month Navigation Arrows", "Button", 40L, "" },
                    { 67L, "If a report is due today and the deadline has not passed, this banner will appear. Click 'Submit Today's Report' to immediately open the submission form.", "Due Today Alert Banner", "Card", 40L, "" },
                    { 68L, "Shows the total number of reports submitted on time for the selected month. A high On-Time count reflects strong compliance.", "On-Time KPI Card", "Card", 40L, "" },
                    { 69L, "Shows reports that were submitted after the daily deadline. Late submissions are still recorded but carry a warning flag in your compliance record.", "Late KPI Card", "Card", 40L, "" },
                    { 70L, "Shows the count of due dates where no report was submitted at all. Missed reports negatively impact your overall compliance rate.", "Missed KPI Card", "Card", 40L, "" },
                    { 71L, "Your overall submission compliance for the month. It is calculated as (On-Time + Late) / Total Due Days x 100. A 100% rate means you never missed a deadline.", "Compliance Rate & Progress Bar", "Chart", 40L, "" },
                    { 72L, "A visual doughnut chart showing the proportion of On-Time (green), Late (orange), and Missed (red) reports for the selected month.", "Month Performance Doughnut Chart", "Chart", 40L, "" },
                    { 73L, "Click 'Calendar View' to see a full monthly grid. Click 'List View' to see a table of all due dates. Your preference is saved automatically in your browser so it persists across sessions.", "Calendar / List View Toggle", "Toggle", 40L, "" },
                    { 74L, "Each cell represents a day. Green cells (✓) are On-Time submissions. Orange cells (!) are Late submissions. Red cells (✗) are Missed deadlines. Blue dashed cells are pending today. White cells are non-due days.", "Color-Coded Calendar Grid", "Card", 40L, "" },
                    { 75L, "Click on any green or orange calendar cell to jump directly to the submission detail page for that day's report.", "Calendar Cell Clickable Links", "Button", 40L, "" },
                    { 76L, "Shows the full narrative report content as submitted by the user for that specific reporting period.", "Report Content Body", "Card", 41L, "" },
                    { 77L, "If the report schema required hourly logs, this table displays every time slot with the submitted activity description. Break slots are shown as greyed-out rows.", "Hourly Time Log Table", "Table", 41L, "" },
                    { 78L, "Displays the listed accomplishments and achievements the user reported for that day.", "Achievements Section", "Card", 41L, "" },
                    { 79L, "Displays any issues, blockers, or challenges the user reported for that day.", "Challenges Section", "Card", 41L, "" },
                    { 80L, "Shows the clickable attachment if a file was uploaded with this report submission.", "File Attachment", "Button", 41L, "" },
                    { 81L, "Modify the main text content of your report. Changes can only be saved before the daily submission deadline.", "Edit Report Content", "Form", 42L, "" },
                    { 82L, "Update the activity descriptions for each hourly time slot. Break rows are locked and cannot be edited.", "Edit Time Log Entries", "Form", 42L, "" },
                    { 83L, "Save all changes. The system will re-evaluate whether the updated submission is still On-Time or Late based on the current server time.", "Save Changes Button", "Button", 42L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 66L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 67L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 68L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 69L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 70L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 73L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 74L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 75L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 76L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 77L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 78L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 80L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 81L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 82L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 83L);
        }
    }
}
