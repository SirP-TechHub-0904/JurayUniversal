using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class projectup23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFeatures_GeneralFeatures_GeneralFeatureId1",
                table: "ProjectFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFeatures_ProjectMains_ProjectMainId1",
                table: "ProjectFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFiles_ProjectMains_ProjectMainId1",
                table: "ProjectFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId1",
                table: "ProjectMains");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_ProjectMains_ProjectMainId1",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectTeamId1",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectTeams_ProjectTeamId2",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_ProjectTeamId1",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeams_ProjectMainId1",
                table: "ProjectTeams");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMains_ProjectCatgeoryId1",
                table: "ProjectMains");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFiles_ProjectMainId1",
                table: "ProjectFiles");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFeatures_GeneralFeatureId1",
                table: "ProjectFeatures");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFeatures_ProjectMainId1",
                table: "ProjectFeatures");

            migrationBuilder.DropColumn(
                name: "ProjectTeamId1",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "ProjectMainId1",
                table: "ProjectTeams");

            migrationBuilder.DropColumn(
                name: "ProjectCatgeoryId1",
                table: "ProjectMains");

            migrationBuilder.DropColumn(
                name: "ProjectMainId1",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "GeneralFeatureId1",
                table: "ProjectFeatures");

            migrationBuilder.DropColumn(
                name: "ProjectMainId1",
                table: "ProjectFeatures");

            migrationBuilder.RenameColumn(
                name: "ProjectTeamId2",
                table: "ProjectUsers",
                newName: "ProjectMainId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectUsers_ProjectTeamId2",
                table: "ProjectUsers",
                newName: "IX_ProjectUsers_ProjectMainId");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectTeamId",
                table: "ProjectUsers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectMainId",
                table: "ProjectTeams",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectCatgeoryId",
                table: "ProjectMains",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "ProjectMains",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectMainId",
                table: "ProjectFiles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectMainId",
                table: "ProjectFeatures",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "GeneralFeatureId",
                table: "ProjectFeatures",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectTeamId",
                table: "ProjectUsers",
                column: "ProjectTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeams_ProjectMainId",
                table: "ProjectTeams",
                column: "ProjectMainId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMains_ProjectCatgeoryId",
                table: "ProjectMains",
                column: "ProjectCatgeoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFiles_ProjectMainId",
                table: "ProjectFiles",
                column: "ProjectMainId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFeatures_GeneralFeatureId",
                table: "ProjectFeatures",
                column: "GeneralFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFeatures_ProjectMainId",
                table: "ProjectFeatures",
                column: "ProjectMainId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFeatures_GeneralFeatures_GeneralFeatureId",
                table: "ProjectFeatures",
                column: "GeneralFeatureId",
                principalTable: "GeneralFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFeatures_ProjectMains_ProjectMainId",
                table: "ProjectFeatures",
                column: "ProjectMainId",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFiles_ProjectMains_ProjectMainId",
                table: "ProjectFiles",
                column: "ProjectMainId",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId",
                table: "ProjectMains",
                column: "ProjectCatgeoryId",
                principalTable: "ProjectCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_ProjectMains_ProjectMainId",
                table: "ProjectTeams",
                column: "ProjectMainId",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectMainId",
                table: "ProjectUsers",
                column: "ProjectMainId",
                principalTable: "ProjectMains",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectTeams_ProjectTeamId",
                table: "ProjectUsers",
                column: "ProjectTeamId",
                principalTable: "ProjectTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFeatures_GeneralFeatures_GeneralFeatureId",
                table: "ProjectFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFeatures_ProjectMains_ProjectMainId",
                table: "ProjectFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFiles_ProjectMains_ProjectMainId",
                table: "ProjectFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId",
                table: "ProjectMains");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_ProjectMains_ProjectMainId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectMainId",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ProjectTeams_ProjectTeamId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_ProjectTeamId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeams_ProjectMainId",
                table: "ProjectTeams");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMains_ProjectCatgeoryId",
                table: "ProjectMains");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFiles_ProjectMainId",
                table: "ProjectFiles");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFeatures_GeneralFeatureId",
                table: "ProjectFeatures");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFeatures_ProjectMainId",
                table: "ProjectFeatures");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "ProjectMains");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProjectMainId",
                table: "ProjectUsers",
                newName: "ProjectTeamId2");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectUsers_ProjectMainId",
                table: "ProjectUsers",
                newName: "IX_ProjectUsers_ProjectTeamId2");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTeamId",
                table: "ProjectUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProjectTeamId1",
                table: "ProjectUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectMainId",
                table: "ProjectTeams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProjectMainId1",
                table: "ProjectTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectCatgeoryId",
                table: "ProjectMains",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProjectCatgeoryId1",
                table: "ProjectMains",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectMainId",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProjectMainId1",
                table: "ProjectFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectMainId",
                table: "ProjectFeatures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "GeneralFeatureId",
                table: "ProjectFeatures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "GeneralFeatureId1",
                table: "ProjectFeatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProjectMainId1",
                table: "ProjectFeatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectTeamId1",
                table: "ProjectUsers",
                column: "ProjectTeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeams_ProjectMainId1",
                table: "ProjectTeams",
                column: "ProjectMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMains_ProjectCatgeoryId1",
                table: "ProjectMains",
                column: "ProjectCatgeoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFiles_ProjectMainId1",
                table: "ProjectFiles",
                column: "ProjectMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFeatures_GeneralFeatureId1",
                table: "ProjectFeatures",
                column: "GeneralFeatureId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFeatures_ProjectMainId1",
                table: "ProjectFeatures",
                column: "ProjectMainId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFeatures_GeneralFeatures_GeneralFeatureId1",
                table: "ProjectFeatures",
                column: "GeneralFeatureId1",
                principalTable: "GeneralFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFeatures_ProjectMains_ProjectMainId1",
                table: "ProjectFeatures",
                column: "ProjectMainId1",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFiles_ProjectMains_ProjectMainId1",
                table: "ProjectFiles",
                column: "ProjectMainId1",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMains_ProjectCategories_ProjectCatgeoryId1",
                table: "ProjectMains",
                column: "ProjectCatgeoryId1",
                principalTable: "ProjectCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_ProjectMains_ProjectMainId1",
                table: "ProjectTeams",
                column: "ProjectMainId1",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectMains_ProjectTeamId1",
                table: "ProjectUsers",
                column: "ProjectTeamId1",
                principalTable: "ProjectMains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ProjectTeams_ProjectTeamId2",
                table: "ProjectUsers",
                column: "ProjectTeamId2",
                principalTable: "ProjectTeams",
                principalColumn: "Id");
        }
    }
}
