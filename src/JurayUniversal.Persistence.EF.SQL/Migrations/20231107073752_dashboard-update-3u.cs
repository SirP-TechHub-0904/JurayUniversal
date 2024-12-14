using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class dashboardupdate3u : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "DashboardDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserCategoryId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDashboard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserCategoryId",
                table: "AspNetUsers",
                column: "UserCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserCategories_UserCategoryId",
                table: "AspNetUsers",
                column: "UserCategoryId",
                principalTable: "UserCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserCategories_UserCategoryId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserCategories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCategoryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "DashboardDatas");

            migrationBuilder.DropColumn(
                name: "UserCategoryId",
                table: "AspNetUsers");
        }
    }
}
