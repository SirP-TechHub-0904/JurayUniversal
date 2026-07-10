using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnablePaystack",
                table: "MisSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePixelPay",
                table: "MisSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaystackPublicKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaystackSecretKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaystackWebhookSecret",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentProvider",
                table: "MisFinanceTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnablePaystack",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "EnablePixelPay",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PaystackPublicKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PaystackSecretKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PaystackWebhookSecret",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "PaymentProvider",
                table: "MisFinanceTransactions");
        }
    }
}
