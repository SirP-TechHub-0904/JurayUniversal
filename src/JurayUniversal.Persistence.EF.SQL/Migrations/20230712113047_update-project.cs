using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class updateproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMains_ProjectCatgeories_ProjectCatgeoryId1",
                table: "ProjectMains");

            migrationBuilder.DropTable(
                name: "ProjectCatgeories");

            migrationBuilder.CreateTable(
                name: "ProjectCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId1",
                table: "ProjectMains",
                column: "ProjectCatgeoryId1",
                principalTable: "ProjectCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId1",
                table: "ProjectMains");

            migrationBuilder.DropTable(
                name: "ProjectCategories");

            migrationBuilder.CreateTable(
                name: "ProjectCatgeories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCatgeories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMains_ProjectCatgeories_ProjectCatgeoryId1",
                table: "ProjectMains",
                column: "ProjectCatgeoryId1",
                principalTable: "ProjectCatgeories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
