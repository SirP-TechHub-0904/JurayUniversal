using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class uservote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VoteCategoryId",
                table: "UserVotes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_VoteCategoryId",
                table: "UserVotes",
                column: "VoteCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserVotes_VoteCategories_VoteCategoryId",
                table: "UserVotes",
                column: "VoteCategoryId",
                principalTable: "VoteCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVotes_VoteCategories_VoteCategoryId",
                table: "UserVotes");

            migrationBuilder.DropIndex(
                name: "IX_UserVotes_VoteCategoryId",
                table: "UserVotes");

            migrationBuilder.DropColumn(
                name: "VoteCategoryId",
                table: "UserVotes");
        }
    }
}
