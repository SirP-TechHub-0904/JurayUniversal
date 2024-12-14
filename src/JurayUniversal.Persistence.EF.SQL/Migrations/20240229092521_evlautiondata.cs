using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class evlautiondata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NutritionCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionCategoryLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultType = table.Column<int>(type: "int", nullable: false),
                    NutritionCategoryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionCategoryLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionCategoryLists_NutritionCategories_NutritionCategoryId",
                        column: x => x.NutritionCategoryId,
                        principalTable: "NutritionCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserNutritionEvaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutritionCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    NutritionCategoryListId = table.Column<long>(type: "bigint", nullable: false),
                    ResultType = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },

                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNutritionEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNutritionEvaluations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserNutritionEvaluations_NutritionCategories_NutritionCategoryId",
                        column: x => x.NutritionCategoryId,
                        principalTable: "NutritionCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserNutritionEvaluations_NutritionCategoryLists_NutritionCategoryListId",
                        column: x => x.NutritionCategoryListId,

                        principalTable: "NutritionCategoryLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NutritionCategoryLists_NutritionCategoryId",
                table: "NutritionCategoryLists",
                column: "NutritionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNutritionEvaluations_NutritionCategoryId",
                table: "UserNutritionEvaluations",
                column: "NutritionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNutritionEvaluations_NutritionCategoryListId",
                table: "UserNutritionEvaluations",
                column: "NutritionCategoryListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNutritionEvaluations_UserId",
                table: "UserNutritionEvaluations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNutritionEvaluations");

            migrationBuilder.DropTable(
                name: "NutritionCategoryLists");

            migrationBuilder.DropTable(
                name: "NutritionCategories");
        }
    }
}
