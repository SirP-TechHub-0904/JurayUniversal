using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class task01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectMessages_AspNetUsers_ReceiverId",
                table: "XProjectMessages");

            migrationBuilder.DropColumn(
                name: "Files",
                table: "XProjectTasks");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "XProjectMessages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectMessages_AspNetUsers_ReceiverId",
                table: "XProjectMessages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectMessages_AspNetUsers_ReceiverId",
                table: "XProjectMessages");

            migrationBuilder.AddColumn<string>(
                name: "Files",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "XProjectMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectMessages_AspNetUsers_ReceiverId",
                table: "XProjectMessages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
