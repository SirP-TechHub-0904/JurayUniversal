using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class supersettingupdate12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActivateDownloadOurAppFooter",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSubscribeFormOnFooter",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateWorkWithUsFooter",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppStoreImageKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppStoreImageUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppStoreLink",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDownloadOurAppFooter",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayStoreImageKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayStoreImageUrl",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayStoreLink",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleDownloadOurAppFooter",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleWorkWithUsFooter",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivateDownloadOurAppFooter",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSubscribeFormOnFooter",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateWorkWithUsFooter",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "AppStoreImageKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "AppStoreImageUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "AppStoreLink",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "DescriptionDownloadOurAppFooter",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PlayStoreImageKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PlayStoreImageUrl",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "PlayStoreLink",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "TitleDownloadOurAppFooter",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "TitleWorkWithUsFooter",
                table: "SuperSettings");
        }
    }
}
