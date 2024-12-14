using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class evaluation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeTableId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorEvaluations_TimeTableId",
                table: "ModeratorEvaluations",
                column: "TimeTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                table: "ModeratorEvaluations",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                table: "ModeratorEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_ModeratorEvaluations_TimeTableId",
                table: "ModeratorEvaluations");

            migrationBuilder.DropColumn(
                name: "TimeTableId",
                table: "ModeratorEvaluations");
        }
    }
}
