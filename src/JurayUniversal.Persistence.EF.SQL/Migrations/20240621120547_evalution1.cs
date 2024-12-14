using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class evalution1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvaluationQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbbreviatedQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeratorEvaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeratorId = table.Column<long>(type: "bigint", nullable: false),
                    ParticipantListId = table.Column<long>(type: "bigint", nullable: false),
                    ParticipantListId1 = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<int>(type: "int", nullable: false),
                    EvaluationQuestionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeratorEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                        column: x => x.EvaluationQuestionId,
                        principalTable: "EvaluationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_Moderators_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Moderators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_ParticipantLists_ParticipantListId1",
                        column: x => x.ParticipantListId1,
                        principalTable: "ParticipantLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorEvaluations_EvaluationQuestionId",
                table: "ModeratorEvaluations",
                column: "EvaluationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorEvaluations_ModeratorId",
                table: "ModeratorEvaluations",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorEvaluations_ParticipantListId1",
                table: "ModeratorEvaluations",
                column: "ParticipantListId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModeratorEvaluations");

            migrationBuilder.DropTable(
                name: "EvaluationQuestions");
        }
    }
}
