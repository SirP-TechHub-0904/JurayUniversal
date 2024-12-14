using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class taskop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XProjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateToStartTesting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateToGoLive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XProjectMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepliedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    XProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XProjectMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XProjectMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_XProjectMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_XProjectMessages_XProjects_XProjectId",
                        column: x => x.XProjectId,
                        principalTable: "XProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "XProjectSections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateToStartTesting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    XProjectId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XProjectSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XProjectSections_XProjects_XProjectId",
                        column: x => x.XProjectId,
                        principalTable: "XProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XProjectTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RateUser = table.Column<int>(type: "int", nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestCriterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestStatus = table.Column<int>(type: "int", nullable: false),
                    TestedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RateTester = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproveStatus = table.Column<int>(type: "int", nullable: false),
                    RateApprover = table.Column<int>(type: "int", nullable: false),
                    XProjectSectionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XProjectTasks_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_XProjectTasks_AspNetUsers_TestedById",
                        column: x => x.TestedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_XProjectTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_XProjectTasks_XProjectSections_XProjectSectionId",
                        column: x => x.XProjectSectionId,
                        principalTable: "XProjectSections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_XProjectMessages_ReceiverId",
                table: "XProjectMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectMessages_SenderId",
                table: "XProjectMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectMessages_XProjectId",
                table: "XProjectMessages",
                column: "XProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectSections_XProjectId",
                table: "XProjectSections",
                column: "XProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectTasks_ApprovedById",
                table: "XProjectTasks",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectTasks_TestedById",
                table: "XProjectTasks",
                column: "TestedById");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectTasks_UserId",
                table: "XProjectTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_XProjectTasks_XProjectSectionId",
                table: "XProjectTasks",
                column: "XProjectSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XProjectMessages");

            migrationBuilder.DropTable(
                name: "XProjectTasks");

            migrationBuilder.DropTable(
                name: "XProjectSections");

            migrationBuilder.DropTable(
                name: "XProjects");
        }
    }
}
