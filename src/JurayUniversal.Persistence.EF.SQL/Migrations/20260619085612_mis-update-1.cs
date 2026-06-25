using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MisDashboardCSS",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MisLayoutJavaScript",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MisKpiIndicators",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisKpiIndicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisUserCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisUserCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisUserFeatureRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeatureAreaKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionRole = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisUserFeatureRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisUserFeatureRoles_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisUserNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisUserNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisUserNotes_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisKpiUserScores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MisKpiIndicatorId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisKpiUserScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisKpiUserScores_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisKpiUserScores_MisKpiIndicators_MisKpiIndicatorId",
                        column: x => x.MisKpiIndicatorId,
                        principalTable: "MisKpiIndicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisKpiUserScores_MisKpiIndicatorId",
                table: "MisKpiUserScores",
                column: "MisKpiIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MisKpiUserScores_ProfileId",
                table: "MisKpiUserScores",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisUserFeatureRoles_ProfileId",
                table: "MisUserFeatureRoles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisUserNotes_ProfileId",
                table: "MisUserNotes",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisKpiUserScores");

            migrationBuilder.DropTable(
                name: "MisUserCategories");

            migrationBuilder.DropTable(
                name: "MisUserFeatureRoles");

            migrationBuilder.DropTable(
                name: "MisUserNotes");

            migrationBuilder.DropTable(
                name: "MisKpiIndicators");

            migrationBuilder.DropColumn(
                name: "MisDashboardCSS",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "MisLayoutJavaScript",
                table: "DataConfigs");
        }
    }
}
