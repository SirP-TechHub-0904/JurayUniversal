using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 163L, "Takes you to the Collaborators page to assign team members.", "Team / Settings", "Button", 105L, "GlobalEditor" },
                    { 164L, "Opens a form to add a new Epic, User Story, Task, or Bug to the backlog.", "New Work Item", "Button", 106L, "GlobalEditor" },
                    { 165L, "Opens a form to define a new Sprint container.", "New Sprint", "Button", 106L, "GlobalEditor" },
                    { 172L, "Navigate back to the Sprint Planning Backlog view.", "Backlog & Planning", "Button", 107L, "GlobalViewer" },
                    { 173L, "Instantly add a new task directly to this active sprint.", "New Task", "Button", 107L, "GlobalEditor" }
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 201L, 16L, "Manage staff access and assign Scrum Master or Developer roles.", "Project Collaborators", "/MisProjects/Projects/Collaborators" },
                    { 202L, 16L, "Your personal dashboard showing projects you are collaborating on.", "My Project Tasks", "/MisProjects/User/Index" },
                    { 203L, 16L, "Form to initialize a new agile project container.", "Create Agile Project", "/MisProjects/Projects/Create" },
                    { 204L, 16L, "Form to add a new Epic, User Story, Task, or Bug to the backlog.", "Create Work Item", "/MisProjects/Backlog/CreateTask" },
                    { 205L, 16L, "Form to define the dates and goals for a new sprint.", "Create Sprint", "/MisProjects/Backlog/CreateSprint" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 166L, "Add a user from the directory to the project team.", "Add Team Member", "Form", 201L, "GlobalEditor" },
                    { 167L, "Remove a user's access to the project.", "Remove User", "Button", 201L, "GlobalEditor" },
                    { 168L, "Quick jump to the active Kanban board for your assigned project.", "Open Kanban Board", "Button", 202L, "" },
                    { 169L, "Save the new project details to the database.", "Create Project", "Button", 203L, "GlobalEditor" },
                    { 170L, "Save the new work item to the unassigned backlog.", "Add to Backlog", "Button", 204L, "GlobalEditor" },
                    { 171L, "Save the sprint definition to the project.", "Create Sprint", "Button", 205L, "GlobalEditor" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 163L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 164L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 165L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 166L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 167L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 168L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 169L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 170L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 171L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 172L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 173L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 201L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 202L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 203L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 204L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 205L);
        }
    }
}
