using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class userupdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectMainId1",
                table: "ProjectUsers");

            migrationBuilder.RenameColumn(
                name: "ProjectMainId1",
                table: "ProjectUsers",
                newName: "ProjectTeamId1");

            migrationBuilder.RenameColumn(
                name: "ProjectMainId",
                table: "ProjectUsers",
                newName: "ProjectTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectUsers_ProjectMainId1",
                table: "ProjectUsers",
                newName: "IX_ProjectUsers_ProjectTeamId1");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AspNetUsers",
                newName: "State");

            migrationBuilder.AddColumn<long>(
                name: "ProjectTeamId2",
                table: "ProjectUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "ProjectUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskType",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LogoKey",
                table: "ProjectMains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "ProjectMains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CVFilePathKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CVFilePathUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactRelationship",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ethnicity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EyeColor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteBooks",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteFood",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteMovies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteMusic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteSports",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteTravelDestinations",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubProfile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HairColor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hobbies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInProfile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportFilePathKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportFilePathUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredLanguage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectMainId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectMainId1 = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateLeft = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTeams_ProjectMains_ProjectMainId1",
                        column: x => x.ProjectMainId1,
                        principalTable: "ProjectMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectTeamId2",
                table: "ProjectUsers",
                column: "ProjectTeamId2");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_RoleId",
                table: "ProjectUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeams_ProjectMainId1",
                table: "ProjectTeams",
                column: "ProjectMainId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_AspNetRoles_RoleId",
                table: "ProjectUsers",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectTeamId1",
                table: "ProjectUsers",
                column: "ProjectTeamId1",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectTeams_ProjectTeamId2",
                table: "ProjectUsers",
                column: "ProjectTeamId2",
                principalTable: "ProjectTeams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_AspNetRoles_RoleId",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectTeamId1",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectTeams_ProjectTeamId2",
                table: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "ProjectTeams");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_ProjectTeamId2",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_RoleId",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "ProjectTeamId2",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "TaskType",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "LogoKey",
                table: "ProjectMains");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "ProjectMains");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CVFilePathKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CVFilePathUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelationship",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ethnicity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EyeColor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteBooks",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteFood",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteMovies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteMusic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteSports",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteTravelDestinations",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubProfile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Hobbies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkedInProfile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PassportFilePathKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PassportFilePathUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreferredLanguage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProjectTeamId1",
                table: "ProjectUsers",
                newName: "ProjectMainId1");

            migrationBuilder.RenameColumn(
                name: "ProjectTeamId",
                table: "ProjectUsers",
                newName: "ProjectMainId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectUsers_ProjectTeamId1",
                table: "ProjectUsers",
                newName: "IX_ProjectUsers_ProjectMainId1");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "AspNetUsers",
                newName: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectMainId1",
                table: "ProjectUsers",
                column: "ProjectMainId1",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
