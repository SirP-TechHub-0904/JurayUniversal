using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class projectup239949408 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "ProjectFeatures",
                newName: "ProjectFeatureStatus");

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskStatus",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UseFeaturesCost",
                table: "ProjectFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Gender",
            //    table: "AspNetUsers",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectTaskStatus",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "UseFeaturesCost",
                table: "ProjectFeatures");

            migrationBuilder.RenameColumn(
                name: "ProjectFeatureStatus",
                table: "ProjectFeatures",
                newName: "Percentage");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Gender",
            //    table: "AspNetUsers",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");
        }
    }
}
