using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class devicedata20030 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginNote",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginNoteFooter",
                table: "SuperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Show = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    MenuSortOrder = table.Column<int>(type: "int", nullable: false),
                    HomeSortFrom = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowInDropdown = table.Column<bool>(type: "bit", nullable: false),
                    ShowInHome = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    TitleBackgroundUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleBackgroundKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TiktokPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoutubeChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowEmailOneInTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowEmailOneInFooter = table.Column<bool>(type: "bit", nullable: false),
                    EmailTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowEmailTwoInTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowEmailTwoInFooter = table.Column<bool>(type: "bit", nullable: false),
                    EmailThree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowEmailThreeInTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowEmailThreeInFooter = table.Column<bool>(type: "bit", nullable: false),
                    PhoneOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowPhoneOneInTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowPhoneOneInFooter = table.Column<bool>(type: "bit", nullable: false),
                    PhoneTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowPhoneTwoInTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowPhoneTwoInFooter = table.Column<bool>(type: "bit", nullable: false),
                    PhoneThree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowPhoneThreeInTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowPhoneThreeInFooter = table.Column<bool>(type: "bit", nullable: false),
                    WorkingHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddFaqToHome = table.Column<bool>(type: "bit", nullable: false),
                    AddFaqToFooter = table.Column<bool>(type: "bit", nullable: false),
                    AddTestimonyToHome = table.Column<bool>(type: "bit", nullable: false),
                    AddTestimonyToFooter = table.Column<bool>(type: "bit", nullable: false),
                    AddPartnerToHome = table.Column<bool>(type: "bit", nullable: false),
                    DisableMainTopMenu = table.Column<bool>(type: "bit", nullable: false),
                    ActivateServicesInHome = table.Column<bool>(type: "bit", nullable: false),
                    ShowThreeService = table.Column<bool>(type: "bit", nullable: false),
                    ShowSixService = table.Column<bool>(type: "bit", nullable: false),
                    ActivateProductsInHome = table.Column<bool>(type: "bit", nullable: false),
                    ShowThreeProducts = table.Column<bool>(type: "bit", nullable: false),
                    ShowSixProducts = table.Column<bool>(type: "bit", nullable: false),
                    DefaultTitleBackgroundUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultTitleBackgroundKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultAttendanceStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefaultAttendanceCloseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyMinute = table.Column<int>(type: "int", nullable: false),
                    OntimeMinute = table.Column<int>(type: "int", nullable: false),
                    LateMinute = table.Column<int>(type: "int", nullable: false),
                    VeryLateMinute = table.Column<int>(type: "int", nullable: false),
                    DefaultAttendanceStartTimeSaturday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportCloseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobCardNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobCardTitleBackgroundUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobCardTitleBackgroundKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVideo = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Show = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Testimonies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebPages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    PageCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInMainTop = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMenuDropDown = table.Column<bool>(type: "bit", nullable: false),
                    ShowInMainMenu = table.Column<bool>(type: "bit", nullable: false),
                    ShowInFooter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebPages_PageCategories_PageCategoryId",
                        column: x => x.PageCategoryId,
                        principalTable: "PageCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeatures_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSamples",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSamples_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageSections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoutubeUrlLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiniTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInHome = table.Column<bool>(type: "bit", nullable: false),
                    Disable = table.Column<bool>(type: "bit", nullable: false),
                    HomePageSortOrder = table.Column<int>(type: "int", nullable: false),
                    HomeSortFrom = table.Column<int>(type: "int", nullable: false),
                    PageSortOrder = table.Column<int>(type: "int", nullable: false),
                    WebPageId = table.Column<long>(type: "bigint", nullable: true),
                    TemplateTypeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSections_TemplateTypes_TemplateTypeId",
                        column: x => x.TemplateTypeId,
                        principalTable: "TemplateTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageSections_WebPages_WebPageId",
                        column: x => x.WebPageId,
                        principalTable: "WebPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageSectionLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoreDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disable = table.Column<bool>(type: "bit", nullable: false),
                    PageSectionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSectionLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSectionLists_PageSections_PageSectionId",
                        column: x => x.PageSectionId,
                        principalTable: "PageSections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCategoryId",
                table: "Blogs",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSectionLists_PageSectionId",
                table: "PageSectionLists",
                column: "PageSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSections_TemplateTypeId",
                table: "PageSections",
                column: "TemplateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSections_WebPageId",
                table: "PageSections",
                column: "WebPageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_ProductId",
                table: "ProductFeatures",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSamples_ProductId",
                table: "ProductSamples",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WebPages_PageCategoryId",
                table: "WebPages",
                column: "PageCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "PageSectionLists");

            migrationBuilder.DropTable(
                name: "ProductFeatures");

            migrationBuilder.DropTable(
                name: "ProductSamples");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Testimonies");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "PageSections");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "WebPages");

            migrationBuilder.DropTable(
                name: "PageCategories");

            migrationBuilder.DropColumn(
                name: "LoginNote",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "LoginNoteFooter",
                table: "SuperSettings");
        }
    }
}
