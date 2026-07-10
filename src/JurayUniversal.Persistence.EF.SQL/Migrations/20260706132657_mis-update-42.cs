using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentProvider",
                table: "MisSmsTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MisSmsTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MisSmsTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MisWidgets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WidgetCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ColSpan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisWidgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisWizardSteps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    TargetElement = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisWizardSteps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MisWidgets",
                columns: new[] { "Id", "ColSpan", "SortOrder", "WidgetCode" },
                values: new object[,]
                {
                    { 1L, 12, 1, "welcome_wizard" },
                    { 2L, 12, 2, "system_health_hub" },
                    { 3L, 12, 3, "applications_hub" },
                    { 4L, 8, 4, "financials_chart" },
                    { 5L, 4, 5, "recent_visited" }
                });

            migrationBuilder.InsertData(
                table: "MisWizardSteps",
                columns: new[] { "Id", "Content", "FeatureName", "StepOrder", "TargetElement", "Title" },
                values: new object[,]
                {
                    { 1L, "This is your main dashboard where you manage all platform modules.", "MIS", 1, "#widget-welcome_wizard", "Welcome to MIS Console" },
                    { 2L, "Click here to add new charts, lists, and metrics to your dashboard.", "MIS", 2, "#addWidgetBtn", "Add Custom Widgets" },
                    { 3L, "Toggle edit mode to drag, resize, and remove widgets.", "MIS", 3, ".btn-outline-secondary", "Rearrange Layout" },
                    { 4L, "Manage all your staff, members, and custom KPIs from this central directory.", "MisUserManagement", 1, ".page-header", "User Directory" },
                    { 5L, "Click here to invite or manually add a new user to the platform.", "MisUserManagement", 2, "#btn-add-user", "Add New User" },
                    { 6L, "View, filter, and manage permissions for existing users.", "MisUserManagement", 3, "#users-table", "User List" },
                    { 7L, "Create and manage concerts, conferences, and paid events.", "MisTicketing", 1, ".page-header", "Event Ticketing" },
                    { 8L, "Setup a new event, configure ticket limits, and pricing.", "MisTicketing", 2, "#btn-create-event", "Create Event" },
                    { 9L, "Track subscriptions, recurring fees, and member dues.", "MisDues", 1, ".page-header", "Financials" },
                    { 10L, "Send automated payment reminders to members.", "MisDues", 2, "#btn-new-invoice", "Generate Invoices" },
                    { 11L, "Manage private workspaces and specialized teams.", "MisCommittees", 1, ".page-header", "Committees" },
                    { 12L, "Start a new collaboration group and invite members.", "MisCommittees", 2, "#btn-add-group", "Create Workspace" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisSmsTransactions_UserId",
                table: "MisSmsTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisSmsTransactions_AspNetUsers_UserId",
                table: "MisSmsTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisSmsTransactions_AspNetUsers_UserId",
                table: "MisSmsTransactions");

            migrationBuilder.DropTable(
                name: "MisWidgets");

            migrationBuilder.DropTable(
                name: "MisWizardSteps");

            migrationBuilder.DropIndex(
                name: "IX_MisSmsTransactions_UserId",
                table: "MisSmsTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentProvider",
                table: "MisSmsTransactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MisSmsTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MisSmsTransactions");
        }
    }
}
