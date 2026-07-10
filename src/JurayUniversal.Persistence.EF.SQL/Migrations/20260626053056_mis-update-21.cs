using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PixelPayBaseUrl",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PixelPayCurrency",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PixelPayKeyId",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PixelPaySecretKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PixelPaySignature",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PixelPayWebhookSecret",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonorEmail",
                table: "MisFinanceTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DonorName",
                table: "MisFinanceTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DonorPhone",
                table: "MisFinanceTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MisFinanceCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PixelPayBaseUrl",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PixelPayCurrency",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PixelPayKeyId",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PixelPaySecretKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PixelPaySignature",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PixelPayWebhookSecret",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "DonorEmail",
                table: "MisFinanceTransactions");

            migrationBuilder.DropColumn(
                name: "DonorName",
                table: "MisFinanceTransactions");

            migrationBuilder.DropColumn(
                name: "DonorPhone",
                table: "MisFinanceTransactions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MisFinanceCategories");
        }
    }
}
