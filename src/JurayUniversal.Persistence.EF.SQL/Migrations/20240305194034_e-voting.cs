using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class evoting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Votings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VotingStatus = table.Column<int>(type: "int", nullable: false),
                    StartVoting = table.Column<bool>(type: "bit", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "VoteCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotingId = table.Column<long>(type: "bigint", nullable: true),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CandidateImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidateImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manifesto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteCategories_AspNetUsers_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoteCategories_Votings_VotingId",
                        column: x => x.VotingId,
                        principalTable: "Votings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserVotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoteCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    VoterUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserVoteStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVotes_AspNetUsers_VoterUserId",
                        column: x => x.VoterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserVotes_VoteCategories_VoteCategoryId",
                        column: x => x.VoteCategoryId,
                        principalTable: "VoteCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_VoteCategoryId",
                table: "UserVotes",
                column: "VoteCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_VoterUserId",
                table: "UserVotes",
                column: "VoterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteCategories_CandidateId",
                table: "VoteCategories",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteCategories_VotingId",
                table: "VoteCategories",
                column: "VotingId");

            migrationBuilder.CreateIndex(
                name: "IX_Votings_CourseId",
                table: "Votings",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserVotes");

            migrationBuilder.DropTable(
                name: "VoteCategories");

            migrationBuilder.DropTable(
                name: "Votings");
        }
    }
}
