using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class studygrop0909 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_StudyGroupCategory_StudyGroupCategoryId1",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_StudyGroupCategoryId1",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "StudyGroupCategoryId1",
                table: "StudyGroups");

            migrationBuilder.AlterColumn<long>(
                name: "StudyGroupCategoryId",
                table: "StudyGroups",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_StudyGroupCategoryId",
                table: "StudyGroups",
                column: "StudyGroupCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroupCategory_CourseId",
                table: "StudyGroupCategory",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroupCategory_Courses_CourseId",
                table: "StudyGroupCategory",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_StudyGroupCategory_StudyGroupCategoryId",
                table: "StudyGroups",
                column: "StudyGroupCategoryId",
                principalTable: "StudyGroupCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroupCategory_Courses_CourseId",
                table: "StudyGroupCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_StudyGroupCategory_StudyGroupCategoryId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_StudyGroupCategoryId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroupCategory_CourseId",
                table: "StudyGroupCategory");

            migrationBuilder.AlterColumn<int>(
                name: "StudyGroupCategoryId",
                table: "StudyGroups",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "StudyGroupCategoryId1",
                table: "StudyGroups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_StudyGroupCategoryId1",
                table: "StudyGroups",
                column: "StudyGroupCategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_StudyGroupCategory_StudyGroupCategoryId1",
                table: "StudyGroups",
                column: "StudyGroupCategoryId1",
                principalTable: "StudyGroupCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
