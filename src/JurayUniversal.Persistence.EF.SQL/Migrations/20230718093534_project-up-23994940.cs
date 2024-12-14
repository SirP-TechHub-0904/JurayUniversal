using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class projectup23994940 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectPositions_ProjectPositionId",
                table: "ProjectUsers");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectPositionId",
                table: "ProjectUsers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectPositions_ProjectPositionId",
                table: "ProjectUsers",
                column: "ProjectPositionId",
                principalTable: "ProjectPositions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectPositions_ProjectPositionId",
                table: "ProjectUsers");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectPositionId",
                table: "ProjectUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectPositions_ProjectPositionId",
                table: "ProjectUsers",
                column: "ProjectPositionId",
                principalTable: "ProjectPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
