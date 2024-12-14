using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class databudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetMainCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessEmails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetMainCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetMainCategories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetSubCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Show = table.Column<bool>(type: "bit", nullable: false),
                    BudgetMainCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetSubCategories_BudgetMainCategories_BudgetMainCategoryId",
                        column: x => x.BudgetMainCategoryId,
                        principalTable: "BudgetMainCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetSubCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    AmountInNaira = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountInDollar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLists_BudgetSubCategories_BudgetSubCategoryId",
                        column: x => x.BudgetSubCategoryId,
                        principalTable: "BudgetSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_BudgetSubCategoryId",
                table: "BudgetLists",
                column: "BudgetSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetMainCategories_UserId",
                table: "BudgetMainCategories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetSubCategories_BudgetMainCategoryId",
                table: "BudgetSubCategories",
                column: "BudgetMainCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLists");

            migrationBuilder.DropTable(
                name: "BudgetSubCategories");

            migrationBuilder.DropTable(
                name: "BudgetMainCategories");
        }
    }
}
