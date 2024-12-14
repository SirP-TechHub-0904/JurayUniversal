using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class jscript9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JSConfiguration",
                table: "SuperSettings");

            migrationBuilder.AddColumn<bool>(
                name: "RedirectTohttps",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RedirectTohttpswww",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectTohttps",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "RedirectTohttpswww",
                table: "SuperSettings");

            migrationBuilder.AddColumn<string>(
                name: "JSConfiguration",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
