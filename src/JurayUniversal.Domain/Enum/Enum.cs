using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JurayUniversal.Domain.Enum
{
    public class Enum
    {
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
            [Description("Pending")]
            Pending = 1,
            [Description("Approved")]
            Approved = 2,

            [Description("Failed")]
            Failed = 3,

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
    }
}