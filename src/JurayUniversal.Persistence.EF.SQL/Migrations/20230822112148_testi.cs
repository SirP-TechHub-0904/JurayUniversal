using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class testi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Testimonies",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Testimonies",
                newName: "SortOrder");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Testimonies",
                newName: "Show");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Testimonies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Testimonies");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Testimonies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "Testimonies",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Show",
                table: "Testimonies",
                newName: "Content");
        }
    }
}
