using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class introresolveevaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModeratorEvaluations");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ParticipantLists_TempId",
            //    table: "ParticipantLists");

            //migrationBuilder.DropColumn(
            //    name: "TempId",
            //    table: "ParticipantLists");

            migrationBuilder.CreateTable(
                name: "AccountModeratorEvaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeratorId = table.Column<long>(type: "bigint", nullable: true),
                    TimeTableId = table.Column<long>(type: "bigint", nullable: true),
                    ParticipantListId = table.Column<int>(type: "int", nullable: true),
                    Response = table.Column<int>(type: "int", nullable: false),
                    EvaluationQuestionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountModeratorEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                        column: x => x.EvaluationQuestionId,
                        principalTable: "EvaluationQuestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountModeratorEvaluations_Moderators_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Moderators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountModeratorEvaluations_ParticipantLists_ParticipantListId",
                        column: x => x.ParticipantListId,
                        principalTable: "ParticipantLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountModeratorEvaluations_TimeTables_TimeTableId",
                        column: x => x.TimeTableId,
                        principalTable: "TimeTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountModeratorEvaluations_EvaluationQuestionId",
                table: "AccountModeratorEvaluations",
                column: "EvaluationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountModeratorEvaluations_ModeratorId",
                table: "AccountModeratorEvaluations",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountModeratorEvaluations_ParticipantListId",
                table: "AccountModeratorEvaluations",
                column: "ParticipantListId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountModeratorEvaluations_TimeTableId",
                table: "AccountModeratorEvaluations",
                column: "TimeTableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountModeratorEvaluations");

            //migrationBuilder.AddColumn<long>(
            //    name: "TempId",
            //    table: "ParticipantLists",
            //    type: "bigint",
            //    nullable: false,
            //    defaultValue: 0L);

            //migrationBuilder.AddUniqueConstraint(
            //    name: "AK_ParticipantLists_TempId",
            //    table: "ParticipantLists",
            //    column: "TempId");

            migrationBuilder.CreateTable(
                name: "ModeratorEvaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationQuestionId = table.Column<long>(type: "bigint", nullable: true),
                    ModeratorId = table.Column<long>(type: "bigint", nullable: true),
                    ParticipantId = table.Column<long>(type: "bigint", nullable: true),
                    TimeTableId = table.Column<long>(type: "bigint", nullable: true),
                    Response = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeratorEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_EvaluationQuestions_EvaluationQuestionId",
                        column: x => x.EvaluationQuestionId,
                        principalTable: "EvaluationQuestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_Moderators_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Moderators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_ParticipantLists_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "ParticipantLists",
                        principalColumn: "TempId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeratorEvaluations_TimeTables_TimeTableId",
                        column: x => x.TimeTableId,
                        principalTable: "TimeTables",
                        principalColumn: "Id");
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
                name: "IX_ModeratorEvaluations_TimeTableId",
                table: "ModeratorEvaluations",
                column: "TimeTableId");
        }
    }
}
