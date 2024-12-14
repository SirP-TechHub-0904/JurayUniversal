using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class ribonTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BucketName",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableBreakingNewsRibon",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddToRibon",
                table: "PageSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddToRibon",
                table: "PageSectionLists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddToRibon",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BucketName",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "EnableBreakingNewsRibon",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AddToRibon",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "AddToRibon",
                table: "PageSectionLists");

            migrationBuilder.DropColumn(
                name: "AddToRibon",
                table: "Blogs");
        }
    }
}
