using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MisEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Venue = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BannerImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MisFinanceCategoryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisEvents_MisFinanceCategories_MisFinanceCategoryId",
                        column: x => x.MisFinanceCategoryId,
                        principalTable: "MisFinanceCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisProposals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypedProposal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateApproved = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisProposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisProposals_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisTicketTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisEventId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCapacity = table.Column<int>(type: "int", nullable: true),
                    SoldCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisTicketTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisTicketTypes_MisEvents_MisEventId",
                        column: x => x.MisEventId,
                        principalTable: "MisEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisGrants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisProposalId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DonorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalAwardAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisGrants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisGrants_MisProposals_MisProposalId",
                        column: x => x.MisProposalId,
                        principalTable: "MisProposals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisProposalFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisProposalId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisProposalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisProposalFiles_MisProposals_MisProposalId",
                        column: x => x.MisProposalId,
                        principalTable: "MisProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisEventTickets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisTicketTypeId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GuestEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GuestPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    ScannedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScannedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisEventTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisEventTickets_AspNetUsers_ScannedByUserId",
                        column: x => x.ScannedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisEventTickets_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisEventTickets_MisTicketTypes_MisTicketTypeId",
                        column: x => x.MisTicketTypeId,
                        principalTable: "MisTicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisGrantMilestones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisGrantId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ExpectedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisGrantMilestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisGrantMilestones_MisGrants_MisGrantId",
                        column: x => x.MisGrantId,
                        principalTable: "MisGrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 7L, "MisTicketing", "Complete guide for managing events, generating ticket tiers, processing payments, and scanning QR codes.", "Event & Ticket Engine Guide" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 48L, 7L, "The main dashboard listing all your events and showing high-level stats like total tickets sold.", "Events Dashboard", "/MisTicketing/Dashboard/Index" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 49L, 7L, "Set up a new event, including dates, venue, and descriptions.", "Create Event", "/MisTicketing/Dashboard/Create" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 50L, 7L, "Manage an event's ticket tiers and check the real-time capacity and sales figures.", "Manage Ticket Tiers", "/MisTicketing/Dashboard/Manage" });

            migrationBuilder.CreateIndex(
                name: "IX_MisEvents_MisFinanceCategoryId",
                table: "MisEvents",
                column: "MisFinanceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MisEventTickets_MisTicketTypeId",
                table: "MisEventTickets",
                column: "MisTicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MisEventTickets_ScannedByUserId",
                table: "MisEventTickets",
                column: "ScannedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisEventTickets_UserProfileId",
                table: "MisEventTickets",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGrantMilestones_MisGrantId",
                table: "MisGrantMilestones",
                column: "MisGrantId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGrants_MisProposalId",
                table: "MisGrants",
                column: "MisProposalId",
                unique: true,
                filter: "[MisProposalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MisProposalFiles_MisProposalId",
                table: "MisProposalFiles",
                column: "MisProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_MisProposals_UserProfileId",
                table: "MisProposals",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisTicketTypes_MisEventId",
                table: "MisTicketTypes",
                column: "MisEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisEventTickets");

            migrationBuilder.DropTable(
                name: "MisGrantMilestones");

            migrationBuilder.DropTable(
                name: "MisProposalFiles");

            migrationBuilder.DropTable(
                name: "MisTicketTypes");

            migrationBuilder.DropTable(
                name: "MisGrants");

            migrationBuilder.DropTable(
                name: "MisEvents");

            migrationBuilder.DropTable(
                name: "MisProposals");

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 7L);
        }
    }
}
