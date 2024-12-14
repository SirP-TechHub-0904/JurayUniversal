using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class data2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginBackgroundImageKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginBackgroundImageUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginNoteTitle",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginTitle",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseNormalLogoInLogin",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseWhiteLogoInLogin",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginBackgroundImageKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LoginBackgroundImageUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LoginNoteTitle",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LoginTitle",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "UseNormalLogoInLogin",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "UseWhiteLogoInLogin",
                table: "SuperSettings");
        }
    }
}
