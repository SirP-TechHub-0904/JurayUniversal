using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class task0973 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GooglePort",
                table: "DataConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "GoogleSSLEnable",
                table: "DataConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GoogleSecurity",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleSenderEmail",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleServer",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GoogleUseDefaultCredentials",
                table: "DataConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GoogleUsername",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MailSender",
                table: "DataConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutlookPort",
                table: "DataConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OutlookSSLEnable",
                table: "DataConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OutlookSecurity",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutlookSenderEmail",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutlookServer",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OutlookUseDefaultCredentials",
                table: "DataConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OutlookUsername",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WebmailPort",
                table: "DataConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WebmailSSLEnable",
                table: "DataConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WebmailSecurity",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebmailSenderEmail",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebmailServer",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WebmailUseDefaultCredentials",
                table: "DataConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WebmailUsername",
                table: "DataConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GooglePort",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleSSLEnable",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleSecurity",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleSenderEmail",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleServer",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleUseDefaultCredentials",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "GoogleUsername",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "MailSender",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookPort",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookSSLEnable",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookSecurity",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookSenderEmail",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookServer",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookUseDefaultCredentials",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "OutlookUsername",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailPort",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailSSLEnable",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailSecurity",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailSenderEmail",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailServer",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailUseDefaultCredentials",
                table: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "WebmailUsername",
                table: "DataConfigs");
        }
    }
}
