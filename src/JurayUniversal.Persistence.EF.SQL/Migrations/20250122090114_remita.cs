using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class remita : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RemitaApproved",
                table: "ParticipantLists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RemitaKey",
                table: "ParticipantLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemitaUrl",
                table: "ParticipantLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemitaApproved",
                table: "ParticipantLists");

            migrationBuilder.DropColumn(
                name: "RemitaKey",
                table: "ParticipantLists");

            migrationBuilder.DropColumn(
                name: "RemitaUrl",
                table: "ParticipantLists");
        }
    }
}
