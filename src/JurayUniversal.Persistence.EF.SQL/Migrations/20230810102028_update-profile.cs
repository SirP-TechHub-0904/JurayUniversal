using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class updateprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "AspNetUsers",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "PermanentHomeAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentLga",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentState",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentHomeAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentLga",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentState",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AspNetUsers",
                newName: "PostalCode");
        }
    }
}
