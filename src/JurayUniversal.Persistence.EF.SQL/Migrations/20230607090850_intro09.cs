using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class intro09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FixedAfterFooter",
                table: "PageSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Qoute",
                table: "PageSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondImageKey",
                table: "PageSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondImageUrl",
                table: "PageSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonLink",
                table: "PageSectionLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonText",
                table: "PageSectionLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiniTitle",
                table: "PageSectionLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedAfterFooter",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "Qoute",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "SecondImageKey",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "SecondImageUrl",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "ButtonLink",
                table: "PageSectionLists");

            migrationBuilder.DropColumn(
                name: "ButtonText",
                table: "PageSectionLists");

            migrationBuilder.DropColumn(
                name: "MiniTitle",
                table: "PageSectionLists");
        }
    }
}
