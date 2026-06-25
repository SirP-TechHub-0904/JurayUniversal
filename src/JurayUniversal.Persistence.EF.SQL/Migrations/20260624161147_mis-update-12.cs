using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupAlias",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MisGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisGroupFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisGroupId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisGroupFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisGroupFiles_AspNetUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisGroupFiles_MisGroups_MisGroupId",
                        column: x => x.MisGroupId,
                        principalTable: "MisGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisGroupMembers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisGroupId = table.Column<long>(type: "bigint", nullable: false),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RolePosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisGroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisGroupMembers_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisGroupMembers_MisGroups_MisGroupId",
                        column: x => x.MisGroupId,
                        principalTable: "MisGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MisGroupPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisGroupId = table.Column<long>(type: "bigint", nullable: false),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisGroupPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisGroupPosts_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MisGroupPosts_MisGroups_MisGroupId",
                        column: x => x.MisGroupId,
                        principalTable: "MisGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 2L, "MisGroups", "This module provides isolated, private workspaces where specific teams can discuss, collaborate, and share files. It is highly flexible and can be adapted for Committees, Chapters, Squads, or any organizational sub-group.", "Committees & Groups Guide" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 13L, 2L, "The high-level command center for groups. Shows total groups, active groups, and total memberships.", "Groups Overview", "/MisGroups/Dashboard/Index" },
                    { 14L, 2L, "The comprehensive list of all groups in the organization where you can add new ones or manage existing ones.", "Group Directory", "/MisGroups/Dashboard/List" },
                    { 15L, 2L, "The administration panel for a specific group where you can add or remove members and set their roles.", "Manage Group Members", "/MisGroups/Dashboard/Detail" },
                    { 16L, 2L, "Your personal dashboard showing only the groups you are a member of.", "My Groups", "/MisGroups/Platform/Index" },
                    { 17L, 2L, "The private collaboration space for a group. You can post messages, share files, and view other members.", "Group Workspace", "/MisGroups/Platform/Workspace" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 28L, "Creates a new group/committee and sets its title and description.", "Create Group", "Button", 14L, "GlobalEditor" },
                    { 29L, "Searches the user directory to add a member to this group with a specific role.", "Add Member", "Form", 15L, "GlobalEditor" },
                    { 30L, "Post a new message to the group's discussion board.", "New Discussion Post", "Form", 17L, "" },
                    { 31L, "Upload a document or file to share with the group.", "Upload File", "Button", 17L, "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisGroupFiles_MisGroupId",
                table: "MisGroupFiles",
                column: "MisGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGroupFiles_UploadedByUserId",
                table: "MisGroupFiles",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGroupMembers_MisGroupId",
                table: "MisGroupMembers",
                column: "MisGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGroupMembers_UserProfileId",
                table: "MisGroupMembers",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGroupPosts_MisGroupId",
                table: "MisGroupPosts",
                column: "MisGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MisGroupPosts_UserProfileId",
                table: "MisGroupPosts",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisGroupFiles");

            migrationBuilder.DropTable(
                name: "MisGroupMembers");

            migrationBuilder.DropTable(
                name: "MisGroupPosts");

            migrationBuilder.DropTable(
                name: "MisGroups");

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DropColumn(
                name: "GroupAlias",
                table: "MisSettings");
        }
    }
}
