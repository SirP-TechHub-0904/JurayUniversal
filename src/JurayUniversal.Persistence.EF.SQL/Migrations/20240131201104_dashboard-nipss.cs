using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class dashboardnipss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Configuration",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "DashboardCSS",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LayoutCSS",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LiveConfiguration",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LoginCSS",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "RedirectTohttps",
                table: "SuperSettings");

            migrationBuilder.RenameColumn(
                name: "RedirectTohttpswww",
                table: "SuperSettings",
                newName: "UserNipssArea");

            migrationBuilder.AddColumn<bool>(
                name: "EnableCV",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InternationalPassportExpiringDate",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InternationalPassportNumber",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PositionOrRank",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PreviousAppointment",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sponsor",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SponsorAddress",
                table: "FormSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genotype",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternationalPassportExpiringDate",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternationalPassportNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionOrRank",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousAppointment",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sponsor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SponsorAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseStatus = table.Column<int>(type: "int", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitleLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseSubThemes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSubThemes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubThemes_CourseId",
                table: "CourseSubThemes",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseSubThemes");

            migrationBuilder.DropTable(
                name: "TitleLists");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropColumn(
                name: "EnableCV",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "InternationalPassportExpiringDate",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "InternationalPassportNumber",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "PositionOrRank",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "PreviousAppointment",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "Sponsor",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "SponsorAddress",
                table: "FormSettings");

            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Genotype",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InternationalPassportExpiringDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InternationalPassportNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PositionOrRank",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreviousAppointment",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sponsor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SponsorAddress",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserNipssArea",
                table: "SuperSettings",
                newName: "RedirectTohttpswww");

            migrationBuilder.AddColumn<string>(
                name: "Configuration",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DashboardCSS",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayoutCSS",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveConfiguration",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginCSS",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RedirectTohttps",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
