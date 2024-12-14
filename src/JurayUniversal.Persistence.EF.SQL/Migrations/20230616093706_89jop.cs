using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class _89jop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivateServicesInHome",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowSixService",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "ShowThreeService",
                table: "Settings",
                newName: "ShowTwoProducts");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitleHome",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ButtonText",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DisableAmount",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MiniTitle",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowSamples",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StatusNote",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IconText",
                table: "ProductFeatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductTitleHome",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ButtonText",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DisableAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MiniTitle",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShowSamples",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatusNote",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IconText",
                table: "ProductFeatures");

            migrationBuilder.RenameColumn(
                name: "ShowTwoProducts",
                table: "Settings",
                newName: "ShowThreeService");

            migrationBuilder.AddColumn<bool>(
                name: "ActivateServicesInHome",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowSixService",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
