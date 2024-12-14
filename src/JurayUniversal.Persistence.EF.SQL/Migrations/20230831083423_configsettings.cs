using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class configsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowMadeByXYZ",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DataConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginCSS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LayoutCSS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DashboardCSS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectTohttpswww = table.Column<bool>(type: "bit", nullable: false),
                    RedirectTohttps = table.Column<bool>(type: "bit", nullable: false),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataConfigs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataConfigs");

            migrationBuilder.DropColumn(
                name: "ShowMadeByXYZ",
                table: "SuperSettings");
        }
    }
}
