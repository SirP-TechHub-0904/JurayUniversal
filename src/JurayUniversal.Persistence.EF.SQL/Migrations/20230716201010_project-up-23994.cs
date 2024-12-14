using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class projectup23994 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectMainId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_ProjectMainId",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "ProjectMainId",
                table: "ProjectUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountSpent",
                table: "ProjectMains",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "ProjectMains",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "FinancialReport",
                table: "ProjectMains",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountSpent",
                table: "ProjectMains");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "ProjectMains");

            migrationBuilder.DropColumn(
                name: "FinancialReport",
                table: "ProjectMains");

            migrationBuilder.AddColumn<long>(
                name: "ProjectMainId",
                table: "ProjectUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectMainId",
                table: "ProjectUsers",
                column: "ProjectMainId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectMainId",
                table: "ProjectUsers",
                column: "ProjectMainId",
                principalTable: "ProjectMains",
                principalColumn: "Id");
        }
    }
}
