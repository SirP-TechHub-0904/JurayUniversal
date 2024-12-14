using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class ribonTask2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BreakingNewsRibonTitle",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RibonPosition",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RibonCustomDisplayTitle",
                table: "PageSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RibonSortOrder",
                table: "PageSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RibonCustomDisplayTitle",
                table: "PageSectionLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RibonSortOrder",
                table: "PageSectionLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RibonCustomDisplayTitle",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RibonSortOrder",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakingNewsRibonTitle",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RibonPosition",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RibonCustomDisplayTitle",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "RibonSortOrder",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "RibonCustomDisplayTitle",
                table: "PageSectionLists");

            migrationBuilder.DropColumn(
                name: "RibonSortOrder",
                table: "PageSectionLists");

            migrationBuilder.DropColumn(
                name: "RibonCustomDisplayTitle",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "RibonSortOrder",
                table: "Blogs");
        }
    }
}
