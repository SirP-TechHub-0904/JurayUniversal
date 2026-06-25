using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { "Provides an analytical overview of active groups and memberships.", "Dashboard Metrics", "Card", 13L, "GlobalViewer" });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "Description", "ElementName", "ElementType", "MisManualPageId" },
                values: new object[] { "Creates a new group/committee and sets its title and description.", "Create Group", "Button", 14L });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "Description", "ElementName", "MisManualPageId", "RequiredRole" },
                values: new object[] { "Searches the user directory to add a member to this group with a specific role.", "Add Member", 15L, "GlobalEditor" });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "Description", "ElementName", "ElementType", "MisManualPageId" },
                values: new object[] { "Review any pending invitations to join a new group. You can choose to Accept or Decline.", "Pending Invites", "Card", 16L });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 32L, "A list of all groups you are an active member of. Click on any group to enter its private workspace.", "Active Groups", "List", 16L, "" },
                    { 33L, "Post a new message to the group's discussion board.", "New Discussion Post", "Form", 17L, "" },
                    { 34L, "Upload a document or file to share with the group.", "Upload File", "Button", 17L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[] { "Creates a new group/committee and sets its title and description.", "Create Group", "Button", 14L, "GlobalEditor" });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "Description", "ElementName", "ElementType", "MisManualPageId" },
                values: new object[] { "Searches the user directory to add a member to this group with a specific role.", "Add Member", "Form", 15L });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "Description", "ElementName", "MisManualPageId", "RequiredRole" },
                values: new object[] { "Post a new message to the group's discussion board.", "New Discussion Post", 17L, "" });

            migrationBuilder.UpdateData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "Description", "ElementName", "ElementType", "MisManualPageId" },
                values: new object[] { "Upload a document or file to share with the group.", "Upload File", "Button", 17L });
        }
    }
}
