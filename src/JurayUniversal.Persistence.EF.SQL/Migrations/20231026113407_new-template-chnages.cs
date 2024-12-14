using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class newtemplatechnages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReloaderIconKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReloaderIconUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SpecialMenuButtonActivate",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpecialMenuButtonLink",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialMenuButtonText",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReloaderIconKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ReloaderIconUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "SpecialMenuButtonActivate",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "SpecialMenuButtonLink",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "SpecialMenuButtonText",
                table: "SuperSettings");
        }
    }
}
