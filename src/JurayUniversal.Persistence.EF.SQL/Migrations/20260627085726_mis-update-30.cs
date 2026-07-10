using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisProposals_AspNetUsers_UserProfileId",
                table: "MisProposals");

            migrationBuilder.DropIndex(
                name: "IX_MisProposals_UserProfileId",
                table: "MisProposals");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "MisProposals");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MisProposals",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MisJobPostings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountOrSalary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisJobPostings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisJobApplicants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisJobPostingId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessionalSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisJobApplicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisJobApplicants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisJobApplicants_MisJobPostings_MisJobPostingId",
                        column: x => x.MisJobPostingId,
                        principalTable: "MisJobPostings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisJobApplicantDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisJobApplicantId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisJobApplicantDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisJobApplicantDocuments_MisJobApplicants_MisJobApplicantId",
                        column: x => x.MisJobApplicantId,
                        principalTable: "MisJobApplicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisJobInterviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisJobApplicantId = table.Column<long>(type: "bigint", nullable: false),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationOrLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewType = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true),
                    AdminFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisJobInterviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisJobInterviews_MisJobApplicants_MisJobApplicantId",
                        column: x => x.MisJobApplicantId,
                        principalTable: "MisJobApplicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 13L, "MisRecruitment", "Applicant Tracking System (ATS) for managing job postings, candidate pipelines, structured CVs, and interview scoring.", "Recruitment & ATS Guide" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 85L, 13L, "High-level overview of active jobs, total applicants, and pending interviews.", "ATS Dashboard", "/MisRecruitment/Dashboard/Index" },
                    { 86L, 13L, "List of all published, closed, and draft job postings.", "Job Postings", "/MisRecruitment/Dashboard/Jobs/Index" },
                    { 87L, 13L, "Create or update a job posting including descriptions, salary, and requirements.", "Manage Job Posting", "/MisRecruitment/Dashboard/Jobs/Manage" },
                    { 88L, 13L, "The pipeline list of all candidates who have applied, filterable by Job Title.", "Applicant Pipeline", "/MisRecruitment/Dashboard/Applicants/Index" },
                    { 89L, 13L, "Detailed view of an applicant's structured CV, attached documents, and interview scoring.", "Review Candidate", "/MisRecruitment/Dashboard/Applicants/Details" },
                    { 90L, 13L, "The public portal where candidates can browse open roles.", "Public Job Board", "/MisRecruitment/Platform/JobBoard" },
                    { 91L, 13L, "The structured application form where candidates enter their details and upload their CV/documents.", "Application Form", "/MisRecruitment/Platform/Apply" },
                    { 92L, 13L, "The public portal where candidates enter their Tracking ID to view the status of their application.", "Track Application", "/MisRecruitment/Platform/Track" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisProposals_UserId",
                table: "MisProposals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisJobApplicantDocuments_MisJobApplicantId",
                table: "MisJobApplicantDocuments",
                column: "MisJobApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_MisJobApplicants_MisJobPostingId",
                table: "MisJobApplicants",
                column: "MisJobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_MisJobApplicants_UserId",
                table: "MisJobApplicants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisJobInterviews_MisJobApplicantId",
                table: "MisJobInterviews",
                column: "MisJobApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisProposals_AspNetUsers_UserId",
                table: "MisProposals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MisProposals_AspNetUsers_UserId",
                table: "MisProposals");

            migrationBuilder.DropTable(
                name: "MisJobApplicantDocuments");

            migrationBuilder.DropTable(
                name: "MisJobInterviews");

            migrationBuilder.DropTable(
                name: "MisJobApplicants");

            migrationBuilder.DropTable(
                name: "MisJobPostings");

            migrationBuilder.DropIndex(
                name: "IX_MisProposals_UserId",
                table: "MisProposals");

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 85L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 86L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 88L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 89L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 90L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 91L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MisProposals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                table: "MisProposals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MisProposals_UserProfileId",
                table: "MisProposals",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_MisProposals_AspNetUsers_UserProfileId",
                table: "MisProposals",
                column: "UserProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
