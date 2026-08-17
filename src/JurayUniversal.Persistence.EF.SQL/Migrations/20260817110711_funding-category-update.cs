using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    /// <inheritdoc />
    public partial class fundingcategoryupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MisGroupId",
                table: "MisFinanceCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserCategoryId",
                table: "MisFinanceCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MisFinanceCategories_MisGroupId",
                table: "MisFinanceCategories",
                column: "MisGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MisFinanceCategories_UserCategoryId",
                table: "MisFinanceCategories",
                column: "UserCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisFinanceCategories_MisGroups_MisGroupId",
                table: "MisFinanceCategories",
                column: "MisGroupId",
                principalTable: "MisGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MisFinanceCategories_UserCategories_UserCategoryId",
                table: "MisFinanceCategories",
                column: "UserCategoryId",
                principalTable: "UserCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisFinanceCategories_MisGroups_MisGroupId",
                table: "MisFinanceCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MisFinanceCategories_UserCategories_UserCategoryId",
                table: "MisFinanceCategories");

            migrationBuilder.DropIndex(
                name: "IX_MisFinanceCategories_MisGroupId",
                table: "MisFinanceCategories");

            migrationBuilder.DropIndex(
                name: "IX_MisFinanceCategories_UserCategoryId",
                table: "MisFinanceCategories");

            migrationBuilder.DropColumn(
                name: "MisGroupId",
                table: "MisFinanceCategories");

            migrationBuilder.DropColumn(
                name: "UserCategoryId",
                table: "MisFinanceCategories");
        }
    }
}
