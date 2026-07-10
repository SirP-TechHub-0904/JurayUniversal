using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveCaptcha",
                table: "MisSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GoogleRecaptchaSecretKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleRecaptchaSiteKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveCaptcha",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "GoogleRecaptchaSecretKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "GoogleRecaptchaSiteKey",
                table: "MisSettings");
        }
    }
}
