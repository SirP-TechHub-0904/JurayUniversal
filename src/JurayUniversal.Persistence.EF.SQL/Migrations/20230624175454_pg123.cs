using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class pg123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DirectUrl",
                table: "WebPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableDirectUrl",
                table: "WebPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomClass",
                table: "PageSections",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectUrl",
                table: "WebPages");

            migrationBuilder.DropColumn(
                name: "EnableDirectUrl",
                table: "WebPages");

            migrationBuilder.DropColumn(
                name: "CustomClass",
                table: "PageSections");
        }
    }
}
