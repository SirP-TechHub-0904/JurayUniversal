using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class devicedata200309 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageSections_TemplateTypes_TemplateTypeId",
                table: "PageSections");

            migrationBuilder.DropIndex(
                name: "IX_PageSections_TemplateTypeId",
                table: "PageSections");

            migrationBuilder.DropColumn(
                name: "TemplateTypeId",
                table: "PageSections");

            migrationBuilder.AddColumn<string>(
                name: "TemplateKey",
                table: "PageSections",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateKey",
                table: "PageSections");

            migrationBuilder.AddColumn<long>(
                name: "TemplateTypeId",
                table: "PageSections",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageSections_TemplateTypeId",
                table: "PageSections",
                column: "TemplateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageSections_TemplateTypes_TemplateTypeId",
                table: "PageSections",
                column: "TemplateTypeId",
                principalTable: "TemplateTypes",
                principalColumn: "Id");
        }
    }
}
