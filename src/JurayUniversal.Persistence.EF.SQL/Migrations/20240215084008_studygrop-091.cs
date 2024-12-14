using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class studygrop091 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StudyGroupId",
                table: "ParticipantLists",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudyGroupCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGroupCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyGroupCategoryId = table.Column<int>(type: "int", nullable: false),
                    StudyGroupCategoryId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudyGroups_StudyGroupCategory_StudyGroupCategoryId1",
                        column: x => x.StudyGroupCategoryId1,
                        principalTable: "StudyGroupCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantLists_StudyGroupId",
                table: "ParticipantLists",
                column: "StudyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_StudyGroupCategoryId1",
                table: "StudyGroups",
                column: "StudyGroupCategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_UserId",
                table: "StudyGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantLists_StudyGroups_StudyGroupId",
                table: "ParticipantLists",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantLists_StudyGroups_StudyGroupId",
                table: "ParticipantLists");

            migrationBuilder.DropTable(
                name: "StudyGroups");

            migrationBuilder.DropTable(
                name: "StudyGroupCategory");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantLists_StudyGroupId",
                table: "ParticipantLists");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "ParticipantLists");
        }
    }
}
