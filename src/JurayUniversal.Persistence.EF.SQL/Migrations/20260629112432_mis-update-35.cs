using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.AddColumn<int>(
                name: "ActiveAiProvider",
                table: "MisSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnthropicApiKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeminiApiKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GlobalNewsletterQuota",
                table: "MisSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "GlobalNewsletterQuotaExpiry",
                table: "MisSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpenAIApiKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SmsCreditBalance",
                table: "MisSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ZeptoMailApiKey",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZeptoMailSendMailId",
                table: "MisSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MisElections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectionType = table.Column<int>(type: "int", nullable: false),
                    VotingStatus = table.Column<int>(type: "int", nullable: false),
                    ShowResults = table.Column<bool>(type: "bit", nullable: false),
                    EnableCandidateImages = table.Column<bool>(type: "bit", nullable: false),
                    ElectionBannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectionBannerKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisElections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisEquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisEquipmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterCampaigns",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRecipients = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterCampaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterCampaigns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterWallets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailQuotaBalance = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterWallets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisPhoneBooks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisPhoneBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisPhoneBooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisProjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisSmsCampaigns",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostIncurred = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRecipients = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisSmsCampaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisSmsCampaigns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisSmsSenderIds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisSmsSenderIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MisWallets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisWallets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisElectionVoters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisElectionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsAccredited = table.Column<bool>(type: "bit", nullable: false),
                    HasVoted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisElectionVoters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisElectionVoters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MisElectionVoters_MisElections_MisElectionId",
                        column: x => x.MisElectionId,
                        principalTable: "MisElections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisVoteCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisElectionId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisVoteCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisVoteCategories_MisElections_MisElectionId",
                        column: x => x.MisElectionId,
                        principalTable: "MisElections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisEquipments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MisEquipmentCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalvageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LifespanMonths = table.Column<int>(type: "int", nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdditionalDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisEquipments_AspNetUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisEquipments_MisEquipmentCategories_MisEquipmentCategoryId",
                        column: x => x.MisEquipmentCategoryId,
                        principalTable: "MisEquipmentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterRecipients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisNewsletterCampaignId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrackingGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterRecipients_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MisNewsletterRecipients_MisNewsletterCampaigns_MisNewsletterCampaignId",
                        column: x => x.MisNewsletterCampaignId,
                        principalTable: "MisNewsletterCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisNewsletterListId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterContacts_MisNewsletterLists_MisNewsletterListId",
                        column: x => x.MisNewsletterListId,
                        principalTable: "MisNewsletterLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisNewsletterWalletTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisNewsletterWalletId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisNewsletterWalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisNewsletterWalletTransactions_MisNewsletterWallets_MisNewsletterWalletId",
                        column: x => x.MisNewsletterWalletId,
                        principalTable: "MisNewsletterWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisPhoneBookId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisContacts_MisPhoneBooks_MisPhoneBookId",
                        column: x => x.MisPhoneBookId,
                        principalTable: "MisPhoneBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisProjectSprints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisProjectId = table.Column<long>(type: "bigint", nullable: false),
                    SprintName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisProjectSprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisProjectSprints_MisProjects_MisProjectId",
                        column: x => x.MisProjectId,
                        principalTable: "MisProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisSmsRecipients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisSmsCampaignId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisSmsRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisSmsRecipients_MisSmsCampaigns_MisSmsCampaignId",
                        column: x => x.MisSmsCampaignId,
                        principalTable: "MisSmsCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisWalletTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisWalletId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisWalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisWalletTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisWalletTransactions_MisWallets_MisWalletId",
                        column: x => x.MisWalletId,
                        principalTable: "MisWallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisCandidates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisVoteCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CandidateImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidateImageKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manifesto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisCandidates_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisCandidates_MisVoteCategories_MisVoteCategoryId",
                        column: x => x.MisVoteCategoryId,
                        principalTable: "MisVoteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisMaintenanceLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisEquipmentId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisMaintenanceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisMaintenanceLogs_MisEquipments_MisEquipmentId",
                        column: x => x.MisEquipmentId,
                        principalTable: "MisEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MisProjectTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisProjectId = table.Column<long>(type: "bigint", nullable: false),
                    MisProjectSprintId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    StoryPoints = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisProjectTasks_AspNetUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MisProjectTasks_MisProjects_MisProjectId",
                        column: x => x.MisProjectId,
                        principalTable: "MisProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MisProjectTasks_MisProjectSprints_MisProjectSprintId",
                        column: x => x.MisProjectSprintId,
                        principalTable: "MisProjectSprints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MisUserVotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MisVoteCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    MisCandidateId = table.Column<long>(type: "bigint", nullable: false),
                    EncryptCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCast = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisUserVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MisUserVotes_MisCandidates_MisCandidateId",
                        column: x => x.MisCandidateId,
                        principalTable: "MisCandidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MisUserVotes_MisVoteCategories_MisVoteCategoryId",
                        column: x => x.MisVoteCategoryId,
                        principalTable: "MisVoteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 93L, 13L, "The public portal where candidates enter their Tracking ID to view the status of their application.", "Track Application", "/MisRecruitment/Platform/Track" },
                    { 110L, 9L, "Create a new pricing plan specifying the email quota and duration for the Newsletter module.", "Create Pricing Plan", "/MisManagement/Plans/Create" },
                    { 111L, 9L, "Update the price, quota, or duration of an existing Newsletter pricing plan.", "Edit Pricing Plan", "/MisManagement/Plans/Edit" },
                    { 125L, 3L, "Portal to join the alumni network.", "Join Alumni", "/MisAlumni/Platform/Join" },
                    { 126L, 6L, "List of overdue and unpaid financial obligations.", "Overdue Payments", "/MisFinances/Dashboard/Overdue" },
                    { 127L, 11L, "Historical overview of completed grants and milestones.", "Grant History", "/MisGrants/Dashboard/History" },
                    { 128L, 11L, "Detailed information regarding a specific grant assigned to you.", "My Grant Details", "/MisGrants/Platform/Details" },
                    { 129L, 11L, "Overview of grants you are currently managing or tracking.", "My Grants", "/MisGrants/Platform/Index" },
                    { 130L, 9L, "List of all active features, their billing status, and lifecycle management.", "Features Directory", "/MisManagement/Index" },
                    { 131L, 9L, "Manage and view features that have been suspended due to billing or compliance issues.", "Suspended Features", "/MisManagement/SuspendedFeature" },
                    { 132L, 9L, "Directory of all available pricing plans across all platform modules.", "Pricing Plans Directory", "/MisManagement/Plans/Index" },
                    { 133L, 1L, "Modify an existing user's profile information.", "Edit User", "/MisUserManagement/Dashboard/Edit" },
                    { 134L, 1L, "Administrative overview of all manual sections and configurations.", "User Manual Dashboard", "/MisUserManagement/Dashboard/UserManual" },
                    { 135L, 1L, "Your personal user portal for managing profile and metrics.", "My Portal", "/MisUserManagement/Platform/Index" },
                    { 200L, 8L, "The central dashboard giving an overview of all system modules and global metrics.", "Console Home", "/MIS/Dashboard/Index" }
                });

            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[,]
                {
                    { 14L, "MisFleet", "Complete asset ledger, real-time depreciation calculation, maintenance logging, and checkout/checkin workflows.", "Fleet & Equipment Management Guide" },
                    { 15L, "MisVoting", "Comprehensive e-voting engine supporting both Open (pool-based) and Closed (accreditation-based) elections with real-time tabulation.", "Elections & E-Voting Guide" },
                    { 16L, "MisProjects", "Agile Project Tracking module featuring interactive Scrum/Kanban boards, Sprint planning, Backlog management, and Work Item tracking.", "Agile Projects & Kanban Guide" },
                    { 17L, "MisSmartAssist", "Bring-Your-Own-Key (BYOK) AI Integration allowing organizations to connect their own OpenAI, Google Gemini, or Anthropic API keys to power intelligent chat features.", "SmartAssist AI Integration Guide" },
                    { 18L, "MisNewsletter", "A complete drag-and-drop newsletter builder, mailing list manager, and global quota distribution system.", "Newsletter Campaign Guide" },
                    { 19L, "MisMassComm", "Complete global SMS engine for sending bulk messages and managing phone book contacts.", "Mass Comm SMS Guide" }
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 94L, 14L, "Analytics tracking total asset value, lifetime maintenance costs, and depreciation.", "Fleet Analytics Dashboard", "/MisFleet/Dashboard/Equipment/Index" },
                    { 95L, 14L, "Detailed view of an individual equipment's details, lifecycle, and depreciation.", "Equipment Details", "/MisFleet/Dashboard/Equipment/Details" },
                    { 96L, 14L, "Form to add a new asset to the fleet inventory.", "Add Equipment", "/MisFleet/Dashboard/Equipment/Create" },
                    { 97L, 14L, "Manage the categories that fleet assets can belong to.", "Fleet Categories", "/MisFleet/Dashboard/Categories/Index" },
                    { 98L, 15L, "Voting command center showing total elections, active voters, and turnout charts.", "Elections Analytics", "/MisVoting/Dashboard/Elections/Index" },
                    { 99L, 15L, "Detailed configuration page for an election, where candidates are listed and settings defined.", "Election Details", "/MisVoting/Dashboard/Elections/Details" },
                    { 100L, 15L, "The accreditation checklist to authorize voters for a Closed Election.", "Voter Accreditation", "/MisVoting/Dashboard/Elections/Accreditation" },
                    { 101L, 15L, "User portal displaying all available elections they can vote in.", "Voter Dashboard", "/MisVoting/Dashboard/Index" },
                    { 102L, 15L, "Real-time live results tabulation for an ongoing or concluded election.", "Live Results", "/MisVoting/Dashboard/Results" },
                    { 103L, 15L, "The secure voting booth where users cast their ballot.", "Voting Booth", "/MisVoting/Platform/Vote" },
                    { 104L, 16L, "Global command center for Agile projects, sprint velocities, and task tracking.", "Agile Analytics Dashboard", "/MisProjects/Dashboard/Index" },
                    { 105L, 16L, "Manage all software and general Agile projects in the organization.", "Agile Projects", "/MisProjects/Projects/Index" },
                    { 106L, 16L, "Plan upcoming sprints, manage your backlog, and assign work items.", "Sprint Planning & Backlog", "/MisProjects/Backlog/Index" },
                    { 107L, 16L, "Interactive drag-and-drop Kanban board for managing the active Sprint.", "Active Sprint Board", "/MisProjects/Board/Index" },
                    { 108L, 17L, "Secure admin panel to select the active AI provider and securely store BYOK API Keys.", "AI Provider Settings", "/MisSmartAssist/Dashboard/Config" },
                    { 109L, 17L, "The primary interface for conversing with the configured AI companion.", "SmartAssist Chat", "/MisSmartAssist/Platform/Chat" },
                    { 112L, 18L, "View the company's global email quota and allocate specific amounts to individual staff members.", "Global Wallet & Sharing", "/MisNewsletter/Dashboard/Wallet/Index" },
                    { 113L, 18L, "Directory of all administrative mailing lists.", "Admin Mailing Lists", "/MisNewsletter/Dashboard/Lists/Index" },
                    { 114L, 18L, "Create a new administrative mailing list.", "Create Admin List", "/MisNewsletter/Dashboard/Lists/Create" },
                    { 115L, 18L, "Manage subscribers for this list. You can add them individually or import a CSV file.", "List Details & CSV Import", "/MisNewsletter/Dashboard/Lists/Details" },
                    { 116L, 18L, "Directory of all scheduled, pending, or completed email campaigns.", "Admin Campaigns", "/MisNewsletter/Dashboard/Campaigns/Index" },
                    { 117L, 18L, "Design your email campaign using the drag-and-drop editor and schedule its delivery.", "Campaign Builder (Admin)", "/MisNewsletter/Dashboard/Campaigns/Create" },
                    { 118L, 18L, "View your personal email quota balance and browse available plans to purchase more.", "My Newsletter Wallet", "/MisNewsletter/Platform/Dashboard/Index" },
                    { 119L, 18L, "Review the selected pricing plan details and proceed to secure payment.", "Purchase Plan Checkout", "/MisNewsletter/Platform/Dashboard/BuyPlan" },
                    { 120L, 18L, "Directory of your personal mailing lists.", "My Mailing Lists", "/MisNewsletter/Platform/Lists/Index" },
                    { 121L, 18L, "Create a new personal mailing list.", "Create Personal List", "/MisNewsletter/Platform/Lists/Create" },
                    { 122L, 18L, "Manage your personal subscribers and upload CSV contact lists.", "Personal List Details", "/MisNewsletter/Platform/Lists/Details" },
                    { 123L, 18L, "Directory of your personal email campaigns.", "My Campaigns", "/MisNewsletter/Platform/Campaigns/Index" },
                    { 124L, 18L, "Design and schedule your personal email campaign.", "Campaign Builder (Personal)", "/MisNewsletter/Platform/Campaigns/Create" },
                    { 136L, 19L, "Analytics dashboard showing SMS delivery rates and campaign stats.", "Mass Comm Analytics", "/MisMassComm/Dashboard/Index" },
                    { 137L, 19L, "Global command center for sending bulk SMS campaigns.", "Admin SMS Campaigns", "/MisMassComm/Dashboard/Campaigns/Index" },
                    { 138L, 19L, "Compose and schedule an SMS campaign to your contacts.", "Create SMS Campaign", "/MisMassComm/Dashboard/Campaigns/Create" },
                    { 139L, 19L, "Global directory of all phone books.", "Admin Phone Books", "/MisMassComm/Dashboard/PhoneBooks/Index" },
                    { 140L, 19L, "Create a new administrative phone book.", "Create Phone Book", "/MisMassComm/Dashboard/PhoneBooks/Create" },
                    { 141L, 19L, "View all contacts within a specific phone book.", "Phone Book Contacts", "/MisMassComm/Dashboard/PhoneBooks/Contacts" },
                    { 142L, 19L, "Upload a CSV of phone numbers into the phone book.", "Import Contacts", "/MisMassComm/Dashboard/PhoneBooks/Import" },
                    { 143L, 19L, "Your personal SMS platform dashboard.", "My SMS Platform", "/MisMassComm/Platform/Index" },
                    { 144L, 19L, "Your personal scheduled and sent SMS campaigns.", "My Campaigns", "/MisMassComm/Platform/Campaigns/Index" },
                    { 145L, 19L, "Compose a new personal SMS campaign.", "Create Personal Campaign", "/MisMassComm/Platform/Campaigns/Create" },
                    { 146L, 19L, "Your personal directory of phone books.", "My Phone Books", "/MisMassComm/Platform/PhoneBooks/Index" },
                    { 147L, 19L, "Create a new personal phone book.", "Create Phone Book", "/MisMassComm/Platform/PhoneBooks/Create" },
                    { 148L, 19L, "View and manage contacts in your phone book.", "My Contacts", "/MisMassComm/Platform/PhoneBooks/Contacts" }
                });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 149L, 19L, "Bulk import numbers via CSV into your personal phone book.", "Import Personal Contacts", "/MisMassComm/Platform/PhoneBooks/Import" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 150L, 19L, "Overview of your SMS wallet and transaction history.", "SMS Wallet", "/MisMassComm/Platform/Wallet/Index" });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 147L, "Top-level KPI tracking active Sprints, Total Tasks, and Items Done.", "Agile Metrics", "Card", 104L, "GlobalViewer" },
                    { 148L, "Donut chart visualizing the distribution of tasks across statuses.", "Global Task Status", "Chart", 104L, "GlobalViewer" },
                    { 149L, "Create a new top-level software or general tracking project.", "New Project", "Button", 105L, "GlobalEditor" },
                    { 150L, "Quick jump to the active Kanban board for a project.", "Active Board", "Button", 105L, "GlobalViewer" },
                    { 151L, "Unassigned tasks waiting to be picked up in future sprints.", "Unassigned Backlog", "List", 106L, "GlobalViewer" },
                    { 152L, "Time-boxed iterations for work execution.", "Sprints", "List", 106L, "GlobalViewer" },
                    { 153L, "Activate a planned sprint to transition it to the Board.", "Start Sprint", "Button", 106L, "GlobalEditor" },
                    { 154L, "Drag a task from 'To Do' into 'In Progress'. The system auto-saves the new status.", "Kanban Columns", "Drag-and-Drop Area", 107L, "GlobalEditor" },
                    { 155L, "Shows the Type (Bug/Story/Task), Points, and Assigned user for the item.", "Task Card", "Card", 107L, "GlobalViewer" },
                    { 156L, "Select which AI Provider (Gemini, OpenAI, Anthropic) will power the chat.", "Active Provider Dropdown", "Form Field", 108L, "SystemAdmin" },
                    { 157L, "Save the API Keys to the database. These keys are never exposed directly to users.", "Save Configuration", "Button", 108L, "SystemAdmin" },
                    { 158L, "Type your prompt or question here to speak to the AI.", "Chat Input", "Text Input", 109L, "GlobalViewer" },
                    { 159L, "Send your message to the active AI provider.", "Send Message", "Button", 109L, "GlobalViewer" },
                    { 160L, "Transfers email quota directly from the global company pool to a specific user's wallet.", "Share Quota Button", "Button", 112L, "GlobalEditor" },
                    { 161L, "Drag components from the right sidebar onto the canvas to design your email.", "GrapesJS Editor", "Drag-and-Drop Area", 117L, "GlobalEditor" },
                    { 162L, "Choose a pricing package and securely checkout via Paystack to fund your wallet.", "Buy Plan Button", "Button", 119L, "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MisCandidates_MisVoteCategoryId",
                table: "MisCandidates",
                column: "MisVoteCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MisCandidates_ProfileId",
                table: "MisCandidates",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MisContacts_MisPhoneBookId",
                table: "MisContacts",
                column: "MisPhoneBookId");

            migrationBuilder.CreateIndex(
                name: "IX_MisElectionVoters_MisElectionId",
                table: "MisElectionVoters",
                column: "MisElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MisElectionVoters_UserId",
                table: "MisElectionVoters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisEquipments_AssignedToUserId",
                table: "MisEquipments",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisEquipments_MisEquipmentCategoryId",
                table: "MisEquipments",
                column: "MisEquipmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MisMaintenanceLogs_MisEquipmentId",
                table: "MisMaintenanceLogs",
                column: "MisEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterCampaigns_UserId",
                table: "MisNewsletterCampaigns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterContacts_MisNewsletterListId",
                table: "MisNewsletterContacts",
                column: "MisNewsletterListId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterLists_UserId",
                table: "MisNewsletterLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterRecipients_MisNewsletterCampaignId",
                table: "MisNewsletterRecipients",
                column: "MisNewsletterCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterRecipients_UserId",
                table: "MisNewsletterRecipients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterWallets_UserId",
                table: "MisNewsletterWallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisNewsletterWalletTransactions_MisNewsletterWalletId",
                table: "MisNewsletterWalletTransactions",
                column: "MisNewsletterWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MisPhoneBooks_UserId",
                table: "MisPhoneBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisProjectSprints_MisProjectId",
                table: "MisProjectSprints",
                column: "MisProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MisProjectTasks_AssignedToUserId",
                table: "MisProjectTasks",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisProjectTasks_MisProjectId",
                table: "MisProjectTasks",
                column: "MisProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MisProjectTasks_MisProjectSprintId",
                table: "MisProjectTasks",
                column: "MisProjectSprintId");

            migrationBuilder.CreateIndex(
                name: "IX_MisSmsCampaigns_UserId",
                table: "MisSmsCampaigns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisSmsRecipients_MisSmsCampaignId",
                table: "MisSmsRecipients",
                column: "MisSmsCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_MisUserVotes_MisCandidateId",
                table: "MisUserVotes",
                column: "MisCandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_MisUserVotes_MisVoteCategoryId",
                table: "MisUserVotes",
                column: "MisVoteCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MisVoteCategories_MisElectionId",
                table: "MisVoteCategories",
                column: "MisElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MisWallets_UserId",
                table: "MisWallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MisWalletTransactions_MisWalletId",
                table: "MisWalletTransactions",
                column: "MisWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MisWalletTransactions_UserId",
                table: "MisWalletTransactions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisContacts");

            migrationBuilder.DropTable(
                name: "MisElectionVoters");

            migrationBuilder.DropTable(
                name: "MisMaintenanceLogs");

            migrationBuilder.DropTable(
                name: "MisNewsletterContacts");

            migrationBuilder.DropTable(
                name: "MisNewsletterPlans");

            migrationBuilder.DropTable(
                name: "MisNewsletterRecipients");

            migrationBuilder.DropTable(
                name: "MisNewsletterWalletTransactions");

            migrationBuilder.DropTable(
                name: "MisProjectTasks");

            migrationBuilder.DropTable(
                name: "MisSmsRecipients");

            migrationBuilder.DropTable(
                name: "MisSmsSenderIds");

            migrationBuilder.DropTable(
                name: "MisUserVotes");

            migrationBuilder.DropTable(
                name: "MisWalletTransactions");

            migrationBuilder.DropTable(
                name: "MisPhoneBooks");

            migrationBuilder.DropTable(
                name: "MisEquipments");

            migrationBuilder.DropTable(
                name: "MisNewsletterLists");

            migrationBuilder.DropTable(
                name: "MisNewsletterCampaigns");

            migrationBuilder.DropTable(
                name: "MisNewsletterWallets");

            migrationBuilder.DropTable(
                name: "MisProjectSprints");

            migrationBuilder.DropTable(
                name: "MisSmsCampaigns");

            migrationBuilder.DropTable(
                name: "MisCandidates");

            migrationBuilder.DropTable(
                name: "MisWallets");

            migrationBuilder.DropTable(
                name: "MisEquipmentCategories");

            migrationBuilder.DropTable(
                name: "MisProjects");

            migrationBuilder.DropTable(
                name: "MisVoteCategories");

            migrationBuilder.DropTable(
                name: "MisElections");

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 147L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 148L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 149L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 150L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 151L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 152L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 153L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 154L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 155L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 156L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 157L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 158L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 159L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 160L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 161L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 162L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 94L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 96L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 97L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 98L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 99L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 110L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 111L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 113L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 114L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 115L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 116L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 118L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 120L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 121L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 122L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 123L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 124L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 125L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 126L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 127L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 128L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 129L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 130L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 131L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 132L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 133L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 134L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 135L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 136L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 137L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 138L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 139L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 140L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 141L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 142L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 143L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 144L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 145L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 146L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 147L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 148L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 149L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 150L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 200L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 106L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 107L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 108L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 109L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 112L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 117L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 119L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DropColumn(
                name: "ActiveAiProvider",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "AnthropicApiKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "GeminiApiKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "GlobalNewsletterQuota",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "GlobalNewsletterQuotaExpiry",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "OpenAIApiKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "SmsCreditBalance",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "ZeptoMailApiKey",
                table: "MisSettings");

            migrationBuilder.DropColumn(
                name: "ZeptoMailSendMailId",
                table: "MisSettings");

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[] { 92L, 13L, "The public portal where candidates enter their Tracking ID to view the status of their application.", "Track Application", "/MisRecruitment/Platform/Track" });
        }
    }
}
