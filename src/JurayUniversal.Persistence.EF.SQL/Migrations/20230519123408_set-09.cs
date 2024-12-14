using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class set09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActivateOnlyAuthorizedDevice",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CompanyIconKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyIconUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsiteLink",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DashboardTitle",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "JustWebsite",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivateOnlyAuthorizedDevice",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CompanyIconKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CompanyIconUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CompanyLogoKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CompanyLogoUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CompanyWebsiteLink",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "DashboardTitle",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "JustWebsite",
                table: "SuperSettings");
        }
    }
}
