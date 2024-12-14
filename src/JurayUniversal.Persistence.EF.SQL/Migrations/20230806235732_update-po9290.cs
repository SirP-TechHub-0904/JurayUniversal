using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class updatepo9290 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PortfolioTemplateId",
                table: "ProfilePortfolioSetting",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PortfolioTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePortfolioSetting_PortfolioTemplateId",
                table: "ProfilePortfolioSetting",
                column: "PortfolioTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePortfolioSetting_PortfolioTemplates_PortfolioTemplateId",
                table: "ProfilePortfolioSetting",
                column: "PortfolioTemplateId",
                principalTable: "PortfolioTemplates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePortfolioSetting_PortfolioTemplates_PortfolioTemplateId",
                table: "ProfilePortfolioSetting");

            migrationBuilder.DropTable(
                name: "PortfolioTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePortfolioSetting_PortfolioTemplateId",
                table: "ProfilePortfolioSetting");

            migrationBuilder.DropColumn(
                name: "PortfolioTemplateId",
                table: "ProfilePortfolioSetting");
        }
    }
}
