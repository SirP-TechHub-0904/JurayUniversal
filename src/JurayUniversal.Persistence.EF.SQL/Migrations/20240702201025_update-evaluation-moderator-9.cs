using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class updateevaluationmoderator9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorEvaluations_ParticipantLists_ParticipantListId1",
                table: "ModeratorEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_ModeratorEvaluations_ParticipantListId1",
                table: "ModeratorEvaluations");

            migrationBuilder.DropColumn(
                name: "ParticipantListId1",
                table: "ModeratorEvaluations");

            migrationBuilder.RenameColumn(
                name: "ParticipantListId",
                table: "ModeratorEvaluations",
                newName: "ParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParticipantId",
                table: "ModeratorEvaluations",
                newName: "ParticipantListId");

            migrationBuilder.AddColumn<int>(
                name: "ParticipantListId1",
                table: "ModeratorEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorEvaluations_ParticipantListId1",
                table: "ModeratorEvaluations",
                column: "ParticipantListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorEvaluations_ParticipantLists_ParticipantListId1",
                table: "ModeratorEvaluations",
                column: "ParticipantListId1",
                principalTable: "ParticipantLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
