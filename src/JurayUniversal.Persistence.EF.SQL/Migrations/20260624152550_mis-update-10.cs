using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "ElementName", "ElementType", "RequiredRole" },
                values: new object[] { "Click on any of the metric cards (Total, Active, Pending, Suspended) to instantly filter the user directory for that specific status.", "User Status Metrics", "Card", "GlobalViewer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "ElementName", "ElementType", "RequiredRole" },
                values: new object[] { "Opens the registration form to manually onboard a new employee or member into the system.", "Add User Button", "Button", "RecordCreator" });
        }
    }
}
