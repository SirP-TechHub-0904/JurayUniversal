using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class participants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "ParticipantId",
            //    table: "ModeratorEvaluations",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ModeratorEvaluations_ParticipantId",
            //    table: "ModeratorEvaluations",
            //    column: "ParticipantId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ModeratorEvaluations_ParticipantLists_ParticipantId",
            //    table: "ModeratorEvaluations",
            //    column: "ParticipantId",
            //    principalTable: "ParticipantLists",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ModeratorEvaluations_ParticipantLists_ParticipantId",
            //    table: "ModeratorEvaluations");

            //migrationBuilder.DropIndex(
            //    name: "IX_ModeratorEvaluations_ParticipantId",
            //    table: "ModeratorEvaluations");

            //migrationBuilder.DropColumn(
            //    name: "ParticipantId",
            //    table: "ModeratorEvaluations");
        }
    }
}
