using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class sms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetworkProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DigitCount = table.Column<int>(type: "int", nullable: false),
                    UnitsPerSms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InternationalDialCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SenderIds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderIdStatus = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsMessageCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmsMessageCategoryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsMessageCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsMessageCategories_SmsMessageCategories_SmsMessageCategoryId",
                        column: x => x.SmsMessageCategoryId,
                        principalTable: "SmsMessageCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SmsSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPerNewMember = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultUserUnitReorderLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlatUnitsPerSms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BlackListedWords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendOrderApprovedNotification = table.Column<bool>(type: "bit", nullable: false),
                    SendLowOnUnitsNotification = table.Column<bool>(type: "bit", nullable: false),
                    SendRecievedRequestNotification = table.Column<bool>(type: "bit", nullable: false),
                    SendReminderToDebtor = table.Column<bool>(type: "bit", nullable: false),
                    SendAccountCreditedNotification = table.Column<bool>(type: "bit", nullable: false),
                    SendUserBirthdayWishes = table.Column<bool>(type: "bit", nullable: false),
                    PreventApiModification = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DialCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceSettingId = table.Column<int>(type: "int", nullable: false),
                    PriceSettingId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DialCodes_PriceSettings_PriceSettingId1",
                        column: x => x.PriceSettingId1,
                        principalTable: "PriceSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitsUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Scheduleddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Resent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response_error_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response_cost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response_msg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response_length = table.Column<int>(type: "int", nullable: false),
                    Response_page = table.Column<int>(type: "int", nullable: false),
                    Response_balance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response_BalanceResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmsMessageCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsMessages_SmsMessageCategories_SmsMessageCategoryId",
                        column: x => x.SmsMessageCategoryId,
                        principalTable: "SmsMessageCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DialCodes_PriceSettingId1",
                table: "DialCodes",
                column: "PriceSettingId1");

            migrationBuilder.CreateIndex(
                name: "IX_SmsMessageCategories_SmsMessageCategoryId",
                table: "SmsMessageCategories",
                column: "SmsMessageCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsMessages_SmsMessageCategoryId",
                table: "SmsMessages",
                column: "SmsMessageCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DialCodes");

            migrationBuilder.DropTable(
                name: "SenderIds");

            migrationBuilder.DropTable(
                name: "SmsMessages");

            migrationBuilder.DropTable(
                name: "SmsSettings");

            migrationBuilder.DropTable(
                name: "PriceSettings");

            migrationBuilder.DropTable(
                name: "SmsMessageCategories");
        }
    }
}
