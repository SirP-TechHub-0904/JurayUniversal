using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakEndTime",
                table: "MisReportSchemas",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakStartTime",
                table: "MisReportSchemas",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeLogIntervalHours",
                table: "MisReportSchemas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Use24HourFormat",
                table: "MisReportSchemas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkEndTime",
                table: "MisReportSchemas",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkStartTime",
                table: "MisReportSchemas",
                type: "time",
                nullable: true);

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { 58L, "A powerful shortcut to instantly assign a newly created report schema to every single active user in the organization.", "Assign to All Active Users", "Toggle", 29L, "GlobalEditor" });

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 33L,
                column: "PageDescription",
                value: "The dynamic form where you write and file your required report before the deadline. The fields you see are strictly defined by the Admin's template flags.");

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 34L, 5L, "The master list of all custom forms and surveys. You can track whether they are currently published or in draft mode.", "Forms Directory", "/MisForms/Dashboard/Index" },
                    { 35L, 5L, "Initial setup page to define a new form's basic details like title, description, and whether it requires a user login.", "Create Form", "/MisForms/Dashboard/Create" },
                    { 36L, 5L, "The visual Drag-and-Drop Form Builder interface. Drag elements from the panel to construct the exact data schema you want to collect.", "Drag-and-Drop Builder", "/MisForms/Dashboard/Builder" },
                    { 37L, 5L, "A dynamically generated table that unpacks user JSON responses and displays them in a readable format matching your custom form labels.", "Form Responses Ledger", "/MisForms/Dashboard/Responses" },
                    { 38L, 5L, "The user-facing dashboard displaying all active, published forms they are eligible to fill out.", "Available Forms", "/MisForms/Platform/Index" },
                    { 39L, 5L, "The dynamic form-filling interface. It instantly translates the Admin's drag-and-drop schema into a functional HTML form.", "Submit Form", "/MisForms/Platform/Submit" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 59L, "Creates a new form definition that you can subsequently build out using the drag-and-drop interface.", "New Form", "Button", 34L, "GlobalEditor" },
                    { 60L, "Draggable input fields like Textboxes, Checkboxes, and Date Pickers that you can drop onto the canvas.", "Builder Toolbar", "Panel", 36L, "GlobalEditor" },
                    { 61L, "Generates the structural JSON payload and saves the entire layout to the database.", "Save Form Layout", "Button", 36L, "GlobalEditor" },
                    { 62L, "Makes the form immediately available to the public or logged-in users depending on your security settings.", "Publish Form", "Button", 36L, "GlobalEditor" },
                    { 63L, "Dynamically unpacks the complex JSON responses into clear, readable column headers matching your custom labels.", "Dynamic Response Table", "Table", 37L, "GlobalViewer" },
                    { 64L, "Select a form from this dashboard to enter the dynamic submission interface.", "Fill Form", "Button", 38L, "" },
                    { 65L, "Automatically packages your dynamic form inputs into a secure JSON string and saves it to the database.", "Submit JSON Payload", "Button", 39L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 60L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 61L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 62L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 63L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 64L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 65L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DropColumn(
                name: "BreakEndTime",
                table: "MisReportSchemas");

            migrationBuilder.DropColumn(
                name: "BreakStartTime",
                table: "MisReportSchemas");

            migrationBuilder.DropColumn(
                name: "TimeLogIntervalHours",
                table: "MisReportSchemas");

            migrationBuilder.DropColumn(
                name: "Use24HourFormat",
                table: "MisReportSchemas");

            migrationBuilder.DropColumn(
                name: "WorkEndTime",
                table: "MisReportSchemas");

            migrationBuilder.DropColumn(
                name: "WorkStartTime",
                table: "MisReportSchemas");

            migrationBuilder.UpdateData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 33L,
                column: "PageDescription",
                value: "The dynamic form where you write and submit your required report before the deadline. The fields you see are strictly defined by the Admin's template flags.");
        }
    }
}
