using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class votingfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteCategories_Votings_VotingId",
                table: "VoteCategories");

            migrationBuilder.DropTable(
                name: "Votings");

            migrationBuilder.RenameColumn(
                name: "VotingId",
                table: "VoteCategories",
                newName: "EvotingId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteCategories_VotingId",
                table: "VoteCategories",
                newName: "IX_VoteCategories_EvotingId");

            migrationBuilder.CreateTable(
                name: "Evotings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateToStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VotingStatus = table.Column<int>(type: "int", nullable: false),
                    StartVoting = table.Column<bool>(type: "bit", nullable: false),
                    ShowResult = table.Column<bool>(type: "bit", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evotings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evotings_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evotings_CourseId",
                table: "Evotings",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteCategories_Evotings_EvotingId",
                table: "VoteCategories",
                column: "EvotingId",
                principalTable: "Evotings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteCategories_Evotings_EvotingId",
                table: "VoteCategories");

            migrationBuilder.DropTable(
                name: "Evotings");

            migrationBuilder.RenameColumn(
                name: "EvotingId",
                table: "VoteCategories",
                newName: "VotingId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteCategories_EvotingId",
                table: "VoteCategories",
                newName: "IX_VoteCategories_VotingId");

            migrationBuilder.CreateTable(
                name: "Votings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowResult = table.Column<bool>(type: "bit", nullable: false),
                    StartVoting = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VotingStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votings_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votings_CourseId",
                table: "Votings",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteCategories_Votings_VotingId",
                table: "VoteCategories",
                column: "VotingId",
                principalTable: "Votings",
                principalColumn: "Id");
        }
    }
}
