using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JurayUniversal.Domain.Enum
{
    public class Enum
    {
        public enum EmailLogStatus
        {
            [Description("Pending")]
            Pending = 0,

            [Description("Sent")]
            Sent = 1,

            [Description("Failed")]
            Failed = 2
        }

        public enum RoleCategory
        {
            [Description("Legacy Dashboard Only")]
            Legacy = 0,

            [Description("MIS Platform Only")]
            MIS = 1,

            [Description("Both Legacy and MIS")]
            Both = 2
        }

        public enum PhaseStatus
        {
            [Description("Upcoming")]
            Upcoming = 0,
            
            [Description("In Progress")]
            InProgress = 1,
            
            [Description("Done")]
            Done = 2
        }

        public enum VotingStatus
        {
            [Description("None")]
            None = 0,

            [Description("Upcoming")]
            Upcoming = 2,

            [Description("Completed")]
            Completed = 3,

            [Description("Terminated")]
            Terminated = 4,


        }

        public enum ElectionType
        {
            [Description("Closed Election (Requires Accreditation)")]
            Closed = 0,

            [Description("Open Election (No Accreditation)")]
            Open = 1
        }
        public enum SenderIdStatus
        {
            Pending = 1,
            Failed = 2,
            Sent = 3,
            Draft = 4,
            Scheduled = 5
        }
        public enum MessageStatus
        {
            Pending = 1,
            Failed = 2,
            Sent = 3,
            Draft = 4,
            Scheduled = 5
        }
        public enum UserVoteStatus
        {
            [Description("None")]
            None = 0,

            [Description("Verified")]
            Verified = 2,

            [Description("Cancelled")]
            Cancelled = 3, 

        }
        public enum ParticipantStatus
        {
            [Description("None")]
            None = 0,

            [Description("Active")]
            Active = 2,

            [Description("Completed")]
            Completed = 3,

            [Description("Terminated")]
            Terminated = 4,
            [Description("Deleted")]
            Deleted = 5,


        }
        public enum EventType
        {

            [Description("NONE")]
            NONE = 0,
            [Description("Timetable")]
            Timetable = 2,
            [Description("Event")]
            Event = 3,

        }
        public enum ContentStatus
        {

            [Description("NONE")]
            NONE = 0,
            [Description("IsBold")]
            IsBold = 2,
            [Description("IsRed")]
            IsRed = 3,

        }
        public enum ContentEnumType
        {

            [Description("NONE")]
            NONE = 0,
            [Description("IsSingle")]
            IsSingle = 2,

        }

        public enum ResultType
        {
            
            [Description("YesNo")]
            YesNo = 0,

            [Description("Text")]
            Text = 3,
             

        }
        public enum CourseStatus
        {
            [Description("None")]
            None = 0,

            [Description("Active")]
            Active = 2,

            [Description("Concluded")]
            Concluded = 3,

            [Description("Upcoming")]
            Upcoming = 4,


        }
        public enum AccomodationUpdateStatus
        {
            [Description("None")]
            None = 0,

            [Description("Request")]
            Request = 2,

            [Description("Allocated")]
            Allocated = 3,
             
        }
        public enum MailSender
        {
            [Description("None")]
            None = 0,

            [Description("Outlook")]
            Outlook = 2,

            [Description("Google")]
            Google = 3,

            [Description("Webmail")]
            Webmail = 4,
             

        }
        public enum Priority
        {
            [Description("None")]
            None = 0,

            [Description("Emergency")]
            Emergency = 2,

            [Description("High")]
            High = 3,

            [Description("Medium")]
            Medium = 4,

            [Description("Low")]
            Low = 5,


        }
        public enum XProjectTaskStatus
        {
            [Description("None")]
            None = 0,

            [Description("Fail")]
            Fail = 1,

            [Description("Success")]
            Success = 2,

             

        }
        public enum EmailType
        {
            [Description("None")]
            None = 0,

            [Description("ChangeEmail")]
            ChangeEmail = 2,

            [Description("VerifyEmail")]
            VerifyEmail = 3,


        }
        public enum RibonPosition
        {
            [Description("None")]
            None = 0,

            [Description("Top")]
            Top = 2,

            [Description("BeforeMenu")]
            BeforeMenu = 3,
            [Description("AfterMenu")]
            AfterMenu = 4,
            [Description("AfterSlider")]
            AfterSlider = 5,
            [Description("BeforeFooter")]
            BeforeFooter = 6,

        }
        public enum DOBStatus
        {
            Public = 1,
            Private = 2,
        }
        public enum GenderStatus
        {
            Male = 0,
            Female = 1,
        }
        public enum ModalTime
        {
            [Description("None")]
            None = 0,

            [Description("Daily")]
            Daily = 2,

            [Description("Weekly")]
            Weekly = 3,

            [Description("Hourly")]
            Hourly = 4,

        }
        public enum ModalOccurance
        {
            [Description("None")]
            None = 0,

            [Description("Once")]
            Once = 2,

            [Description("Repeated")]
            Repeated = 3,

        }
        public enum ProjectStatus
        {
            [Description("Pending")]
            Pending = 0,
            [Description("Active")]
            Active = 1,

            [Description("Completed")]
            Completed = 2,

            [Description("Suspended")]
            Suspended = 3,

        }
        public enum TaskType
        {
            [Description("Idea")]
            Idea = 0,

            [Description("New")]
            New = 2,

            [Description("BackDrop")]
            BackDrop = 3,

        }
        public enum ProjectTaskStatus
        {
            [Description("ToDo")]
            ToDo = 0,

            [Description("Doing")]
            Doing = 2,

            [Description("Done")]
            Done = 3,

        }

        public enum ProjectFeatureStatus
        {
            [Description("Inprogress")]
            Inprogress = 0,

            [Description("Completed")]
            Completed = 2,

            [Description("Upgrading")]
            Upgrading = 3,
            [Description("Suspended")]
            Suspended = 4,
        }
        public enum PropertyStatus
        {
            [Description("None")]
            None = 0,

            [Description("Publish")]
            Publish = 2,

            [Description("Unpublish")]
            Unpublish = 3,

        }
        public enum NotificationStatus
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Sent")]
            Sent = 1,

            [Description("NotSent")]
            NotSent = 2,


        }
        public enum NotificationType
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("SMS")]
            SMS = 1,

            [Description("Email")]
            Email = 2


        }
        public enum HomeSortFrom
        {
            [Description("Top")]
            Top = 0,

            [Description("Bottom")]
            Bottom = 2,
            [Description("Middle")]
            Middle = 3,

        }
        public enum BlogSectionPosition
        {
            [Description("BeforeRecentPost")]
            BeforeRecentPost = 0,

            [Description("Bottom")]
            Bottom = 2,
            [Description("Middle")]
            Middle = 3,

        }
        public enum MenuSortFrom
        {
            [Description("Left")]
            left = 0,

            [Description("Right")]
            Right = 2,

        }
        public enum UserStatus
        {
            [Description("Pending")]
            Pending = 0,

            [Description("Active")]
            Active = 2,
            [Description("Suspended")]
            Suspended = 3,

            [Description("Leave")]
            Leave = 4,
            [Description("Deleted")]
            Deleted = 6,
        }
        public enum PaymentStatus
        {
            Pending = 0,
            Success = 1,
            Failed = 2
        }
        public enum PagePosition
        {
            [Description("None")]
            None = 0,

            [Description("Top")]
            Top = 2,

            [Description("Menu")]
            Menu = 3,
            [Description("Footer")]
            Footer = 4,
        }
        public enum AttendanceStatus
        {
            [Description("None")]
            None = 0,

            [Description("Present")]
            Present = 2,

            [Description("Absent")]
            Absent = 3,
            [Description("Late")]
            Late = 4
        }
        public enum PeriodStatus
        {
            [Description("None")]
            None = 0,

            [Description("Early")]
            Early = 2,

            [Description("Ontime")]
            Ontime = 3,

            [Description("Late")]
            Late = 5,

            [Description("VeryLate")]
            VeryLate = 7
        }
        public enum TrainingStatus
        {
            [Description("None")]
            None = 0,

            [Description("Done")]
            Done = 2,

            [Description("Canceled")]
            Canceled = 3,
            [Description("Missed")]
            Missed = 8,

        }
        public enum FundStatus
        {
            [Description("Success")]
            Success = 2,

            [Description("Failed")]
            Failed = 3,
            [Description("Pending")]
            Pending = 4,

        }

        public enum ProposalStatus
        {
            [Description("Draft")]
            Draft = 0,

            [Description("Pending")]
            Pending = 1,

            [Description("Approved")]
            Approved = 2,

            [Description("Failed")]
            Failed = 3,

            [Description("Under Review")]
            UnderReview = 4,

            [Description("Rejected")]
            Rejected = 5,

            [Description("Submitted")]
            Submitted = 6
        }
        public enum FundType
        {
            [Description("Credit")]
            Credit = 2,

            [Description("Debit")]
            Debit = 3,

        }
        public enum FundMeans
        {
            [Description("Transfer")]
            Transfer = 2,

            [Description("Cash")]
            Cash = 3,
            [Description("Check")]
            Check = 4,
        }
        public enum ReportPeriodStatus
        {
            [Description("None")]
            None = 0,

            [Description("Early")]
            Early = 2,

            [Description("Ontime")]
            Ontime = 3,

            [Description("Late")]
            Late = 5,

            [Description("VeryLate")]
            VeryLate = 7
        }
        public enum ReportStatus
        {
            [Description("None")]
            None = 0,

            [Description("Submitted")]
            Submitted = 2,
            [Description("Failed")]
            Failed = 3,
            [Description("NoReport")]
            NoReport = 4,
        }
        public enum VerificationStatus
        {
            [Description("Pending")]
            Pending = 0,

            [Description("Verified")]
            Verified = 2,

        }
        public enum TaskNoteStatus
        {
            [Description("None")]
            None = 0,

            [Description("Active")]
            Active = 2,

        }
        public enum Gender
        {
            [Description("Male")]
            Male = 0,

            [Description("Female")]
            Female = 2,

        }

        /// <summary>
        /// MIS: Defines the action level a user has within a specific feature Area.
        /// GlobalEditor = full CRUD rights across all features they are assigned to.
        /// FeatureEditor = full CRUD rights only within a specific feature.
        /// Readonly = can view data only — all Create/Edit/Delete buttons are hidden.
        /// </summary>
        public enum MisActionRole
        {
            [Description("Global Editor")]
            GlobalEditor = 1,

            [Description("Feature Editor")]
            FeatureEditor = 2,

            [Description("Read Only")]
            Readonly = 3,
        }

        public enum FeatureBillingDuration
        {
            [Description("Monthly")]
            Monthly = 1,

            [Description("Yearly")]
            Yearly = 2,

            [Description("One-Time")]
            OneTime = 3,
            
            [Description("Free")]
            Free = 4
        }

        public enum FeatureBillingStatus
        {
            [Description("Disabled")]
            Disabled = 0,

            [Description("Active")]
            Active = 1,

            [Description("Suspended")]
            Suspended = 2
        }

        public enum PaymentFrequency
        {
            [Description("One-Time")]
            OneTime = 0,

            [Description("Daily")]
            Daily = 1,

            [Description("Weekly")]
            Weekly = 2,

            [Description("Monthly")]
            Monthly = 3,

            [Description("Yearly")]
            Yearly = 4
        }

        public enum JobStatus
        {
            [Description("Draft")]
            Draft = 0,
            
            [Description("Published")]
            Published = 1,
            
            [Description("Closed")]
            Closed = 2
        }

        public enum JobType
        {
            [Description("Full-Time")]
            FullTime = 0,
            
            [Description("Part-Time")]
            PartTime = 1,
            
            [Description("Contract")]
            Contract = 2,
            
            [Description("Internship")]
            Internship = 3
        }

        public enum ApplicationStatus
        {
            [Description("Applied")]
            Applied = 0,
            
            [Description("CV Screening")]
            CVScreening = 1,
            
            [Description("Phone Screening")]
            PhoneScreening = 2,
            
            [Description("Online Interview")]
            OnlineInterview = 3,
            
            [Description("Offline Interview")]
            OfflineInterview = 4,
            
            [Description("Background Check")]
            BackgroundCheck = 5,
            
            [Description("Offered")]
            Offered = 6,
            
            [Description("Rejected")]
            Rejected = 7,

            [Description("Employed")]
            Employed = 8
        }

        public enum InterviewType
        {
            [Description("Online Video")]
            OnlineVideo = 0,
            
            [Description("Offline Physical")]
            OfflinePhysical = 1,
            
            [Description("Phone Call")]
            PhoneCall = 2
        }

        public enum MisElectionType
        {
            [Description("Closed Election (Requires Accreditation)")]
            Closed = 0,

            [Description("Open Election (No Accreditation)")]
            Open = 1,

            [Description("Public Poll (No Login Required)")]
            Public = 2
        }

        public enum MisVotingStatus
        {
            [Description("Upcoming")]
            Upcoming = 0,

            [Description("Active")]
            Active = 1,

            [Description("Completed")]
            Completed = 2,

            [Description("Terminated")]
            Terminated = 3
        }

        public enum MisEquipmentStatus
        {
            [Description("Available")]
            Available = 0,

            [Description("In Use")]
            InUse = 1,

            [Description("Maintenance")]
            Maintenance = 2,

            [Description("Retired")]
            Retired = 3
        }

        // --- MIS PROJECTS AGILE ENUMS ---

        public enum MisProjectStatus
        {
            [Description("Active")]
            Active = 0,
            [Description("Completed")]
            Completed = 1,
            [Description("On Hold")]
            OnHold = 2
        }

        public enum MisSprintStatus
        {
            [Description("Planned")]
            Planned = 0,
            [Description("Active")]
            Active = 1,
            [Description("Closed")]
            Closed = 2
        }

        public enum MisTaskType
        {
            [Description("Task")]
            Task = 0,
            [Description("User Story")]
            Story = 1,
            [Description("Bug")]
            Bug = 2,
            [Description("Epic")]
            Epic = 3
        }

        public enum MisTaskStatus
        {
            [Description("To Do")]
            ToDo = 0,
            [Description("In Progress")]
            InProgress = 1,
            [Description("Review")]
            Review = 2,
            [Description("Done")]
            Done = 3
        }

        public enum MisTaskPriority
        {
            [Description("Low")]
            Low = 0,
            [Description("Medium")]
            Medium = 1,
            [Description("High")]
            High = 2,
            [Description("Critical")]
            Critical = 3
        }

        // --- MIS SMART ASSIST AI ENUMS ---

        public enum MisAiProvider
        {
            [Description("None (Disabled)")]
            None = 0,
            [Description("Google Gemini")]
            Gemini = 1,
            [Description("OpenAI (ChatGPT)")]
            OpenAI = 2,
            [Description("Anthropic (Claude)")]
            Anthropic = 3
        }

        // --- MIS MASS COMM ENUMS ---

        public enum MisWalletTransactionType
        {
            [Description("Credit")]
            Credit = 0,
            [Description("Debit")]
            Debit = 1,
            [Description("Transfer")]
            Transfer = 2
        }

        public enum MisSmsCampaignStatus
        {
            [Description("Draft")]
            Draft = 0,
            [Description("Scheduled")]
            Scheduled = 1,
            [Description("Sending")]
            Sending = 2,
            [Description("Completed")]
            Completed = 3,
            [Description("Failed")]
            Failed = 4,
            [Description("Pending")]
            Pending = 5
        }

        public enum MisSmsDeliveryStatus
        {
            [Description("Pending")]
            Pending = 0,
            [Description("Sent")]
            Sent = 1,
            [Description("Failed")]
            Failed = 2
        }

        public enum MisSenderIdStatus
        {
            [Description("Pending")]
            Pending = 0,
            [Description("Active")]
            Active = 1,
            [Description("Rejected")]
            Rejected = 2
        }

        // --- MIS NEWSLETTER ENUMS ---
        
        public enum MisNewsletterCampaignStatus
        {
            [Description("Draft")]
            Draft = 0,
            [Description("Scheduled")]
            Scheduled = 1,
            [Description("Sending")]
            Sending = 2,
            [Description("Completed")]
            Completed = 3,
            [Description("Failed")]
            Failed = 4,
            [Description("Pending")]
            Pending = 5
        }

        public enum MisNewsletterDeliveryStatus
        {
            [Description("Pending")]
            Pending = 0,
            [Description("Sent")]
            Sent = 1,
            [Description("Failed")]
            Failed = 2
        }

        public enum LeaveRequestType
        {
            [Description("Annual Leave")]
            AnnualLeave = 1,

            [Description("Sick Leave")]
            SickLeave = 2,

            [Description("Maternity / Paternity Leave")]
            MaternityLeave = 3,

            [Description("Casual Leave")]
            CasualLeave = 4,

            [Description("Excuse from Duty")]
            ExcuseDuty = 5,

            [Description("Late to Work")]
            LateToWork = 6,

            [Description("Other")]
            Other = 7
        }

        public enum LeaveRequestStatus
        {
            [Description("Pending")]
            Pending = 0,

            [Description("Approved")]
            Approved = 1,

            [Description("Denied")]
            Denied = 2
        }

        public enum CaptchaType
        {
            [Description("None")]
            None = 0,

            [Description("Arithmetic Captcha")]
            Arithmetic = 1,

            [Description("Google reCAPTCHA")]
            GoogleRecaptcha = 2
        }
    }
}