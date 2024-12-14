using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class task02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectFiles_XProjectTasks_XProjectTaskId",
                table: "XProjectFiles");

            migrationBuilder.AlterColumn<long>(
                name: "XProjectTaskId",
                table: "XProjectFiles",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "XProjectId",
                table: "XProjectFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_XProjectFiles_XProjectId",
                table: "XProjectFiles",
                column: "XProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectFiles_XProjects_XProjectId",
                table: "XProjectFiles",
                column: "XProjectId",
                principalTable: "XProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectFiles_XProjectTasks_XProjectTaskId",
                table: "XProjectFiles",
                column: "XProjectTaskId",
                principalTable: "XProjectTasks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectFiles_XProjects_XProjectId",
                table: "XProjectFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectFiles_XProjectTasks_XProjectTaskId",
                table: "XProjectFiles");

            migrationBuilder.DropIndex(
                name: "IX_XProjectFiles_XProjectId",
                table: "XProjectFiles");

            migrationBuilder.DropColumn(
                name: "XProjectId",
                table: "XProjectFiles");

            migrationBuilder.AlterColumn<long>(
                name: "XProjectTaskId",
                table: "XProjectFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectFiles_XProjectTasks_XProjectTaskId",
                table: "XProjectFiles",
                column: "XProjectTaskId",
                principalTable: "XProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
