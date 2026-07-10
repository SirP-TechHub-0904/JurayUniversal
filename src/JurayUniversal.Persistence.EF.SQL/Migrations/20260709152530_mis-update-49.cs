using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailMaxRetries",
                table: "MisSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnableZeptoMail",
                table: "MisSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ZeptoMailSenderEmail",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZeptoMailSenderName",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MisNewsletterEmailLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MailBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterEmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterEmailLogs_MisNewsletterCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "MisNewsletterCampaigns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterEmailLogs_CampaignId",
                table: "MisNewsletterEmailLogs",
                column: "CampaignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisNewsletterEmailLogs");

            migrationBuilder.DropColumn(
                name: "EmailMaxRetries",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "EnableZeptoMail",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "ZeptoMailSenderEmail",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "ZeptoMailSenderName",
                table: "MisSettings");
        }
    }
}
