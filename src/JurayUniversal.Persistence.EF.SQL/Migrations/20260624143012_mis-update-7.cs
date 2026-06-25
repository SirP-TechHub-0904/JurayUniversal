using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MisManuals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleFolder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisManuals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisManualPages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisManualId = table.Column<long>(type: "bigint", nullable: false),
                    RouteKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisManualPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisManualPages_MisManuals_MisManualId",
                        column: x => x.MisManualId,
                        principalTable: "MisManuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisManualElements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisManualPageId = table.Column<long>(type: "bigint", nullable: false),
                    ElementName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisManualElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisManualElements_MisManualPages_MisManualPageId",
                        column: x => x.MisManualPageId,
                        principalTable: "MisManualPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 1L, "MisUserManagement", "Manage all platform users, roles, and KPIs here.", "User Management Guide" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 1L, 1L, "The main directory of all users in the system.", "User Directory", "/MisUserManagement/Dashboard/Index" });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { 1L, "Click this button to register a new user in the system.", "Add User Button", "Button", 1L, "GlobalEditor" });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { 2L, "Allows you to suspend or activate a user account.", "User Status Toggle", "Button", 1L, "SystemAdmin" });

            migrationBuilder.CreateIndex(
                name: "IX_MisManualElements_MisManualPageId",
                table: "MisManualElements",
                column: "MisManualPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MisManualPages_MisManualId",
                table: "MisManualPages",
                column: "MisManualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisManualElements");

            migrationBuilder.DropTable(
                name: "MisManualPages");

            migrationBuilder.DropTable(
                name: "MisManuals");
        }
    }
}
