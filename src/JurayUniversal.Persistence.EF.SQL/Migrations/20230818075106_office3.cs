using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class office3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActivateOfficeActivity",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OfficeActivityCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeActivityCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeActivityDialies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeActivityCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Logs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateCount = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeActivityDialies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeActivityDialies_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeActivityDialies_OfficeActivityCategories_OfficeActivityCategoryId",
                        column: x => x.OfficeActivityCategoryId,
                        principalTable: "OfficeActivityCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeActivityDialies_LastUpdateById",
                table: "OfficeActivityDialies",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeActivityDialies_OfficeActivityCategoryId",
                table: "OfficeActivityDialies",
                column: "OfficeActivityCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeActivityDialies");

            migrationBuilder.DropTable(
                name: "OfficeActivityCategories");

            migrationBuilder.DropColumn(
                name: "ActivateOfficeActivity",
                table: "SuperSettings");
        }
    }
}
