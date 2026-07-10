using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisJobApplicants_AspNetUsers_UserId",
                table: "MisJobApplicants");

            migrationBuilder.DropIndex(
                name: "IX_MisJobApplicants_UserId",
                table: "MisJobApplicants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MisJobApplicants");

            migrationBuilder.AddColumn<long>(
                name: "MisApplicantAccountId",
                table: "MisJobApplicants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MisApplicantAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisApplicantAccounts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 139L, "Provides an analytical overview of active jobs, applicant counts, and interviews.", "ATS Metrics Dashboard", "Card", 85L, "GlobalViewer" },
                    { 140L, "Create a new job posting or manage an existing one.", "Manage Job", "Button", 86L, "GlobalEditor" },
                    { 141L, "View the Kanban-style pipeline of all applicants for a specific role.", "Applicant Pipeline", "Dashboard", 88L, "GlobalViewer" },
                    { 142L, "Progress an applicant to the next stage (e.g., Interview, Offer).", "Change Status", "Form", 89L, "GlobalEditor" },
                    { 143L, "Schedule an offline or online interview and email the candidate.", "Schedule Interview", "Modal", 89L, "GlobalEditor" },
                    { 144L, "Rate the candidate's interview performance based on predefined criteria.", "Score Interview", "Form", 89L, "GlobalEditor" },
                    { 145L, "Automatically provision a real system account for an applicant who has been offered a role.", "Employ Candidate", "Button", 89L, "GlobalEditor" },
                    { 146L, "Securely sign in or register to view your historical applications.", "Applicant Auth Portal", "Form", 90L, "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisJobApplicants_MisApplicantAccountId",
                table: "MisJobApplicants",
                column: "MisApplicantAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisJobApplicants_MisApplicantAccounts_MisApplicantAccountId",
                table: "MisJobApplicants",
                column: "MisApplicantAccountId",
                principalTable: "MisApplicantAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisJobApplicants_MisApplicantAccounts_MisApplicantAccountId",
                table: "MisJobApplicants");

            migrationBuilder.DropTable(
                name: "MisApplicantAccounts");

            migrationBuilder.DropIndex(
                name: "IX_MisJobApplicants_MisApplicantAccountId",
                table: "MisJobApplicants");

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 139L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 140L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 141L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 142L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 143L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 144L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 145L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 146L);

            migrationBuilder.DropColumn(
                name: "MisApplicantAccountId",
                table: "MisJobApplicants");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MisJobApplicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MisJobApplicants_UserId",
                table: "MisJobApplicants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisJobApplicants_AspNetUsers_UserId",
                table: "MisJobApplicants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
