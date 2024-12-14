using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class settingsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserManagement",
                table: "SuperSettings",
                newName: "UsersDashboard");

            migrationBuilder.RenameColumn(
                name: "Sliders",
                table: "SuperSettings",
                newName: "StatisticsDashboard");

            migrationBuilder.RenameColumn(
                name: "Settings",
                table: "SuperSettings",
                newName: "ProjectsDashboard");

            migrationBuilder.RenameColumn(
                name: "RoleManagement",
                table: "SuperSettings",
                newName: "OrderDashboard");

            migrationBuilder.RenameColumn(
                name: "ReportingSystem",
                table: "SuperSettings",
                newName: "FinanceDashboard");

            migrationBuilder.RenameColumn(
                name: "Proposals",
                table: "SuperSettings",
                newName: "CommunicationDashboard");

            migrationBuilder.RenameColumn(
                name: "MeetingsManagement",
                table: "SuperSettings",
                newName: "ChartsDashboard");

            migrationBuilder.RenameColumn(
                name: "ManagePages",
                table: "SuperSettings",
                newName: "ActivateVisitorsReportAndAnalysis");

            migrationBuilder.RenameColumn(
                name: "Jobs",
                table: "SuperSettings",
                newName: "ActivateUserReportAndAnalysis");

            migrationBuilder.RenameColumn(
                name: "Forum",
                table: "SuperSettings",
                newName: "ActivateUserManagement");

            migrationBuilder.RenameColumn(
                name: "FAQ",
                table: "SuperSettings",
                newName: "ActivateUser");

            migrationBuilder.RenameColumn(
                name: "Expenses",
                table: "SuperSettings",
                newName: "ActivateTrainings");

            migrationBuilder.RenameColumn(
                name: "Dashboard",
                table: "SuperSettings",
                newName: "ActivateTestimonyContent");

            migrationBuilder.RenameColumn(
                name: "ContactUsRequest",
                table: "SuperSettings",
                newName: "ActivateTask");

            migrationBuilder.RenameColumn(
                name: "Blogs",
                table: "SuperSettings",
                newName: "ActivateSmsTools");

            migrationBuilder.RenameColumn(
                name: "Attendance",
                table: "SuperSettings",
                newName: "ActivateSliderContent");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ActivateManagementTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateNewsletterTools",
                table: "SuperSettings");

            migrationBuilder.DropColumn(
                name: "ActivateNoticeBoardCommnunicationTools",
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

            migrationBuilder.RenameColumn(
                name: "UsersDashboard",
                table: "SuperSettings",
                newName: "UserManagement");

            migrationBuilder.RenameColumn(
                name: "StatisticsDashboard",
                table: "SuperSettings",
                newName: "Sliders");

            migrationBuilder.RenameColumn(
                name: "ProjectsDashboard",
                table: "SuperSettings",
                newName: "Settings");

            migrationBuilder.RenameColumn(
                name: "OrderDashboard",
                table: "SuperSettings",
                newName: "RoleManagement");

            migrationBuilder.RenameColumn(
                name: "FinanceDashboard",
                table: "SuperSettings",
                newName: "ReportingSystem");

            migrationBuilder.RenameColumn(
                name: "CommunicationDashboard",
                table: "SuperSettings",
                newName: "Proposals");

            migrationBuilder.RenameColumn(
                name: "ChartsDashboard",
                table: "SuperSettings",
                newName: "MeetingsManagement");

            migrationBuilder.RenameColumn(
                name: "ActivateVisitorsReportAndAnalysis",
                table: "SuperSettings",
                newName: "ManagePages");

            migrationBuilder.RenameColumn(
                name: "ActivateUserReportAndAnalysis",
                table: "SuperSettings",
                newName: "Jobs");

            migrationBuilder.RenameColumn(
                name: "ActivateUserManagement",
                table: "SuperSettings",
                newName: "Forum");

            migrationBuilder.RenameColumn(
                name: "ActivateUser",
                table: "SuperSettings",
                newName: "FAQ");

            migrationBuilder.RenameColumn(
                name: "ActivateTrainings",
                table: "SuperSettings",
                newName: "Expenses");

            migrationBuilder.RenameColumn(
                name: "ActivateTestimonyContent",
                table: "SuperSettings",
                newName: "Dashboard");

            migrationBuilder.RenameColumn(
                name: "ActivateTask",
                table: "SuperSettings",
                newName: "ContactUsRequest");

            migrationBuilder.RenameColumn(
                name: "ActivateSmsTools",
                table: "SuperSettings",
                newName: "Blogs");

            migrationBuilder.RenameColumn(
                name: "ActivateSliderContent",
                table: "SuperSettings",
                newName: "Attendance");
        }
    }
}
