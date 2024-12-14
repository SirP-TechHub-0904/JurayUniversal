using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class setting1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivateAccountsOnFinanceManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateAttendance",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateBirthdayTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateBlogContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateBudgetOnFinanceManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateChatCommnunicationTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateCommnunicationTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateCommunicationReportAndAnalysis",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContactInformationContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContactUsCommnunicationTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContentManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContentReportAndAnalysis",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateEmailTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateExpensesOnFinanceManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateFaqContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateFinanceManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateForumCommnunicationTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateJobReferrals",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateManagementTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateNewsletterTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateNoticeBoardCommnunicationTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateOfficeActivity",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateOrder",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivatePagesContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivatePaymentOnFinanceManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProductContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProject",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProjectReportAndAnalysis",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProposalManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateReport",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateReportAndAnalysis",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateReportOnFinanceManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateRoles",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSalaries",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSettings",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSiteMap",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSliderContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSmsTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateTask",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateTestimonyContent",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateTrainings",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateUser",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateUserManagement",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateUserReportAndAnalysis",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateVisitorsReportAndAnalysis",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ChartsDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "CommunicationDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "FinanceDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "OrderDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ProjectsDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "StatisticsDashboard",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "UsersDashboard",
                table: "SuperSettings");

            migrationBuilder.AddColumn<bool>(
                name: "ActivateAccountsOnFinanceManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateAttendance",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateBirthdayTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateBlogContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateBudgetOnFinanceManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateChatCommnunicationTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateCommnunicationTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateCommunicationReportAndAnalysis",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContactInformationContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContactUsCommnunicationTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContentManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContentReportAndAnalysis",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateEmailTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateExpensesOnFinanceManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateFaqContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateFinanceManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateForumCommnunicationTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateJobReferrals",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateManagementTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateNewsletterTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateNoticeBoardCommnunicationTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateOfficeActivity",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateOrder",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivatePagesContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivatePaymentOnFinanceManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProductContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProject",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProjectReportAndAnalysis",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProposalManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateReport",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateReportAndAnalysis",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateReportOnFinanceManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateRoles",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSalaries",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSettings",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSiteMap",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSliderContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSmsTools",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateTask",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateTestimonyContent",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateTrainings",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateUser",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateUserManagement",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateUserReportAndAnalysis",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateVisitorsReportAndAnalysis",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ChartsDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CommunicationDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FinanceDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OrderDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProjectsDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StatisticsDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsersDashboard",
                table: "DashboardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FormSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormSettings");

            migrationBuilder.DropColumn(
                name: "ActivateAccountsOnFinanceManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateAttendance",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateBirthdayTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateBlogContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateBudgetOnFinanceManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateChatCommnunicationTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateCommnunicationTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateCommunicationReportAndAnalysis",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContactInformationContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContactUsCommnunicationTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContentManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateContentReportAndAnalysis",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateEmailTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateExpensesOnFinanceManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateFaqContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateFinanceManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateForumCommnunicationTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateJobReferrals",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateManagementTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateNewsletterTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateNoticeBoardCommnunicationTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateOfficeActivity",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateOrder",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivatePagesContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivatePaymentOnFinanceManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProductContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProject",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProjectReportAndAnalysis",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateProposalManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateReport",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateReportAndAnalysis",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateReportOnFinanceManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateRoles",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSalaries",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSettings",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSiteMap",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSliderContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateSmsTools",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateTask",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateTestimonyContent",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateTrainings",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateUser",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateUserManagement",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateUserReportAndAnalysis",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ActivateVisitorsReportAndAnalysis",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ChartsDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "CommunicationDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "FinanceDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "OrderDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "ProjectsDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "StatisticsDashboard",
                table: "DashboardSettings");

            migrationBuilder.DropColumn(
                name: "UsersDashboard",
                table: "DashboardSettings");

            migrationBuilder.AddColumn<bool>(
                name: "ActivateAccountsOnFinanceManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateAttendance",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateBirthdayTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateBlogContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateBudgetOnFinanceManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateChatCommnunicationTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateCommnunicationTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateCommunicationReportAndAnalysis",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContactInformationContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContactUsCommnunicationTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContentManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateContentReportAndAnalysis",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateEmailTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateExpensesOnFinanceManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateFaqContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateFinanceManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateForumCommnunicationTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateJobReferrals",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateManagementTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateNewsletterTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateNoticeBoardCommnunicationTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateOfficeActivity",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateOrder",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivatePagesContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivatePaymentOnFinanceManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProductContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProject",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProjectReportAndAnalysis",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateProposalManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateReport",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateReportAndAnalysis",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateReportOnFinanceManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateRoles",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSalaries",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSettings",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSiteMap",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSliderContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateSmsTools",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateTask",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateTestimonyContent",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateTrainings",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateUser",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateUserManagement",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateUserReportAndAnalysis",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateVisitorsReportAndAnalysis",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ChartsDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CommunicationDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FinanceDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OrderDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProjectsDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StatisticsDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsersDashboard",
                table: "SuperSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
