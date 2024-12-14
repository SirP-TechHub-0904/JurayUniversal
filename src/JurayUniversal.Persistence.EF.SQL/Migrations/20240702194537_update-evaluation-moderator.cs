using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class updateevaluationmoderator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                table: "ModeratorEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_Moderators_ModeratorId",
                table: "ModeratorEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                table: "ModeratorEvaluations");

            migrationBuilder.AlterColumn<long>(
                name: "TimeTableId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ParticipantListId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ModeratorId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "EvaluationQuestionId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                table: "ModeratorEvaluations",
                column: "EvaluationQuestionId",
                principalTable: "EvaluationQuestions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_Moderators_ModeratorId",
                table: "ModeratorEvaluations",
                column: "ModeratorId",
                principalTable: "Moderators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                table: "ModeratorEvaluations",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                table: "ModeratorEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_Moderators_ModeratorId",
                table: "ModeratorEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                table: "ModeratorEvaluations");

            migrationBuilder.AlterColumn<long>(
                name: "TimeTableId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ParticipantListId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModeratorId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "EvaluationQuestionId",
                table: "ModeratorEvaluations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                table: "ModeratorEvaluations",
                column: "EvaluationQuestionId",
                principalTable: "EvaluationQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_Moderators_ModeratorId",
                table: "ModeratorEvaluations",
                column: "ModeratorId",
                principalTable: "Moderators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                table: "ModeratorEvaluations",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
