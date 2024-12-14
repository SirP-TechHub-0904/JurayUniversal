using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class DashboardSetting
    {
        public DashboardSetting()
        {
            ShowActivityProgram = true;
            ActivityProgramTitle = "Program Title";
        }

        public long Id { get; set; }

        [Display(Name = "Activity Program Title")]
        public string? ActivityProgramTitle { get; set; }


        [Display(Name = "Show Activity Program")]
        public bool ShowActivityProgram { get; set; }

        [Display(Name = "User Custom Dashboard")]
        public bool UserCustomDashboard { get; set; }


        [Display(Name = "Activate Dashboard")]
        public bool ActivateDashboard { get; set; }

        [Display(Name = "Statistics Dashboard")]
        public bool StatisticsDashboard { get; set; }
        [Display(Name = "Charts Dashboard")]
        public bool ChartsDashboard { get; set; }
        [Display(Name = "Projects Dashboard")]
        public bool ProjectsDashboard { get; set; }
        [Display(Name = "Communication Dashboard")]
        public bool CommunicationDashboard { get; set; }
        [Display(Name = "Users Dashboard")]
        public bool UsersDashboard { get; set; }
        [Display(Name = "Finance Dashboard")]
        public bool FinanceDashboard { get; set; }
        [Display(Name = "Order Dashboard")]
        public bool OrderDashboard { get; set; }

        [Display(Name = "Activate Project on Dashboard")]
        public bool ActivateProject { get; set; }


        [Display(Name = "Activate User Management and Role on Dashboard")]
        public bool ActivateUserManagement { get; set; }

        [Display(Name = "Activate User on Dashboard")]
        public bool ActivateUser { get; set; }
        [Display(Name = "Activate Roles on Dashboard")]
        public bool ActivateRoles { get; set; }
        [Display(Name = "Activate Attendance on Dashboard")]
        public bool ActivateAttendance { get; set; }
        [Display(Name = "Activate Report on Dashboard")]
        public bool ActivateReport { get; set; }
        [Display(Name = "Activate Task on Dashboard")]
        public bool ActivateTask { get; set; }
        [Display(Name = "Activate Trainings on Dashboard")]
        public bool ActivateTrainings { get; set; }
        [Display(Name = "Activate Salaries on Dashboard")]
        public bool ActivateSalaries { get; set; }

        [Display(Name = "Activate Job Referrals on Dashboard")]
        public bool ActivateJobReferrals { get; set; }
        [Display(Name = "Activate Finance Management on Dashboard")]
        public bool ActivateFinanceManagement { get; set; }
        [Display(Name = "Activate Payment on Finance Management on Dashboard")]
        public bool ActivatePaymentOnFinanceManagement { get; set; }

        [Display(Name = "Activate Budget on Finance Management on Dashboard")]
        public bool ActivateBudgetOnFinanceManagement { get; set; }
        [Display(Name = "Activate Expenses on Finance Management on Dashboard")]
        public bool ActivateExpensesOnFinanceManagement { get; set; }
        [Display(Name = "Activate Accounts on Finance Management on Dashboard")]
        public bool ActivateAccountsOnFinanceManagement { get; set; }
        [Display(Name = "Activate Report on Finance Management on Dashboard")]
        public bool ActivateReportOnFinanceManagement { get; set; }
        [Display(Name = "Activate Proposal Management on Dashboard")]
        public bool ActivateProposalManagement { get; set; }


        [Display(Name = "Activate Report And Analysis on Dashboard")]
        public bool ActivateReportAndAnalysis { get; set; }
        [Display(Name = "Activate user Report And Analysis on Dashboard")]
        public bool ActivateUserReportAndAnalysis { get; set; }

        [Display(Name = "Activate Visitors Report And Analysis on Dashboard")]
        public bool ActivateVisitorsReportAndAnalysis { get; set; }

        [Display(Name = "Activate Project Report And Analysis on Dashboard")]
        public bool ActivateProjectReportAndAnalysis { get; set; }

        [Display(Name = "Activate Communication Report And Analysis on Dashboard")]
        public bool ActivateCommunicationReportAndAnalysis { get; set; }

        [Display(Name = "Activate Content Report And Analysis on Dashboard")]
        public bool ActivateContentReportAndAnalysis { get; set; }

        [Display(Name = "Activate Content Management on Dashboard")]
        public bool ActivateContentManagement { get; set; }

        [Display(Name = "Activate Pages Content on Dashboard")]
        public bool ActivatePagesContent { get; set; }

        [Display(Name = "Activate Blog Content on Dashboard")]
        public bool ActivateBlogContent { get; set; }
        [Display(Name = "Activate Product Content on Dashboard")]
        public bool ActivateProductContent { get; set; }
        [Display(Name = "Activate FAQ Content on Dashboard")]
        public bool ActivateFaqContent { get; set; }
        [Display(Name = "Activate Testimony Content on Dashboard")]
        public bool ActivateTestimonyContent { get; set; }
        [Display(Name = "Activate Slider Content on Dashboard")]
        public bool ActivateSliderContent { get; set; }
        [Display(Name = "Activate Contact Information Content on Dashboard")]
        public bool ActivateContactInformationContent { get; set; }
        [Display(Name = "Activate Site Map on Dashboard")]
        public bool ActivateSiteMap { get; set; }

        [Display(Name = "Activate Commnunication Tools on Dashboard")]
        public bool ActivateCommnunicationTools { get; set; }

        [Display(Name = "Activate Forum Commnunication Tools on Dashboard")]
        public bool ActivateForumCommnunicationTools { get; set; }

        [Display(Name = "Activate Chat Commnunication Tools on Dashboard")]
        public bool ActivateChatCommnunicationTools { get; set; }

        [Display(Name = "Activate NoticeBoard Commnunication Tools on Dashboard")]
        public bool ActivateNoticeBoardCommnunicationTools { get; set; }
        [Display(Name = "Activate Contact Us Commnunication Tools on Dashboard")]
        public bool ActivateContactUsCommnunicationTools { get; set; }

        [Display(Name = "Activate Management Tools on Dashboard")]
        public bool ActivateManagementTools { get; set; }

        [Display(Name = "Activate SMS Tools on Dashboard")]
        public bool ActivateSmsTools { get; set; }

        [Display(Name = "Activate Email Tools on Dashboard")]
        public bool ActivateEmailTools { get; set; }

        [Display(Name = "Activate Email Tools on Dashboard")]
        public bool ActivateNewsletterTools { get; set; }


        [Display(Name = "Activate Birthday Tools on Dashboard")]
        public bool ActivateBirthdayTools { get; set; }

        [Display(Name = "Activate Settings on Dashboard")]
        public bool ActivateSettings { get; set; }


        [Display(Name = "Activate Order on Dashboard")]
        public bool ActivateOrder { get; set; }
       

        [Display(Name = "Activate Office Activity")]
        public bool ActivateOfficeActivity { get; set; }

    }
}
