using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class port029 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortfolioBreacrumImageKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioBreacrumImageUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioDescription",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioImageOneKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioImageOneUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioImageTwoKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioImageTwoUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioMiniTitle",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioTitle",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortfolioBreacrumImageKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioBreacrumImageUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioDescription",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioImageOneKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioImageOneUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioImageTwoKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioImageTwoUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioMiniTitle",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PortfolioTitle",
                table: "SuperSettings");
        }
    }
}
