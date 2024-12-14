using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class updateds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StudyGroupId",
                table: "StaffManagers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffManagers_StudyGroupId",
                table: "StaffManagers",
                column: "StudyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffManagers_StudyGroups_StudyGroupId",
                table: "StaffManagers",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffManagers_StudyGroups_StudyGroupId",
                table: "StaffManagers");

            migrationBuilder.DropIndex(
                name: "IX_StaffManagers_StudyGroupId",
                table: "StaffManagers");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "StaffManagers");
        }
    }
}
