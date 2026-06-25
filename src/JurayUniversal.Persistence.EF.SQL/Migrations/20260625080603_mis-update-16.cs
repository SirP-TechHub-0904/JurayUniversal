using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlumniAlias",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MisAlumniPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisAlumniPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisAlumniPosts_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisAlumniProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExitYear = table.Column<int>(type: "int", nullable: false),
                    CurrentProfession = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAvailableForMentorship = table.Column<bool>(type: "bit", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisAlumniProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisAlumniProfiles_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisReportSchemas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeadlineTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DeadlineDayOfWeek = table.Column<int>(type: "int", nullable: true),
                    DeadlineDayOfMonth = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RequireTimeLog = table.Column<bool>(type: "bit", nullable: false),
                    RequireAchievements = table.Column<bool>(type: "bit", nullable: false),
                    RequireChallenges = table.Column<bool>(type: "bit", nullable: false),
                    RequireNextDayPlan = table.Column<bool>(type: "bit", nullable: false),
                    RequireFileUpload = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisReportSchemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisReportParticipants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisReportSchemaId = table.Column<long>(type: "bigint", nullable: false),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisReportParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisReportParticipants_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisReportParticipants_MisReportSchemas_MisReportSchemaId",
                        column: x => x.MisReportSchemaId,
                        principalTable: "MisReportSchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisReportSubmissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisReportSchemaId = table.Column<long>(type: "bigint", nullable: false),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskAssigned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achievements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Challenges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlansForNextPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisReportSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisReportSubmissions_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisReportSubmissions_MisReportSchemas_MisReportSchemaId",
                        column: x => x.MisReportSchemaId,
                        principalTable: "MisReportSchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisReportTimeLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisReportSubmissionId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ActivityDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisReportTimeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisReportTimeLogs_MisReportSubmissions_MisReportSubmissionId",
                        column: x => x.MisReportSubmissionId,
                        principalTable: "MisReportSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 18L, 2L, "A form to create a new organizational group.", "Create Group", "/MisGroups/Dashboard/Create" },
                    { 19L, 2L, "A form to update the details of an existing group.", "Edit Group", "/MisGroups/Dashboard/Edit" },
                    { 20L, 2L, "An invitation landing page where users can accept invites to join a group.", "Join Group", "/MisGroups/Platform/Join" }
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[,]
                {
                    { 3L, "MisAlumni", "The Alumni Networking Directory allows past members to connect, find mentorship, and share business opportunities in a secure environment. Users can maintain professional profiles, flag their mentorship availability, and post on a community bulletin board.", "Alumni Network Guide" },
                    { 4L, "MisReports", "Dynamic Interval Reporting enforces strict submission deadlines for required staff reports, enabling administrators to track compliance automatically. Using advanced Template Flags, admins can dynamically build reports requiring hourly logs, file uploads, achievements, and challenges.", "Dynamic Reports Guide" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 35L, "Fill out the required information to initialize a new group.", "Create Form", "Form", 18L, "GlobalEditor" },
                    { 36L, "Modify the fundamental details of this group.", "Edit Form", "Form", 19L, "GlobalEditor" },
                    { 37L, "Accept the invitation to join this group. You will either be granted immediate access or be placed in a pending queue for admin approval.", "Join Request", "Button", 20L, "" }
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 21L, 3L, "Administrative overview of the alumni network, displaying total members and available mentors.", "Alumni Dashboard", "/MisAlumni/Dashboard/Index" },
                    { 22L, 3L, "Admin view to verify pending alumni or revoke access.", "Alumni Directory (Admin)", "/MisAlumni/Dashboard/List" },
                    { 23L, 3L, "A form to manually flag an existing user as an Alumni.", "Add Alumni", "/MisAlumni/Dashboard/Create" },
                    { 24L, 3L, "The welcome dashboard for the Alumni Network showing recent posts and featured mentors.", "Network Welcome", "/MisAlumni/Platform/Index" },
                    { 25L, 3L, "A searchable grid of all verified Alumni. You can filter by profession or mentorship availability.", "Member Directory", "/MisAlumni/Platform/Directory" },
                    { 26L, 3L, "The central bulletin board for business opportunities and networking requests.", "Opportunities Board", "/MisAlumni/Platform/Opportunities" },
                    { 27L, 3L, "Your personal portal to update your employment history and mentorship status.", "My Alumni Profile", "/MisAlumni/Platform/MyProfile" },
                    { 28L, 4L, "Graphical dashboard showing On-Time versus Late submission trends over the last 7 days. Helps management quickly identify reporting bottlenecks.", "Reports Analytics", "/MisReports/Dashboard/Index" },
                    { 29L, 4L, "Manage the definitions, deadlines, and Dynamic Templates for different reports. Toggle on/off features like Hourly Logs or File Uploads to shape exactly what staff must submit.", "Manage Schemas", "/MisReports/Dashboard/Schemas/Index" },
                    { 30L, 4L, "Assign specific staff members who must submit this report. Only these enrolled participants will see the report on their platform dashboard.", "Required Participants", "/MisReports/Dashboard/Schemas/Participants" },
                    { 31L, 4L, "The master ledger displaying all historical report submissions and their timing status. Click into any submission to read the full report, download attachments, and leave Manager Comments.", "Submissions Ledger", "/MisReports/Dashboard/Submissions" },
                    { 32L, 4L, "Your personal queue of reports that are due today or in the near future. Keep an eye on the deadline to avoid late flags.", "My Pending Reports", "/MisReports/Platform/Index" },
                    { 33L, 4L, "The dynamic form where you write and submit your required report before the deadline. The fields you see are strictly defined by the Admin's template flags.", "Submit Report", "/MisReports/Platform/Submit" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 38L, "Click any analytical card to immediately navigate to a filtered view of the alumni roster.", "Analytics Cards", "Card", 21L, "GlobalViewer" },
                    { 39L, "Review a pending user's request to join the Alumni network and grant them official access.", "Verify Status", "Button", 22L, "GlobalEditor" },
                    { 40L, "Select an existing active user and explicitly grant them access to the Alumni Network.", "Flag Alumni", "Form", 23L, "GlobalEditor" },
                    { 41L, "Quickly navigate to the searchable directory or the opportunities board.", "Welcome Actions", "Button", 24L, "" },
                    { 42L, "Toggle this to only see members who have explicitly stated they are open to mentoring others.", "Mentors Only Filter", "Toggle", 25L, "" },
                    { 43L, "Click to view the user's public LinkedIn profile in a new tab.", "LinkedIn Connect", "Button", 25L, "" },
                    { 44L, "Filter the bulletin board to only show specific types of posts, like Job Openings.", "Post Filters", "Button", 26L, "" },
                    { 45L, "Draft a new business or networking opportunity to share with the entire alumni community.", "Create Post", "Form", 26L, "" },
                    { 46L, "Update your graduation year, current company, and professional title.", "Profile Details", "Form", 27L, "" },
                    { 47L, "Enable this switch to let other alumni know you are willing to mentor them.", "Mentorship Toggle", "Toggle", 27L, "" },
                    { 48L, "Visual representation of staff compliance over the last 7 days. Green indicates on-time submissions, yellow indicates late.", "Submission Trends Chart", "Chart", 28L, "GlobalViewer" },
                    { 49L, "Toggle to immediately disable this report schema. Staff will no longer be required to submit it.", "Active Toggle", "Toggle", 29L, "GlobalEditor" },
                    { 50L, "Add a staff member to the mandatory reporting list for this specific schema.", "Enroll Staff", "Form", 30L, "GlobalEditor" },
                    { 51L, "View the full details of a submission, including time logs, file attachments, achievements, and challenges.", "View Submission Modal", "Button", 31L, "GlobalViewer" },
                    { 52L, "Leave official feedback or a performance review directly on the user's report submission. The user will be able to see this comment.", "Manager Comments Box", "Form", 31L, "GlobalEditor" },
                    { 53L, "A prominent card highlighting a report you must submit today to avoid a late penalty.", "Pending Report Alert", "Card", 32L, "" },
                    { 54L, "Your historical log of previously submitted reports.", "My History", "Table", 32L, "" },
                    { 55L, "Write and file your report. Based on the admin template, this form may dynamically ask you to upload a file or fill out an hourly log table.", "Dynamic Submit Form", "Form", 33L, "" },
                    { 56L, "Check this box when creating a schema to force users to fill out an hourly breakdown of their activities.", "Require Time Log Flag", "Toggle", 29L, "GlobalEditor" },
                    { 57L, "Check this box to require users to upload a PDF or Excel document alongside their report.", "Require File Upload Flag", "Toggle", 29L, "GlobalEditor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisAlumniPosts_UserProfileId",
                table: "MisAlumniPosts",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisAlumniProfiles_UserProfileId",
                table: "MisAlumniProfiles",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisReportParticipants_MisReportSchemaId",
                table: "MisReportParticipants",
                column: "MisReportSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MisReportParticipants_UserProfileId",
                table: "MisReportParticipants",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisReportSubmissions_MisReportSchemaId",
                table: "MisReportSubmissions",
                column: "MisReportSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MisReportSubmissions_UserProfileId",
                table: "MisReportSubmissions",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisReportTimeLogs_MisReportSubmissionId",
                table: "MisReportTimeLogs",
                column: "MisReportSubmissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisAlumniPosts");

            migrationBuilder.DropTable(
                name: "MisAlumniProfiles");

            migrationBuilder.DropTable(
                name: "MisReportParticipants");

            migrationBuilder.DropTable(
                name: "MisReportTimeLogs");

            migrationBuilder.DropTable(
                name: "MisReportSubmissions");

            migrationBuilder.DropTable(
                name: "MisReportSchemas");

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 55L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 57L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DropColumn(
                name: "AlumniAlias",
                table: "MisSettings");
        }
    }
}
