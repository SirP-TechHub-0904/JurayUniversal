using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class formadjust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UpdateAwards",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateCertificate",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateEducation",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateExperience",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateInterest",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateLanguage",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateReference",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateSkills",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateAwards",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateCertificate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateEducation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateExperience",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateInterest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateLanguage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateReference",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateSkills",
                table: "AspNetUsers");
        }
    }
}
