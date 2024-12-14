using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class port02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccountUpgraded",
                table: "ProfilePortfolioSetting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseUpgradeWebLink",
                table: "ProfilePortfolioSetting",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountUpgraded",
                table: "ProfilePortfolioSetting");

            migrationBuilder.DropColumn(
                name: "UseUpgradeWebLink",
                table: "ProfilePortfolioSetting");
        }
    }
}
