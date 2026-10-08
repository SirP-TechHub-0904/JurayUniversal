using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    /// <inheritdoc />
    public partial class formcomunity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessPinHash",
                table: "MisCommunityFormSubmissions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "MisCommunityFormSubmissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LastActiveStep",
                table: "MisCommunityFormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSavedAtUtc",
                table: "MisCommunityFormSubmissions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessPinHash",
                table: "MisCommunityFormSubmissions");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "MisCommunityFormSubmissions");

            migrationBuilder.DropColumn(
                name: "LastActiveStep",
                table: "MisCommunityFormSubmissions");

            migrationBuilder.DropColumn(
                name: "LastSavedAtUtc",
                table: "MisCommunityFormSubmissions");
        }
    }
}
