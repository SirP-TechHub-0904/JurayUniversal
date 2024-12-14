using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class projectupdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActivateJobReferrals",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GeneralFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCatgeories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCatgeories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMains",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Materials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileReferralId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectCatgeoryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectCatgeoryId1 = table.Column<long>(type: "bigint", nullable: false),
                    Urls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Authentications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFinised = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastDateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMains_AspNetUsers_ProfileReferralId",
                        column: x => x.ProfileReferralId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectMains_ProjectCatgeories_ProjectCatgeoryId1",
                        column: x => x.ProjectCatgeoryId1,
                        principalTable: "ProjectCatgeories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralFeatureId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneralFeatureId1 = table.Column<long>(type: "bigint", nullable: false),
                    Percentage = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Report = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Challenges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFinised = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectMainId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectMainId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFeatures_GeneralFeatures_GeneralFeatureId1",
                        column: x => x.GeneralFeatureId1,
                        principalTable: "GeneralFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectFeatures_ProjectMains_ProjectMainId1",
                        column: x => x.ProjectMainId1,
                        principalTable: "ProjectMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectMainId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectMainId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFiles_ProjectMains_ProjectMainId1",
                        column: x => x.ProjectMainId1,
                        principalTable: "ProjectMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectMainId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectMainId1 = table.Column<long>(type: "bigint", nullable: false),
                    ProjectPositionId = table.Column<long>(type: "bigint", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLeft = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_ProjectMains_ProjectMainId1",
                        column: x => x.ProjectMainId1,
                        principalTable: "ProjectMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_ProjectPositions_ProjectPositionId",
                        column: x => x.ProjectPositionId,
                        principalTable: "ProjectPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectFeatureId = table.Column<long>(type: "bigint", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFinised = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Report = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Challenges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_ProjectFeatures_ProjectFeatureId",
                        column: x => x.ProjectFeatureId,
                        principalTable: "ProjectFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFeatures_GeneralFeatureId1",
                table: "ProjectFeatures",
                column: "GeneralFeatureId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFeatures_ProjectMainId1",
                table: "ProjectFeatures",
                column: "ProjectMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFiles_ProjectMainId1",
                table: "ProjectFiles",
                column: "ProjectMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMains_ProfileReferralId",
                table: "ProjectMains",
                column: "ProfileReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMains_ProjectCatgeoryId1",
                table: "ProjectMains",
                column: "ProjectCatgeoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProfileId",
                table: "ProjectTasks",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectFeatureId",
                table: "ProjectTasks",
                column: "ProjectFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProfileId",
                table: "ProjectUsers",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectMainId1",
                table: "ProjectUsers",
                column: "ProjectMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectPositionId",
                table: "ProjectUsers",
                column: "ProjectPositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFiles");

            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "ProjectFeatures");

            migrationBuilder.DropTable(
                name: "ProjectPositions");

            migrationBuilder.DropTable(
                name: "GeneralFeatures");

            migrationBuilder.DropTable(
                name: "ProjectMains");

            migrationBuilder.DropTable(
                name: "ProjectCatgeories");

            migrationBuilder.DropColumn(
                name: "ActivateJobReferrals",
                table: "SuperSettings");
        }
    }
}
