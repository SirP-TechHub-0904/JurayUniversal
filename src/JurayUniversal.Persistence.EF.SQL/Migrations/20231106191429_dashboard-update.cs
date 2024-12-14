using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class dashboardupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyProgramCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDashboard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProgramCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DashboardDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DashboardSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityProgramTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowActivityProgram = table.Column<bool>(type: "bit", nullable: false),
                    UserCustomDashboard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPrograms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyProgramCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Show = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPrograms_CompanyProgramCategories_CompanyProgramCategoryId",
                        column: x => x.CompanyProgramCategoryId,
                        principalTable: "CompanyProgramCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DashboardDataLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataIdQuery = table.Column<long>(type: "bigint", nullable: false),
                    DashboardDataId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardDataLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DashboardDataLists_DashboardDatas_DashboardDataId",
                        column: x => x.DashboardDataId,
                        principalTable: "DashboardDatas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyProgramId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProgramUsers_CompanyPrograms_CompanyProgramId",
                        column: x => x.CompanyProgramId,
                        principalTable: "CompanyPrograms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPrograms_CompanyProgramCategoryId",
                table: "CompanyPrograms",
                column: "CompanyProgramCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardDataLists_DashboardDataId",
                table: "DashboardDataLists",
                column: "DashboardDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramUsers_CompanyProgramId",
                table: "ProgramUsers",
                column: "CompanyProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramUsers_UserId",
                table: "ProgramUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardDataLists");

            migrationBuilder.DropTable(
                name: "DashboardSettings");

            migrationBuilder.DropTable(
                name: "ProgramUsers");

            migrationBuilder.DropTable(
                name: "DashboardDatas");

            migrationBuilder.DropTable(
                name: "CompanyPrograms");

            migrationBuilder.DropTable(
                name: "CompanyProgramCategories");
        }
    }
}
