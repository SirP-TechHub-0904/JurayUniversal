using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class projectup2399 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId",
                table: "ProjectMains");

            migrationBuilder.RenameColumn(
                name: "ProjectCatgeoryId",
                table: "ProjectMains",
                newName: "ProjectCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMains_ProjectCatgeoryId",
                table: "ProjectMains",
                newName: "IX_ProjectMains_ProjectCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCategoryId",
                table: "ProjectMains",
                column: "ProjectCategoryId",
                principalTable: "ProjectCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCategoryId",
                table: "ProjectMains");

            migrationBuilder.RenameColumn(
                name: "ProjectCategoryId",
                table: "ProjectMains",
                newName: "ProjectCatgeoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMains_ProjectCategoryId",
                table: "ProjectMains",
                newName: "IX_ProjectMains_ProjectCatgeoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId",
                table: "ProjectMains",
                column: "ProjectCatgeoryId",
                principalTable: "ProjectCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
