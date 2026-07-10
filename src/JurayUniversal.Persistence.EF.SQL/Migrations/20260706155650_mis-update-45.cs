using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MisUserRecentVisits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleFolder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastVisitedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisUserRecentVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisUserRecentVisits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "MisWidgets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ColSpan",
                value: 5);

            migrationBuilder.UpdateData(
                table: "MisWidgets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ColSpan",
                value: 7);

            migrationBuilder.CreateIndex(
                name: "IX_MisUserRecentVisits_UserId",
                table: "MisUserRecentVisits",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisUserRecentVisits");

            migrationBuilder.UpdateData(
                table: "MisWidgets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ColSpan",
                value: 12);

            migrationBuilder.UpdateData(
                table: "MisWidgets",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ColSpan",
                value: 4);
        }
    }
}
