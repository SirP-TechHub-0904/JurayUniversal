using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleCategory",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 74L,
                column: "Description",
                value: "Each cell represents a day. Green cells (✓) are On-Time submissions. Orange cells (!) are Late submissions. Red cells (✗) are Missed deadlines. Blue dashed cells are pending today. White cells are non-due days.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleCategory",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 74L,
                column: "Description",
                value: "Each cell represents a day. Green cells (?) are On-Time submissions. Orange cells (!) are Late submissions. Red cells (?) are Missed deadlines. Blue dashed cells are pending today. White cells are non-due days.");
        }
    }
}
