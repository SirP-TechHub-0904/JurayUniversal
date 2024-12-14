using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class devicedata2003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuperSettings_TemplateCategories_TemplateCategoryId",
                table: "SuperSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateTypes_TemplateCategories_TemplateCategoryId",
                table: "TemplateTypes");

            migrationBuilder.DropIndex(
                name: "IX_SuperSettings_TemplateCategoryId",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "TemplateCategoryId",
                table: "SuperSettings");

            migrationBuilder.AlterColumn<long>(
                name: "TemplateCategoryId",
                table: "TemplateTypes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "TrackKey",
                table: "TemplateTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueKey",
                table: "TemplateCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LoginTemplateKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateLayoutKey",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateTypes_TemplateCategories_TemplateCategoryId",
                table: "TemplateTypes",
                column: "TemplateCategoryId",
                principalTable: "TemplateCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateTypes_TemplateCategories_TemplateCategoryId",
                table: "TemplateTypes");

            migrationBuilder.DropColumn(
                name: "TrackKey",
                table: "TemplateTypes");

            migrationBuilder.DropColumn(
                name: "UniqueKey",
                table: "TemplateCategories");

            migrationBuilder.DropColumn(
                name: "LoginTemplateKey",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "TemplateLayoutKey",
                table: "SuperSettings");

            migrationBuilder.AlterColumn<long>(
                name: "TemplateCategoryId",
                table: "TemplateTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TemplateCategoryId",
                table: "SuperSettings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuperSettings_TemplateCategoryId",
                table: "SuperSettings",
                column: "TemplateCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuperSettings_TemplateCategories_TemplateCategoryId",
                table: "SuperSettings",
                column: "TemplateCategoryId",
                principalTable: "TemplateCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateTypes_TemplateCategories_TemplateCategoryId",
                table: "TemplateTypes",
                column: "TemplateCategoryId",
                principalTable: "TemplateCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
