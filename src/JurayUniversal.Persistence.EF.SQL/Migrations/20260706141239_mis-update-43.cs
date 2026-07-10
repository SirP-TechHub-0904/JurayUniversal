using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisWidgets",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.CreateTable(
                name: "MisLeaveRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateReviewed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisLeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisLeaveRequests_AspNetUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisLeaveRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisWizardSteps",
                columns: new[] { "Id", "Content", "FeatureName", "StepOrder", "TargetElement", "Title" },
                values: new object[,]
                {
                    { 13L, "Review all leave requests and excuses submitted by platform users.", "MisUserManagement_Leaves", 1, ".page-header", "Leaves & Excuses" },
                    { 14L, "Click 'Review' on any pending request to approve or deny it with remarks.", "MisUserManagement_Leaves", 2, "#leaves-table", "Request Queue" },
                    { 15L, "Need time off or running late? Click here to submit a formal request.", "Platform_Leaves", 1, "#btn-new-request", "Submit a Request" },
                    { 16L, "Monitor the approval status of your submitted requests here.", "Platform_Leaves", 2, "#my-requests-table", "Track Status" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisLeaveRequests_ReviewedById",
                table: "MisLeaveRequests",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_MisLeaveRequests_UserId",
                table: "MisLeaveRequests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisLeaveRequests");

            migrationBuilder.DeleteData(
                table: "MisWizardSteps",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "MisWizardSteps",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "MisWizardSteps",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "MisWizardSteps",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.InsertData(
                table: "MisWidgets",
                columns: new[] { "Id", "ColSpan", "SortOrder", "WidgetCode" },
                values: new object[] { 4L, 8, 4, "financials_chart" });
        }
    }
}
