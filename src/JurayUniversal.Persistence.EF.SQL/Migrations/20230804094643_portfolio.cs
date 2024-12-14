using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class portfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ethnicity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EyeColor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteBooks",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteFood",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteMovies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteMusic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteSports",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteTravelDestinations",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubProfile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Hobbies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "AspNetUsers",
                newName: "WebsiteAddress");

            migrationBuilder.RenameColumn(
                name: "Religion",
                table: "AspNetUsers",
                newName: "OfficeEmail");

            migrationBuilder.RenameColumn(
                name: "PreferredLanguage",
                table: "AspNetUsers",
                newName: "ContactMailEmail");

            migrationBuilder.RenameColumn(
                name: "LinkedInProfile",
                table: "AspNetUsers",
                newName: "Biography");

            migrationBuilder.RenameColumn(
                name: "Languages",
                table: "AspNetUsers",
                newName: "AltPhoneNumber");

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProfilePortfolio",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerifyTokenFolio",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ClientRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DateOfBirthStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdditionalProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProfiles_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioContactUses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioContactUses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioContactUses_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProfileCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivacyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Authorize = table.Column<bool>(type: "bit", nullable: false),
                    TokenKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileCategories_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePortfolios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePortfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePortfolios_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePortfolioSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DefaultWebLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpgradeWebLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableContactUs = table.Column<bool>(type: "bit", nullable: false),
                    DisableAccount = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePortfolioSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePortfolioSetting_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProfileCategoryLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfileCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Honours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currently = table.Column<bool>(type: "bit", nullable: false),
                    PrivacyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Authorize = table.Column<bool>(type: "bit", nullable: false),
                    TokenKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileCategoryLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileCategoryLists_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProfileCategoryLists_ProfileCategories_ProfileCategoryId",
                        column: x => x.ProfileCategoryId,
                        principalTable: "ProfileCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProfiles_ProfileId",
                table: "AdditionalProfiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioContactUses_ProfileId",
                table: "PortfolioContactUses",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileCategories_ProfileId",
                table: "ProfileCategories",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileCategoryLists_ProfileCategoryId",
                table: "ProfileCategoryLists",
                column: "ProfileCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileCategoryLists_ProfileId",
                table: "ProfileCategoryLists",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePortfolios_ProfileId",
                table: "ProfilePortfolios",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePortfolioSetting_ProfileId",
                table: "ProfilePortfolioSetting",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalProfiles");

            migrationBuilder.DropTable(
                name: "PortfolioContactUses");

            migrationBuilder.DropTable(
                name: "ProfileCategoryLists");

            migrationBuilder.DropTable(
                name: "ProfilePortfolios");

            migrationBuilder.DropTable(
                name: "ProfilePortfolioSetting");

            migrationBuilder.DropTable(
                name: "ProfileCategories");

            migrationBuilder.DropColumn(
                name: "ActivateProfilePortfolio",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "VerifyTokenFolio",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ClientRequest");

            migrationBuilder.DropColumn(
                name: "DateOfBirthStatus",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "WebsiteAddress",
                table: "AspNetUsers",
                newName: "Skills");

            migrationBuilder.RenameColumn(
                name: "OfficeEmail",
                table: "AspNetUsers",
                newName: "Religion");

            migrationBuilder.RenameColumn(
                name: "ContactMailEmail",
                table: "AspNetUsers",
                newName: "PreferredLanguage");

            migrationBuilder.RenameColumn(
                name: "Biography",
                table: "AspNetUsers",
                newName: "LinkedInProfile");

            migrationBuilder.RenameColumn(
                name: "AltPhoneNumber",
                table: "AspNetUsers",
                newName: "Languages");

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ethnicity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EyeColor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteBooks",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteFood",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteMovies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteMusic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteSports",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteTravelDestinations",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubProfile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HairColor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hobbies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }
    }
}
