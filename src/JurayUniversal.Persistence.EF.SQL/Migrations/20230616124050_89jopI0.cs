using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class _89jopI0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddBlogToFooter",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddBlogToHome",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddBlogToMenu",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BlogDisplayTitle",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Publish",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddBlogToFooter",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AddBlogToHome",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AddBlogToMenu",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BlogDisplayTitle",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Publish",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Blogs");
        }
    }
}
