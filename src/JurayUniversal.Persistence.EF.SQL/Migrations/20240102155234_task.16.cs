using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class task16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_ApprovedById",
                table: "XProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_TestedById",
                table: "XProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_UserId",
                table: "XProjectTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "XProjectTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "TestedById",
                table: "XProjectTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedById",
                table: "XProjectTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_ApprovedById",
                table: "XProjectTasks",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_TestedById",
                table: "XProjectTasks",
                column: "TestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_UserId",
                table: "XProjectTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_ApprovedById",
                table: "XProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_TestedById",
                table: "XProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_UserId",
                table: "XProjectTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "XProjectTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TestedById",
                table: "XProjectTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedById",
                table: "XProjectTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_ApprovedById",
                table: "XProjectTasks",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_TestedById",
                table: "XProjectTasks",
                column: "TestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_AspNetUsers_UserId",
                table: "XProjectTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
