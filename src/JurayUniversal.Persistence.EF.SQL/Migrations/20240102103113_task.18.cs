using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class task18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectSections_XProjects_XProjectId",
                table: "XProjectSections");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_XProjectSections_XProjectSectionId",
                table: "XProjectTasks");

            migrationBuilder.AlterColumn<long>(
                name: "XProjectSectionId",
                table: "XProjectTasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "XProjectId",
                table: "XProjectSections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectSections_XProjects_XProjectId",
                table: "XProjectSections",
                column: "XProjectId",
                principalTable: "XProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_XProjectSections_XProjectSectionId",
                table: "XProjectTasks",
                column: "XProjectSectionId",
                principalTable: "XProjectSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XProjectSections_XProjects_XProjectId",
                table: "XProjectSections");

            migrationBuilder.DropForeignKey(
                name: "FK_XProjectTasks_XProjectSections_XProjectSectionId",
                table: "XProjectTasks");

            migrationBuilder.AlterColumn<long>(
                name: "XProjectSectionId",
                table: "XProjectTasks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "XProjectId",
                table: "XProjectSections",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectSections_XProjects_XProjectId",
                table: "XProjectSections",
                column: "XProjectId",
                principalTable: "XProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_XProjectTasks_XProjectSections_XProjectSectionId",
                table: "XProjectTasks",
                column: "XProjectSectionId",
                principalTable: "XProjectSections",
                principalColumn: "Id");
        }
    }
}
