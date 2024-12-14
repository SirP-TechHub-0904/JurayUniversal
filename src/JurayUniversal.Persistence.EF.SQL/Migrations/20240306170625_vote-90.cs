using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class vote90 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVotes_VoteCategories_VoteCategoryId",
                table: "UserVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteCategories_AspNetUsers_CandidateId",
                table: "VoteCategories");

            migrationBuilder.DropIndex(
                name: "IX_VoteCategories_CandidateId",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "CandidateImageKey",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "CandidateImageUrl",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "Manifesto",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "VoteCategories");

            migrationBuilder.RenameColumn(
                name: "ShortProfile",
                table: "VoteCategories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "VoteCategoryId",
                table: "UserVotes",
                newName: "VoteCondidateId");

            migrationBuilder.RenameIndex(
                name: "IX_UserVotes_VoteCategoryId",
                table: "UserVotes",
                newName: "IX_UserVotes_VoteCondidateId");

            migrationBuilder.CreateTable(
                name: "VoteCondidates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoteCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultPosition = table.Column<int>(type: "int", nullable: false),
                    CandidateImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidateImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manifesto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteCondidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteCondidates_AspNetUsers_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoteCondidates_VoteCategories_VoteCategoryId",
                        column: x => x.VoteCategoryId,
                        principalTable: "VoteCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoteCondidates_CandidateId",
                table: "VoteCondidates",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteCondidates_VoteCategoryId",
                table: "VoteCondidates",
                column: "VoteCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserVotes_VoteCondidates_VoteCondidateId",
                table: "UserVotes",
                column: "VoteCondidateId",
                principalTable: "VoteCondidates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVotes_VoteCondidates_VoteCondidateId",
                table: "UserVotes");

            migrationBuilder.DropTable(
                name: "VoteCondidates");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "VoteCategories",
                newName: "ShortProfile");

            migrationBuilder.RenameColumn(
                name: "VoteCondidateId",
                table: "UserVotes",
                newName: "VoteCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserVotes_VoteCondidateId",
                table: "UserVotes",
                newName: "IX_UserVotes_VoteCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "CandidateId",
                table: "VoteCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CandidateImageKey",
                table: "VoteCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CandidateImageUrl",
                table: "VoteCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "VoteCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Manifesto",
                table: "VoteCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "VoteCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoteCategories_CandidateId",
                table: "VoteCategories",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserVotes_VoteCategories_VoteCategoryId",
                table: "UserVotes",
                column: "VoteCategoryId",
                principalTable: "VoteCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteCategories_AspNetUsers_CandidateId",
                table: "VoteCategories",
                column: "CandidateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
