using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class office30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeActivityDialies_AspNetUsers_LastUpdateById",
                table: "OfficeActivityDialies");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateCount",
                table: "OfficeActivityDialies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Logs",
                table: "OfficeActivityDialies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastUpdateById",
                table: "OfficeActivityDialies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Customer",
                table: "OfficeActivityDialies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeActivityDialies_AspNetUsers_LastUpdateById",
                table: "OfficeActivityDialies",
                column: "LastUpdateById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeActivityDialies_AspNetUsers_LastUpdateById",
                table: "OfficeActivityDialies");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "OfficeActivityDialies");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateCount",
                table: "OfficeActivityDialies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Logs",
                table: "OfficeActivityDialies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastUpdateById",
                table: "OfficeActivityDialies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeActivityDialies_AspNetUsers_LastUpdateById",
                table: "OfficeActivityDialies",
                column: "LastUpdateById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
