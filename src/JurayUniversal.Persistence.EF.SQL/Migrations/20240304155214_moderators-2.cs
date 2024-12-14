using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class moderators2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moderator_AspNetUsers_UserId",
                table: "Moderator");

            migrationBuilder.DropForeignKey(
                name: "FK_Moderator_TimeTables_TimeTableId",
                table: "Moderator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Moderator",
                table: "Moderator");

            migrationBuilder.RenameTable(
                name: "Moderator",
                newName: "Moderators");

            migrationBuilder.RenameIndex(
                name: "IX_Moderator_UserId",
                table: "Moderators",
                newName: "IX_Moderators_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Moderator_TimeTableId",
                table: "Moderators",
                newName: "IX_Moderators_TimeTableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Moderators",
                table: "Moderators",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Moderators_AspNetUsers_UserId",
                table: "Moderators",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Moderators_TimeTables_TimeTableId",
                table: "Moderators",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moderators_AspNetUsers_UserId",
                table: "Moderators");

            migrationBuilder.DropForeignKey(
                name: "FK_Moderators_TimeTables_TimeTableId",
                table: "Moderators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Moderators",
                table: "Moderators");

            migrationBuilder.RenameTable(
                name: "Moderators",
                newName: "Moderator");

            migrationBuilder.RenameIndex(
                name: "IX_Moderators_UserId",
                table: "Moderator",
                newName: "IX_Moderator_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Moderators_TimeTableId",
                table: "Moderator",
                newName: "IX_Moderator_TimeTableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Moderator",
                table: "Moderator",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Moderator_AspNetUsers_UserId",
                table: "Moderator",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Moderator_TimeTables_TimeTableId",
                table: "Moderator",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id");
        }
    }
}
