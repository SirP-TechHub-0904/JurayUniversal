using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class task091 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApproveTaskNote",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDateFinished",
                table: "XProjectTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Log",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TesterDateFinished",
                table: "XProjectTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TesterTaskNote",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserDateFinished",
                table: "XProjectTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserTaskNote",
                table: "XProjectTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveTaskNote",
                table: "XProjectTasks");

            migrationBuilder.DropColumn(
                name: "ApprovedDateFinished",
                table: "XProjectTasks");

            migrationBuilder.DropColumn(
                name: "Log",
                table: "XProjectTasks");

            migrationBuilder.DropColumn(
                name: "TesterDateFinished",
                table: "XProjectTasks");

            migrationBuilder.DropColumn(
                name: "TesterTaskNote",
                table: "XProjectTasks");

            migrationBuilder.DropColumn(
                name: "UserDateFinished",
                table: "XProjectTasks");

            migrationBuilder.DropColumn(
                name: "UserTaskNote",
                table: "XProjectTasks");
        }
    }
}
