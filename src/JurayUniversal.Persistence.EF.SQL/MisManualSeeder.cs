using JurayUniversal.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Persistence.EF.SQL
{
    public static class MisManualSeeder
    {
        public static void SeedMisManuals(this ModelBuilder builder)
        {
            // 1. Insert the Main Manual Guide (Feature Level)
            builder.Entity<MisManual>().HasData(
                new MisManual { Id = 1L, ModuleFolder = "MisUserManagement", Overview = "This module controls the entire platform's user base. It allows you to register members, assign them to categories, monitor their Key Performance Indicators (KPIs), and tightly control what features they can access via granular roles.", Title = "User Management & Directory Guide" },
                new MisManual { Id = 2L, ModuleFolder = "MisGroups", Overview = "This module provides isolated, private workspaces where specific teams can discuss, collaborate, and share files. It is highly flexible and can be adapted for Committees, Chapters, Squads, or any organizational sub-group.", Title = "Committees & Groups Guide" },
                new MisManual { Id = 3L, ModuleFolder = "MisAlumni", Overview = "The Alumni Networking Directory allows past members to connect, find mentorship, and share business opportunities in a secure environment. Users can maintain professional profiles, flag their mentorship availability, and post on a community bulletin board.", Title = "Alumni Network Guide" },
                new MisManual { Id = 4L, ModuleFolder = "MisReports", Overview = "Dynamic Interval Reporting enforces strict submission deadlines for required staff reports, enabling administrators to track compliance automatically. Using advanced Template Flags, admins can dynamically build reports requiring hourly logs, file uploads, achievements, and challenges.", Title = "Dynamic Reports Guide" },
                new MisManual { Id = 5L, ModuleFolder = "MisForms", Overview = "The Form Builder module allows administrators to create custom surveys, registration forms, or feedback modules using a drag-and-drop interface, with built-in analytical tracking for submissions.", Title = "Form Builder & Surveys Guide" },
                new MisManual { Id = 6L, ModuleFolder = "MisFinances", Overview = "Complete guide for managing compulsory dues and voluntary donations, including payment setup, tracking, and PixelPay integration.", Title = "Dues & Donations Guide" },
                new MisManual { Id = 7L, ModuleFolder = "MisTicketing", Overview = "Complete guide for managing events, generating ticket tiers, processing payments, and scanning QR codes.", Title = "Event & Ticket Engine Guide" },
                new MisManual { Id = 8L, ModuleFolder = "MisSetting", Overview = "Global configuration and terminology settings for the MIS environment.", Title = "System Configuration Guide" },
                new MisManual { Id = 9L, ModuleFolder = "MisManagement", Overview = "Billing parameters, feature toggles, and overall module lifecycle management.", Title = "Feature & Billing Guide" },
                new MisManual { Id = 10L, ModuleFolder = "MisAuth", Overview = "Security preferences including Two-Factor Authentication (2FA), Authenticator App setup, and Recovery Codes.", Title = "Security & Auth Guide" },
                new MisManual { Id = 11L, ModuleFolder = "MisGrants", Overview = "Grant Ledger and external funding tracking.", Title = "Grant Tracking Guide" },
                new MisManual { Id = 12L, ModuleFolder = "MisProposals", Overview = "Proposal Pipeline for internal pitches and external grant applications.", Title = "Proposal Pipeline Guide" },
                new MisManual { Id = 13L, ModuleFolder = "MisRecruitment", Overview = "Applicant Tracking System (ATS) for managing job postings, candidate pipelines, structured CVs, and interview scoring.", Title = "Recruitment & ATS Guide" },
                new MisManual { Id = 14L, ModuleFolder = "MisFleet", Overview = "Complete asset ledger, real-time depreciation calculation, maintenance logging, and checkout/checkin workflows.", Title = "Fleet & Equipment Management Guide" },
                new MisManual { Id = 15L, ModuleFolder = "MisVoting", Overview = "Comprehensive e-voting engine supporting both Open (pool-based) and Closed (accreditation-based) elections with real-time tabulation.", Title = "Elections & E-Voting Guide" },
                new MisManual { Id = 16L, ModuleFolder = "MisProjects", Overview = "Agile Project Tracking module featuring interactive Scrum/Kanban boards, Sprint planning, Backlog management, and Work Item tracking.", Title = "Agile Projects & Kanban Guide" },
                new MisManual { Id = 17L, ModuleFolder = "MisSmartAssist", Overview = "Bring-Your-Own-Key (BYOK) AI Integration allowing organizations to connect their own OpenAI, Google Gemini, or Anthropic API keys to power intelligent chat features.", Title = "SmartAssist AI Integration Guide" },
                new MisManual { Id = 18L, ModuleFolder = "MisNewsletter", Overview = "A complete drag-and-drop newsletter builder, mailing list manager, and global quota distribution system.", Title = "Newsletter Campaign Guide" },
                new MisManual { Id = 19L, ModuleFolder = "MisMassComm", Overview = "Complete global SMS engine for sending bulk messages and managing phone book contacts.", Title = "Mass Comm SMS Guide" }
            );

            // 2. Insert the Pages
            builder.Entity<MisManualPage>().HasData(
                new MisManualPage { Id = 1L, MisManualId = 1L, PageDescription = "The high-level command center for the User Management module. This page gives you demographic statistics, quick analytics on active/suspended accounts, and shortcuts to manage users.", PageTitle = "User Directory Dashboard", RouteKey = "/MisUserManagement/Dashboard/Index" },
                new MisManualPage { Id = 2L, MisManualId = 1L, PageDescription = "The comprehensive table view of all users in the organization. It supports deep filtering by status, category, and advanced search.", PageTitle = "Full User Roster", RouteKey = "/MisUserManagement/Dashboard/List" },
                new MisManualPage { Id = 3L, MisManualId = 1L, PageDescription = "The detailed individual record for a specific user. This is where administrators manage a user's specific access rights, private HR notes, and performance scores.", PageTitle = "User 360 Profile (Admin View)", RouteKey = "/MisUserManagement/Dashboard/Detail" },
                new MisManualPage { Id = 4L, MisManualId = 1L, PageDescription = "The self-service portal where a regular user can view their own information, update their password, and check their performance scores.", PageTitle = "My Personal Details", RouteKey = "/MisUserManagement/Platform/MyProfile" },
                
                // NEW PAGES
                new MisManualPage { Id = 5L, MisManualId = 1L, PageDescription = "Manage the various categories, departments, or cohorts that users can be grouped into.", PageTitle = "Category Directory", RouteKey = "/MisUserManagement/Dashboard/Categories/Index" },
                new MisManualPage { Id = 6L, MisManualId = 1L, PageDescription = "Define the core metrics and criteria by which users in the organization are evaluated.", PageTitle = "KPI Indicators Settings", RouteKey = "/MisUserManagement/Dashboard/KpiSettings" },
                new MisManualPage { Id = 7L, MisManualId = 1L, PageDescription = "The evaluation panel where managers rate an individual user's performance against the established KPIs for a specific period.", PageTitle = "KPI Evaluation Panel", RouteKey = "/MisUserManagement/Dashboard/KpiAssign" },
                new MisManualPage { Id = 8L, MisManualId = 1L, PageDescription = "A competitive or analytical view ranking the overall performance scores of all evaluated users during a specific period.", PageTitle = "KPI Leaderboard", RouteKey = "/MisUserManagement/Dashboard/KpiResults" },
                new MisManualPage { Id = 9L, MisManualId = 1L, PageDescription = "Your personal performance dashboard. Review your evaluations, scores, and manager remarks over time.", PageTitle = "My KPI Scores", RouteKey = "/MisUserManagement/Platform/MyKpis" },
                new MisManualPage { Id = 10L, MisManualId = 1L, PageDescription = "The security control center for a user. Strictly control what modules they can access and their exact permission level.", PageTitle = "Manage Access Permissions", RouteKey = "/MisUserManagement/Dashboard/Roles" },
                new MisManualPage { Id = 11L, MisManualId = 1L, PageDescription = "A transparent, read-only view of your active access rights and permission levels across all platform modules.", PageTitle = "My Permissions Matrix", RouteKey = "/MisUserManagement/Platform/MyRoles" },
                new MisManualPage { Id = 12L, MisManualId = 1L, PageDescription = "Keep your account secure by changing your password regularly and enabling Two-Factor Authentication (2FA).", PageTitle = "Security Settings & 2FA", RouteKey = "/MisUserManagement/Platform/ChangePassword" },

                // --- MIS COMMITTEES PAGES ---
                new MisManualPage { Id = 13L, MisManualId = 2L, PageDescription = "The high-level command center for groups. Shows total groups, active groups, and total memberships.", PageTitle = "Groups Overview", RouteKey = "/MisGroups/Dashboard/Index" },
                new MisManualPage { Id = 14L, MisManualId = 2L, PageDescription = "The comprehensive list of all groups in the organization where you can add new ones or manage existing ones.", PageTitle = "Group Directory", RouteKey = "/MisGroups/Dashboard/List" },
                new MisManualPage { Id = 15L, MisManualId = 2L, PageDescription = "The administration panel for a specific group where you can add or remove members and set their roles.", PageTitle = "Manage Group Members", RouteKey = "/MisGroups/Dashboard/Detail" },
                new MisManualPage { Id = 16L, MisManualId = 2L, PageDescription = "Your personal dashboard showing only the groups you are a member of.", PageTitle = "My Groups", RouteKey = "/MisGroups/Platform/Index" },
                new MisManualPage { Id = 17L, MisManualId = 2L, PageDescription = "The private collaboration space for a group. You can post messages, share files, and view other members.", PageTitle = "Group Workspace", RouteKey = "/MisGroups/Platform/Workspace" },
                new MisManualPage { Id = 18L, MisManualId = 2L, PageDescription = "A form to create a new organizational group.", PageTitle = "Create Group", RouteKey = "/MisGroups/Dashboard/Create" },
                new MisManualPage { Id = 19L, MisManualId = 2L, PageDescription = "A form to update the details of an existing group.", PageTitle = "Edit Group", RouteKey = "/MisGroups/Dashboard/Edit" },
                new MisManualPage { Id = 20L, MisManualId = 2L, PageDescription = "An invitation landing page where users can accept invites to join a group.", PageTitle = "Join Group", RouteKey = "/MisGroups/Platform/Join" },

                // --- MIS ALUMNI PAGES ---
                new MisManualPage { Id = 21L, MisManualId = 3L, PageDescription = "Administrative overview of the alumni network, displaying total members and available mentors.", PageTitle = "Alumni Dashboard", RouteKey = "/MisAlumni/Dashboard/Index" },
                new MisManualPage { Id = 22L, MisManualId = 3L, PageDescription = "Admin view to verify pending alumni or revoke access.", PageTitle = "Alumni Directory (Admin)", RouteKey = "/MisAlumni/Dashboard/List" },
                new MisManualPage { Id = 23L, MisManualId = 3L, PageDescription = "A form to manually flag an existing user as an Alumni.", PageTitle = "Add Alumni", RouteKey = "/MisAlumni/Dashboard/Create" },
                new MisManualPage { Id = 24L, MisManualId = 3L, PageDescription = "The welcome dashboard for the Alumni Network showing recent posts and featured mentors.", PageTitle = "Network Welcome", RouteKey = "/MisAlumni/Platform/Index" },
                new MisManualPage { Id = 25L, MisManualId = 3L, PageDescription = "A searchable grid of all verified Alumni. You can filter by profession or mentorship availability.", PageTitle = "Member Directory", RouteKey = "/MisAlumni/Platform/Directory" },
                new MisManualPage { Id = 26L, MisManualId = 3L, PageDescription = "The central bulletin board for business opportunities and networking requests.", PageTitle = "Opportunities Board", RouteKey = "/MisAlumni/Platform/Opportunities" },
                new MisManualPage { Id = 27L, MisManualId = 3L, PageDescription = "Your personal portal to update your employment history and mentorship status.", PageTitle = "My Alumni Profile", RouteKey = "/MisAlumni/Platform/MyProfile" },

                // --- MIS REPORTS PAGES ---
                new MisManualPage { Id = 28L, MisManualId = 4L, PageDescription = "Graphical dashboard showing On-Time versus Late submission trends over the last 7 days. Helps management quickly identify reporting bottlenecks.", PageTitle = "Reports Analytics", RouteKey = "/MisReports/Dashboard/Index" },
                new MisManualPage { Id = 29L, MisManualId = 4L, PageDescription = "Manage the definitions, deadlines, and Dynamic Templates for different reports. Toggle on/off features like Hourly Logs or File Uploads to shape exactly what staff must submit.", PageTitle = "Manage Schemas", RouteKey = "/MisReports/Dashboard/Schemas/Index" },
                new MisManualPage { Id = 30L, MisManualId = 4L, PageDescription = "Assign specific staff members who must submit this report. Only these enrolled participants will see the report on their platform dashboard.", PageTitle = "Required Participants", RouteKey = "/MisReports/Dashboard/Schemas/Participants" },
                new MisManualPage { Id = 31L, MisManualId = 4L, PageDescription = "The master ledger displaying all historical report submissions and their timing status. Click into any submission to read the full report, download attachments, and leave Manager Comments.", PageTitle = "Submissions Ledger", RouteKey = "/MisReports/Dashboard/Submissions" },
                new MisManualPage { Id = 32L, MisManualId = 4L, PageDescription = "Your personal queue of reports that are due today or in the near future. Keep an eye on the deadline to avoid late flags.", PageTitle = "My Pending Reports", RouteKey = "/MisReports/Platform/Index" },

                // --- MIS FINANCES PAGES ---
                new MisManualPage { Id = 43L, MisManualId = 6L, PageDescription = "This is where you define and manage all your organization's payment categories — both compulsory dues (e.g. annual subscriptions) and optional donations (e.g. building fund, charity).", PageTitle = "Payment Categories", RouteKey = "/MisFinances/Dashboard/Categories" },
                new MisManualPage { Id = 44L, MisManualId = 6L, PageDescription = "The Ledger gives you a complete financial picture for a single payment category — total revenue collected, who has paid, and (for compulsory dues) a full defaulters list.", PageTitle = "Payment Ledger", RouteKey = "/MisFinances/Dashboard/Ledger" },
                new MisManualPage { Id = 45L, MisManualId = 6L, PageDescription = "A chronological master log of every single payment transaction recorded in the system across all categories. Filter by category, status, or date range.", PageTitle = "All Transactions", RouteKey = "/MisFinances/Dashboard/Transactions" },
                new MisManualPage { Id = 46L, MisManualId = 6L, PageDescription = "Your personal financial dashboard. Shows all payments you owe to the organization, causes you can support, and your complete payment history.", PageTitle = "My Finances", RouteKey = "/MisFinances/Platform/MyFinances" },
                new MisManualPage { Id = 33L, MisManualId = 4L, PageDescription = "The dynamic form where you write and file your required report before the deadline. The fields you see are strictly defined by the Admin's template flags.", PageTitle = "Submit Report", RouteKey = "/MisReports/Platform/Submit" },
                new MisManualPage { Id = 40L, MisManualId = 4L, PageDescription = "Your dedicated workspace for this specific report schedule. View monthly analytics, check the visual calendar, and submit or edit your reports.", PageTitle = "Report Workspace", RouteKey = "/MisReports/Platform/SchemaDetails" },
                new MisManualPage { Id = 41L, MisManualId = 4L, PageDescription = "Detailed view of your filed report submission, detailing times logs, achievements, challenges, and attachments.", PageTitle = "Report Details", RouteKey = "/MisReports/Platform/Details" },
                new MisManualPage { Id = 42L, MisManualId = 4L, PageDescription = "Update your submitted report data and active time logs before the daily submission deadline cut-off.", PageTitle = "Edit Report Submission", RouteKey = "/MisReports/Platform/Edit" },

                // --- MIS FORMS PAGES ---
                new MisManualPage { Id = 34L, MisManualId = 5L, PageDescription = "The master list of all custom forms and surveys. You can track whether they are currently published or in draft mode.", PageTitle = "Forms Directory", RouteKey = "/MisForms/Dashboard/Index" },
                new MisManualPage { Id = 35L, MisManualId = 5L, PageDescription = "Initial setup page to define a new form's basic details and configure settings such as user login requirements, multiple submission limits, and user response view permissions.", PageTitle = "Create Form", RouteKey = "/MisForms/Dashboard/Create" },
                new MisManualPage { Id = 36L, MisManualId = 5L, PageDescription = "The visual Drag-and-Drop Form Builder interface. Drag elements from the panel to construct the exact data schema you want to collect.", PageTitle = "Drag-and-Drop Builder", RouteKey = "/MisForms/Dashboard/Builder" },
                new MisManualPage { Id = 37L, MisManualId = 5L, PageDescription = "A dynamically generated table that unpacks user JSON responses and displays them in a readable format matching your custom form labels.", PageTitle = "Form Responses Ledger", RouteKey = "/MisForms/Dashboard/Responses" },
                new MisManualPage { Id = 38L, MisManualId = 5L, PageDescription = "The user-facing dashboard displaying all active, published forms they are eligible to fill out.", PageTitle = "Available Forms", RouteKey = "/MisForms/Platform/Index" },
                new MisManualPage { Id = 39L, MisManualId = 5L, PageDescription = "The dynamic form-filling interface. It instantly translates the Admin's drag-and-drop schema into a functional HTML form.", PageTitle = "Submit Form", RouteKey = "/MisForms/Platform/Submit" },
                new MisManualPage { Id = 47L, MisManualId = 5L, PageDescription = "Your personal portal to view all your previous submissions for a specific form/survey, provided the administrator enabled response viewing.", PageTitle = "My Submissions / Responses", RouteKey = "/MisForms/Platform/MyResponses" },

                // --- MIS TICKETING PAGES ---
                new MisManualPage { Id = 48L, MisManualId = 7L, PageDescription = "The main dashboard listing all your events and showing high-level stats like total tickets sold.", PageTitle = "Events Dashboard", RouteKey = "/MisTicketing/Dashboard/Index" },
                new MisManualPage { Id = 49L, MisManualId = 7L, PageDescription = "Set up a new event, including dates, venue, and descriptions.", PageTitle = "Create Event", RouteKey = "/MisTicketing/Dashboard/Create" },
                new MisManualPage { Id = 50L, MisManualId = 7L, PageDescription = "Manage an event's ticket tiers and check the real-time capacity and sales figures.", PageTitle = "Manage Ticket Tiers", RouteKey = "/MisTicketing/Dashboard/Manage" },
                
                // --- ADDED MIS TICKETING PAGES ---
                new MisManualPage { Id = 51L, MisManualId = 7L, PageDescription = "Scan guest QR codes for fast event check-in.", PageTitle = "QR Check-In Scanner", RouteKey = "/MisTicketing/Dashboard/Scan" },
                new MisManualPage { Id = 52L, MisManualId = 7L, PageDescription = "Detailed view of an upcoming event including ticket availability.", PageTitle = "Event Details", RouteKey = "/MisTicketing/Platform/EventDetails" },
                new MisManualPage { Id = 53L, MisManualId = 7L, PageDescription = "Finalize your ticket purchase and review your order.", PageTitle = "Checkout", RouteKey = "/MisTicketing/Platform/Checkout" },
                new MisManualPage { Id = 54L, MisManualId = 7L, PageDescription = "Browse all upcoming events and secure your tickets.", PageTitle = "Upcoming Events", RouteKey = "/MisTicketing/Platform/Index" },

                // --- ADDED MIS FINANCES PAGES ---
                new MisManualPage { Id = 55L, MisManualId = 6L, PageDescription = "A complete administrative history of all system transactions.", PageTitle = "Transaction History", RouteKey = "/MisFinances/Dashboard/TransactionHistory" },
                new MisManualPage { Id = 56L, MisManualId = 6L, PageDescription = "Detailed receipt and status of a specific payment.", PageTitle = "Transaction Details", RouteKey = "/MisFinances/Dashboard/TransactionDetails" },
                new MisManualPage { Id = 57L, MisManualId = 6L, PageDescription = "Financial analytics dashboard tracking revenue and payments.", PageTitle = "Financial Analytics", RouteKey = "/MisFinances/Dashboard/Index" },
                new MisManualPage { Id = 58L, MisManualId = 6L, PageDescription = "A history of all personal payments and contributions.", PageTitle = "My Payment History", RouteKey = "/MisFinances/Platform/TransactionHistory" },

                // --- ADDED MIS USER MANAGEMENT PAGES ---
                new MisManualPage { Id = 59L, MisManualId = 1L, PageDescription = "Create a new organizational category.", PageTitle = "Create Category", RouteKey = "/MisUserManagement/Dashboard/Categories/Create" },
                new MisManualPage { Id = 60L, MisManualId = 1L, PageDescription = "Modify an existing category.", PageTitle = "Edit Category", RouteKey = "/MisUserManagement/Dashboard/Categories/Edit" },
                new MisManualPage { Id = 61L, MisManualId = 1L, PageDescription = "Remove a category permanently.", PageTitle = "Delete Category", RouteKey = "/MisUserManagement/Dashboard/Categories/Delete" },

                // --- ADDED MIS REPORTS PAGES ---
                new MisManualPage { Id = 62L, MisManualId = 4L, PageDescription = "Define a new required report and its deadlines.", PageTitle = "Create Schema", RouteKey = "/MisReports/Dashboard/Schemas/Create" },
                new MisManualPage { Id = 63L, MisManualId = 4L, PageDescription = "Modify an existing report template.", PageTitle = "Edit Schema", RouteKey = "/MisReports/Dashboard/Schemas/Edit" },
                new MisManualPage { Id = 64L, MisManualId = 4L, PageDescription = "Detailed view of an individual staff submission.", PageTitle = "Submission Details", RouteKey = "/MisReports/Dashboard/Details" },

                // --- MIS SETTING PAGES ---
                new MisManualPage { Id = 65L, MisManualId = 8L, PageDescription = "Customize global terminology and application behavior.", PageTitle = "Global Configuration", RouteKey = "/MisSetting/Dashboard/Index" },

                // --- MIS MANAGEMENT PAGES ---
                new MisManualPage { Id = 66L, MisManualId = 9L, PageDescription = "Update billing and lifecycle settings for a feature.", PageTitle = "Edit Feature Config", RouteKey = "/MisManagement/Edit" },
                new MisManualPage { Id = 67L, MisManualId = 9L, PageDescription = "Register a new feature and define billing.", PageTitle = "Configure New Feature", RouteKey = "/MisManagement/Create" },
                new MisManualPage { Id = 110L, MisManualId = 9L, PageDescription = "Create a new pricing plan specifying the email quota and duration for the Newsletter module.", PageTitle = "Create Pricing Plan", RouteKey = "/MisManagement/Plans/Create" },
                new MisManualPage { Id = 111L, MisManualId = 9L, PageDescription = "Update the price, quota, or duration of an existing Newsletter pricing plan.", PageTitle = "Edit Pricing Plan", RouteKey = "/MisManagement/Plans/Edit" },

                // --- MIS AUTH PAGES ---
                new MisManualPage { Id = 68L, MisManualId = 10L, PageDescription = "Overview of Two-Factor Authentication status.", PageTitle = "2FA Status", RouteKey = "/MisAuth/Auth/MisTwoFactorAuthentication" },
                new MisManualPage { Id = 69L, MisManualId = 10L, PageDescription = "View emergency backup codes.", PageTitle = "Recovery Codes", RouteKey = "/MisAuth/Auth/MisShowRecoveryCodes" },
                new MisManualPage { Id = 70L, MisManualId = 10L, PageDescription = "Reset the keys used by your authenticator app.", PageTitle = "Reset Authenticator", RouteKey = "/MisAuth/Auth/MisResetAuthenticator" },
                new MisManualPage { Id = 71L, MisManualId = 10L, PageDescription = "Generate new emergency backup codes.", PageTitle = "Generate Recovery Codes", RouteKey = "/MisAuth/Auth/MisGenerateRecoveryCodes" },
                new MisManualPage { Id = 72L, MisManualId = 10L, PageDescription = "Scan a QR code to link your authenticator app.", PageTitle = "Enable Authenticator", RouteKey = "/MisAuth/Auth/MisEnableAuthenticator" },
                new MisManualPage { Id = 73L, MisManualId = 10L, PageDescription = "Turn off two-factor authentication.", PageTitle = "Disable 2FA", RouteKey = "/MisAuth/Auth/MisDisable2fa" },

                // --- MIS GRANTS PAGES ---
                new MisManualPage { Id = 74L, MisManualId = 11L, PageDescription = "Analytics dashboard showing all awarded external grants.", PageTitle = "Grant Ledger", RouteKey = "/MisGrants/Dashboard/Index" },
                new MisManualPage { Id = 75L, MisManualId = 11L, PageDescription = "View deep details, milestones, and financials for a specific grant.", PageTitle = "Grant Details", RouteKey = "/MisGrants/Dashboard/Details" },
                new MisManualPage { Id = 76L, MisManualId = 11L, PageDescription = "Form to register or update the details of an awarded grant.", PageTitle = "Manage Grant", RouteKey = "/MisGrants/Dashboard/Manage" },
                new MisManualPage { Id = 77L, MisManualId = 11L, PageDescription = "Directory of all external donors, sponsors, and funding agencies.", PageTitle = "Donors Directory", RouteKey = "/MisGrants/Dashboard/Donors/Index" },
                new MisManualPage { Id = 78L, MisManualId = 11L, PageDescription = "Add or update the profile of a funding organization.", PageTitle = "Manage Donor", RouteKey = "/MisGrants/Dashboard/Donors/Manage" },
                
                // --- MIS PROPOSALS PAGES ---
                new MisManualPage { Id = 79L, MisManualId = 12L, PageDescription = "Overview of all active internal pitches and submitted grant proposals.", PageTitle = "Proposal Pipeline", RouteKey = "/MisProposals/Dashboard/Index" },
                new MisManualPage { Id = 80L, MisManualId = 12L, PageDescription = "Detailed view of a proposal, including its execution phases and uploaded documents.", PageTitle = "Proposal Details", RouteKey = "/MisProposals/Dashboard/Details" },
                new MisManualPage { Id = 81L, MisManualId = 12L, PageDescription = "Form for an Administrator to draft or edit a new proposal.", PageTitle = "Manage Proposal", RouteKey = "/MisProposals/Dashboard/Manage" },
                new MisManualPage { Id = 82L, MisManualId = 12L, PageDescription = "Your personal dashboard showing proposals and pitches you have submitted.", PageTitle = "My Proposals", RouteKey = "/MisProposals/Platform/Index" },
                new MisManualPage { Id = 83L, MisManualId = 12L, PageDescription = "View the current review status and phases of your pitched proposal.", PageTitle = "My Proposal Details", RouteKey = "/MisProposals/Platform/Details" },
                new MisManualPage { Id = 84L, MisManualId = 12L, PageDescription = "Form for a regular user to submit a new pitch or proposal to management.", PageTitle = "Submit Proposal", RouteKey = "/MisProposals/Platform/Submit" },

                // --- MIS RECRUITMENT PAGES ---
                new MisManualPage { Id = 85L, MisManualId = 13L, PageDescription = "High-level overview of active jobs, total applicants, and pending interviews.", PageTitle = "ATS Dashboard", RouteKey = "/MisRecruitment/Dashboard/Index" },
                new MisManualPage { Id = 86L, MisManualId = 13L, PageDescription = "List of all published, closed, and draft job postings.", PageTitle = "Job Postings", RouteKey = "/MisRecruitment/Dashboard/Jobs/Index" },
                new MisManualPage { Id = 87L, MisManualId = 13L, PageDescription = "Create or update a job posting including descriptions, salary, and requirements.", PageTitle = "Manage Job Posting", RouteKey = "/MisRecruitment/Dashboard/Jobs/Manage" },
                new MisManualPage { Id = 88L, MisManualId = 13L, PageDescription = "The pipeline list of all candidates who have applied, filterable by Job Title.", PageTitle = "Applicant Pipeline", RouteKey = "/MisRecruitment/Dashboard/Applicants/Index" },
                new MisManualPage { Id = 89L, MisManualId = 13L, PageDescription = "Detailed view of an applicant's structured CV, attached documents, and interview scoring.", PageTitle = "Review Candidate", RouteKey = "/MisRecruitment/Dashboard/Applicants/Details" },
                new MisManualPage { Id = 90L, MisManualId = 13L, PageDescription = "The public portal where candidates can browse open roles.", PageTitle = "Public Job Board", RouteKey = "/MisRecruitment/Platform/JobBoard" },
                new MisManualPage { Id = 91L, MisManualId = 13L, PageDescription = "The structured application form where candidates enter their details and upload their CV/documents.", PageTitle = "Application Form", RouteKey = "/MisRecruitment/Platform/Apply" },
                new MisManualPage { Id = 93L, MisManualId = 13L, PageDescription = "The public portal where candidates enter their Tracking ID to view the status of their application.", PageTitle = "Track Application", RouteKey = "/MisRecruitment/Platform/Track" },

                // --- MIS FLEET PAGES ---
                new MisManualPage { Id = 94L, MisManualId = 14L, PageDescription = "Analytics tracking total asset value, lifetime maintenance costs, and depreciation.", PageTitle = "Fleet Analytics Dashboard", RouteKey = "/MisFleet/Dashboard/Equipment/Index" },
                new MisManualPage { Id = 95L, MisManualId = 14L, PageDescription = "Detailed view of an individual equipment's details, lifecycle, and depreciation.", PageTitle = "Equipment Details", RouteKey = "/MisFleet/Dashboard/Equipment/Details" },
                new MisManualPage { Id = 96L, MisManualId = 14L, PageDescription = "Form to add a new asset to the fleet inventory.", PageTitle = "Add Equipment", RouteKey = "/MisFleet/Dashboard/Equipment/Create" },
                new MisManualPage { Id = 97L, MisManualId = 14L, PageDescription = "Manage the categories that fleet assets can belong to.", PageTitle = "Fleet Categories", RouteKey = "/MisFleet/Dashboard/Categories/Index" },

                // --- MIS VOTING PAGES ---
                new MisManualPage { Id = 98L, MisManualId = 15L, PageDescription = "Voting command center showing total elections, active voters, and turnout charts.", PageTitle = "Elections Analytics", RouteKey = "/MisVoting/Dashboard/Elections/Index" },
                new MisManualPage { Id = 99L, MisManualId = 15L, PageDescription = "Detailed configuration page for an election, where candidates are listed and settings defined.", PageTitle = "Election Details", RouteKey = "/MisVoting/Dashboard/Elections/Details" },
                new MisManualPage { Id = 100L, MisManualId = 15L, PageDescription = "The accreditation checklist to authorize voters for a Closed Election.", PageTitle = "Voter Accreditation", RouteKey = "/MisVoting/Dashboard/Elections/Accreditation" },
                new MisManualPage { Id = 101L, MisManualId = 15L, PageDescription = "User portal displaying all available elections they can vote in.", PageTitle = "Voter Dashboard", RouteKey = "/MisVoting/Dashboard/Index" },
                new MisManualPage { Id = 102L, MisManualId = 15L, PageDescription = "Real-time live results tabulation for an ongoing or concluded election.", PageTitle = "Live Results", RouteKey = "/MisVoting/Dashboard/Results" },
                new MisManualPage { Id = 103L, MisManualId = 15L, PageDescription = "The secure voting booth where users cast their ballot.", PageTitle = "Voting Booth", RouteKey = "/MisVoting/Platform/Vote" },

                // --- MIS PROJECTS PAGES ---
                new MisManualPage { Id = 104L, MisManualId = 16L, PageDescription = "Global command center for Agile projects, sprint velocities, and task tracking.", PageTitle = "Agile Analytics Dashboard", RouteKey = "/MisProjects/Dashboard/Index" },
                new MisManualPage { Id = 105L, MisManualId = 16L, PageDescription = "Manage all software and general Agile projects in the organization.", PageTitle = "Agile Projects", RouteKey = "/MisProjects/Projects/Index" },
                new MisManualPage { Id = 106L, MisManualId = 16L, PageDescription = "Plan upcoming sprints, manage your backlog, and assign work items.", PageTitle = "Sprint Planning & Backlog", RouteKey = "/MisProjects/Backlog/Index" },
                new MisManualPage { Id = 107L, MisManualId = 16L, PageDescription = "Interactive drag-and-drop Kanban board for managing the active Sprint.", PageTitle = "Active Sprint Board", RouteKey = "/MisProjects/Board/Index" },
                new MisManualPage { Id = 201L, MisManualId = 16L, PageDescription = "Manage staff access and assign Scrum Master or Developer roles.", PageTitle = "Project Collaborators", RouteKey = "/MisProjects/Projects/Collaborators" },
                new MisManualPage { Id = 202L, MisManualId = 16L, PageDescription = "Your personal dashboard showing projects you are collaborating on.", PageTitle = "My Project Tasks", RouteKey = "/MisProjects/User/Index" },
                new MisManualPage { Id = 203L, MisManualId = 16L, PageDescription = "Form to initialize a new agile project container.", PageTitle = "Create Agile Project", RouteKey = "/MisProjects/Projects/Create" },
                new MisManualPage { Id = 204L, MisManualId = 16L, PageDescription = "Form to add a new Epic, User Story, Task, or Bug to the backlog.", PageTitle = "Create Work Item", RouteKey = "/MisProjects/Backlog/CreateTask" },
                new MisManualPage { Id = 205L, MisManualId = 16L, PageDescription = "Form to define the dates and goals for a new sprint.", PageTitle = "Create Sprint", RouteKey = "/MisProjects/Backlog/CreateSprint" },

                // --- MIS SMART ASSIST PAGES ---
                new MisManualPage { Id = 108L, MisManualId = 17L, PageDescription = "Secure admin panel to select the active AI provider and securely store BYOK API Keys.", PageTitle = "AI Provider Settings", RouteKey = "/MisSmartAssist/Dashboard/Config" },
                new MisManualPage { Id = 109L, MisManualId = 17L, PageDescription = "The primary interface for conversing with the configured AI companion.", PageTitle = "SmartAssist Chat", RouteKey = "/MisSmartAssist/Platform/Chat" },

                // --- MIS SYSTEM HOME ---
                new MisManualPage { Id = 200L, MisManualId = 8L, PageDescription = "The central dashboard giving an overview of all system modules and global metrics.", PageTitle = "Console Home", RouteKey = "/MIS/Dashboard/Index" },

                // --- MIS NEWSLETTER PAGES ---
                new MisManualPage { Id = 112L, MisManualId = 18L, PageDescription = "View the company's global email quota and allocate specific amounts to individual staff members.", PageTitle = "Global Wallet & Sharing", RouteKey = "/MisNewsletter/Dashboard/Wallet/Index" },
                new MisManualPage { Id = 113L, MisManualId = 18L, PageDescription = "Directory of all administrative mailing lists.", PageTitle = "Admin Mailing Lists", RouteKey = "/MisNewsletter/Dashboard/Lists/Index" },
                new MisManualPage { Id = 114L, MisManualId = 18L, PageDescription = "Create a new administrative mailing list.", PageTitle = "Create Admin List", RouteKey = "/MisNewsletter/Dashboard/Lists/Create" },
                new MisManualPage { Id = 115L, MisManualId = 18L, PageDescription = "Manage subscribers for this list. You can add them individually or import a CSV file.", PageTitle = "List Details & CSV Import", RouteKey = "/MisNewsletter/Dashboard/Lists/Details" },
                new MisManualPage { Id = 116L, MisManualId = 18L, PageDescription = "Directory of all scheduled, pending, or completed email campaigns.", PageTitle = "Admin Campaigns", RouteKey = "/MisNewsletter/Dashboard/Campaigns/Index" },
                new MisManualPage { Id = 117L, MisManualId = 18L, PageDescription = "Design your email campaign using the drag-and-drop editor and schedule its delivery.", PageTitle = "Campaign Builder (Admin)", RouteKey = "/MisNewsletter/Dashboard/Campaigns/Create" },
                
                new MisManualPage { Id = 118L, MisManualId = 18L, PageDescription = "View your personal email quota balance and browse available plans to purchase more.", PageTitle = "My Newsletter Wallet", RouteKey = "/MisNewsletter/Platform/Dashboard/Index" },
                new MisManualPage { Id = 119L, MisManualId = 18L, PageDescription = "Review the selected pricing plan details and proceed to secure payment.", PageTitle = "Purchase Plan Checkout", RouteKey = "/MisNewsletter/Platform/Dashboard/BuyPlan" },
                new MisManualPage { Id = 120L, MisManualId = 18L, PageDescription = "Directory of your personal mailing lists.", PageTitle = "My Mailing Lists", RouteKey = "/MisNewsletter/Platform/Lists/Index" },
                new MisManualPage { Id = 121L, MisManualId = 18L, PageDescription = "Create a new personal mailing list.", PageTitle = "Create Personal List", RouteKey = "/MisNewsletter/Platform/Lists/Create" },
                new MisManualPage { Id = 122L, MisManualId = 18L, PageDescription = "Manage your personal subscribers and upload CSV contact lists.", PageTitle = "Personal List Details", RouteKey = "/MisNewsletter/Platform/Lists/Details" },
                new MisManualPage { Id = 123L, MisManualId = 18L, PageDescription = "Directory of your personal email campaigns.", PageTitle = "My Campaigns", RouteKey = "/MisNewsletter/Platform/Campaigns/Index" },
                new MisManualPage { Id = 124L, MisManualId = 18L, PageDescription = "Design and schedule your personal email campaign.", PageTitle = "Campaign Builder (Personal)", RouteKey = "/MisNewsletter/Platform/Campaigns/Create" },

                // --- MISCELLANEOUS MISSING PAGES ---
                new MisManualPage { Id = 125L, MisManualId = 3L, PageDescription = "Portal to join the alumni network.", PageTitle = "Join Alumni", RouteKey = "/MisAlumni/Platform/Join" },
                new MisManualPage { Id = 126L, MisManualId = 6L, PageDescription = "List of overdue and unpaid financial obligations.", PageTitle = "Overdue Payments", RouteKey = "/MisFinances/Dashboard/Overdue" },
                new MisManualPage { Id = 127L, MisManualId = 11L, PageDescription = "Historical overview of completed grants and milestones.", PageTitle = "Grant History", RouteKey = "/MisGrants/Dashboard/History" },
                new MisManualPage { Id = 128L, MisManualId = 11L, PageDescription = "Detailed information regarding a specific grant assigned to you.", PageTitle = "My Grant Details", RouteKey = "/MisGrants/Platform/Details" },
                new MisManualPage { Id = 129L, MisManualId = 11L, PageDescription = "Overview of grants you are currently managing or tracking.", PageTitle = "My Grants", RouteKey = "/MisGrants/Platform/Index" },
                new MisManualPage { Id = 130L, MisManualId = 9L, PageDescription = "List of all active features, their billing status, and lifecycle management.", PageTitle = "Features Directory", RouteKey = "/MisManagement/Index" },
                new MisManualPage { Id = 131L, MisManualId = 9L, PageDescription = "Manage and view features that have been suspended due to billing or compliance issues.", PageTitle = "Suspended Features", RouteKey = "/MisManagement/SuspendedFeature" },
                new MisManualPage { Id = 132L, MisManualId = 9L, PageDescription = "Directory of all available pricing plans across all platform modules.", PageTitle = "Pricing Plans Directory", RouteKey = "/MisManagement/Plans/Index" },
                new MisManualPage { Id = 133L, MisManualId = 1L, PageDescription = "Modify an existing user's profile information.", PageTitle = "Edit User", RouteKey = "/MisUserManagement/Dashboard/Edit" },
                new MisManualPage { Id = 134L, MisManualId = 1L, PageDescription = "Administrative overview of all manual sections and configurations.", PageTitle = "User Manual Dashboard", RouteKey = "/MisUserManagement/Dashboard/UserManual" },
                new MisManualPage { Id = 135L, MisManualId = 1L, PageDescription = "Your personal user portal for managing profile and metrics.", PageTitle = "My Portal", RouteKey = "/MisUserManagement/Platform/Index" },

                // --- MIS MASS COMM PAGES ---
                new MisManualPage { Id = 136L, MisManualId = 19L, PageDescription = "Analytics dashboard showing SMS delivery rates and campaign stats.", PageTitle = "Mass Comm Analytics", RouteKey = "/MisMassComm/Dashboard/Index" },
                new MisManualPage { Id = 137L, MisManualId = 19L, PageDescription = "Global command center for sending bulk SMS campaigns.", PageTitle = "Admin SMS Campaigns", RouteKey = "/MisMassComm/Dashboard/Campaigns/Index" },
                new MisManualPage { Id = 138L, MisManualId = 19L, PageDescription = "Compose and schedule an SMS campaign to your contacts.", PageTitle = "Create SMS Campaign", RouteKey = "/MisMassComm/Dashboard/Campaigns/Create" },
                new MisManualPage { Id = 139L, MisManualId = 19L, PageDescription = "Global directory of all phone books.", PageTitle = "Admin Phone Books", RouteKey = "/MisMassComm/Dashboard/PhoneBooks/Index" },
                new MisManualPage { Id = 140L, MisManualId = 19L, PageDescription = "Create a new administrative phone book.", PageTitle = "Create Phone Book", RouteKey = "/MisMassComm/Dashboard/PhoneBooks/Create" },
                new MisManualPage { Id = 141L, MisManualId = 19L, PageDescription = "View all contacts within a specific phone book.", PageTitle = "Phone Book Contacts", RouteKey = "/MisMassComm/Dashboard/PhoneBooks/Contacts" },
                new MisManualPage { Id = 142L, MisManualId = 19L, PageDescription = "Upload a CSV of phone numbers into the phone book.", PageTitle = "Import Contacts", RouteKey = "/MisMassComm/Dashboard/PhoneBooks/Import" },
                new MisManualPage { Id = 143L, MisManualId = 19L, PageDescription = "Your personal SMS platform dashboard.", PageTitle = "My SMS Platform", RouteKey = "/MisMassComm/Platform/Index" },
                new MisManualPage { Id = 144L, MisManualId = 19L, PageDescription = "Your personal scheduled and sent SMS campaigns.", PageTitle = "My Campaigns", RouteKey = "/MisMassComm/Platform/Campaigns/Index" },
                new MisManualPage { Id = 145L, MisManualId = 19L, PageDescription = "Compose a new personal SMS campaign.", PageTitle = "Create Personal Campaign", RouteKey = "/MisMassComm/Platform/Campaigns/Create" },
                new MisManualPage { Id = 146L, MisManualId = 19L, PageDescription = "Your personal directory of phone books.", PageTitle = "My Phone Books", RouteKey = "/MisMassComm/Platform/PhoneBooks/Index" },
                new MisManualPage { Id = 147L, MisManualId = 19L, PageDescription = "Create a new personal phone book.", PageTitle = "Create Phone Book", RouteKey = "/MisMassComm/Platform/PhoneBooks/Create" },
                new MisManualPage { Id = 148L, MisManualId = 19L, PageDescription = "View and manage contacts in your phone book.", PageTitle = "My Contacts", RouteKey = "/MisMassComm/Platform/PhoneBooks/Contacts" },
                new MisManualPage { Id = 149L, MisManualId = 19L, PageDescription = "Bulk import numbers via CSV into your personal phone book.", PageTitle = "Import Personal Contacts", RouteKey = "/MisMassComm/Platform/PhoneBooks/Import" },
                new MisManualPage { Id = 150L, MisManualId = 19L, PageDescription = "Overview of your SMS wallet and transaction history.", PageTitle = "SMS Wallet", RouteKey = "/MisMassComm/Platform/Wallet/Index" }
            );

            // 3. Insert the Role-Aware Elements
            builder.Entity<MisManualElement>().HasData(
                // --- PAGE 1: Dashboard Elements ---
                new MisManualElement { Id = 1L, Description = "Click on any of the metric cards (Total, Active, Pending, Suspended) to instantly filter the user directory for that specific status.", ElementName = "User Status Metrics", ElementType = "Card", MisManualPageId = 1L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 2L, Description = "Navigates to the detailed tabular list of all users where you can filter, search, and perform bulk actions.", ElementName = "Manage Users Button", ElementType = "Button", MisManualPageId = 1L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 3L, Description = "Allows you to slice the demographic and registration data by a specific date range.", ElementName = "Filter Analytics Date Picker", ElementType = "Form", MisManualPageId = 1L, RequiredRole = "GlobalViewer" },

                // --- PAGE 2: User List Elements ---
                new MisManualElement { Id = 4L, Description = "Allows you to modify the user's basic profile information, contact details, and category.", ElementName = "Edit User Icon", ElementType = "Button", MisManualPageId = 2L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 5L, Description = "Immediately revokes or restores a user's ability to log into the platform.", ElementName = "Suspend/Activate Account", ElementType = "Toggle", MisManualPageId = 2L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 6L, Description = "Opens the comprehensive 360-degree view of the user, including their assigned assets, roles, and KPIs.", ElementName = "View Profile Details", ElementType = "Button", MisManualPageId = 2L, RequiredRole = "GlobalViewer" },

                // --- PAGE 3: Admin User Profile View Elements ---
                new MisManualElement { Id = 7L, Description = "Crucial security function: Grants this specific user access to a platform module (like Ticketing or AI Studio) with a specific permission level (e.g. GlobalViewer or SystemAdmin).", ElementName = "Assign Feature Role", ElementType = "Form", MisManualPageId = 3L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 8L, Description = "Allows administrators to attach internal notes to the user's profile. Checking 'Is Private' ensures the user cannot see this note.", ElementName = "Add HR Note", ElementType = "Form", MisManualPageId = 3L, RequiredRole = "RecordManager" },
                new MisManualElement { Id = 9L, Description = "Records a performance evaluation score for the user for a specific period.", ElementName = "Update KPI Score", ElementType = "Form", MisManualPageId = 3L, RequiredRole = "GlobalEditor" },

                // --- PAGE 4: End-User View Elements ---
                new MisManualElement { Id = 10L, Description = "Redirects you to the security settings to update your login credentials and enable 2-Factor Authentication.", ElementName = "Change Password", ElementType = "Button", MisManualPageId = 4L, RequiredRole = "" },
                new MisManualElement { Id = 11L, Description = "Allows you to see your historical performance scores and reviews entered by your managers.", ElementName = "View My KPIs", ElementType = "Tab", MisManualPageId = 4L, RequiredRole = "" },
                new MisManualElement { Id = 12L, Description = "Logs you securely out of the platform across all your devices.", ElementName = "Log Out", ElementType = "Button", MisManualPageId = 4L, RequiredRole = "" },

                // --- PAGE 5: Category Directory ---
                new MisManualElement { Id = 13L, Description = "Creates a new category grouping that users can be assigned to.", ElementName = "Add New Category", ElementType = "Button", MisManualPageId = 5L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 14L, Description = "Quickly find a category by its title or description.", ElementName = "Search Category", ElementType = "Input", MisManualPageId = 5L, RequiredRole = "GlobalViewer" },

                // --- PAGE 6: KPI Settings ---
                new MisManualElement { Id = 15L, Description = "Define a new performance metric (e.g., 'Punctuality', 'Sales Targets') and its maximum possible score.", ElementName = "Create Indicator", ElementType = "Button", MisManualPageId = 6L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 16L, Description = "Modify or remove an existing performance indicator.", ElementName = "Manage Indicators", ElementType = "Action", MisManualPageId = 6L, RequiredRole = "SystemAdmin" },

                // --- PAGE 7: KPI Assign ---
                new MisManualElement { Id = 17L, Description = "Input the numerical score achieved by the user for each active KPI.", ElementName = "Score Inputs", ElementType = "Form", MisManualPageId = 7L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 18L, Description = "Save the evaluation to the user's permanent performance record.", ElementName = "Submit Evaluation", ElementType = "Button", MisManualPageId = 7L, RequiredRole = "GlobalEditor" },

                // --- PAGE 8: KPI Results ---
                new MisManualElement { Id = 19L, Description = "Filter the leaderboard to view rankings for a specific month, quarter, or year.", ElementName = "Select Evaluation Period", ElementType = "Dropdown", MisManualPageId = 8L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 20L, Description = "Displays the cumulative score for each user alongside manager remarks.", ElementName = "Leaderboard Table", ElementType = "Table", MisManualPageId = 8L, RequiredRole = "GlobalViewer" },

                // --- PAGE 9: My KPIs ---
                new MisManualElement { Id = 21L, Description = "A historical list of every performance review you have received since joining.", ElementName = "Evaluation History", ElementType = "List", MisManualPageId = 9L, RequiredRole = "" },

                // --- PAGE 10: Roles ---
                new MisManualElement { Id = 22L, Description = "Select a platform module (e.g. Asset Management) and assign a permission level (e.g. GlobalViewer, RecordCreator, SystemAdmin).", ElementName = "Grant Permission", ElementType = "Form", MisManualPageId = 10L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 23L, Description = "Instantly strip this user's access to a specific module.", ElementName = "Revoke Role", ElementType = "Button", MisManualPageId = 10L, RequiredRole = "SystemAdmin" },

                // --- PAGE 11: My Roles ---
                new MisManualElement { Id = 24L, Description = "Displays a grid of every feature you can access and exactly what actions you are authorized to take.", ElementName = "Permissions Matrix", ElementType = "Table", MisManualPageId = 11L, RequiredRole = "" },

                // --- PAGE 12: Change Password ---
                new MisManualElement { Id = 25L, Description = "Change your login password. You must know your current password to set a new one.", ElementName = "Update Password", ElementType = "Form", MisManualPageId = 12L, RequiredRole = "" },
                new MisManualElement { Id = 26L, Description = "Scan the QR code with Google Authenticator or Authy to add a secondary security layer to your account.", ElementName = "Setup 2FA", ElementType = "QR Code", MisManualPageId = 12L, RequiredRole = "" },
                new MisManualElement { Id = 27L, Description = "Enter the 6-digit code from your app to finalize the 2FA setup.", ElementName = "Verify & Enable", ElementType = "Button", MisManualPageId = 12L, RequiredRole = "" },

                // --- MIS COMMITTEES ELEMENTS ---
                new MisManualElement { Id = 28L, Description = "Provides an analytical overview of active groups and memberships.", ElementName = "Dashboard Metrics", ElementType = "Card", MisManualPageId = 13L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 29L, Description = "Creates a new group/committee and sets its title and description.", ElementName = "Create Group", ElementType = "Button", MisManualPageId = 14L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 30L, Description = "Searches the user directory to add a member to this group with a specific role.", ElementName = "Add Member", ElementType = "Form", MisManualPageId = 15L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 31L, Description = "Review any pending invitations to join a new group. You can choose to Accept or Decline.", ElementName = "Pending Invites", ElementType = "Card", MisManualPageId = 16L, RequiredRole = "" },
                new MisManualElement { Id = 32L, Description = "A list of all groups you are an active member of. Click on any group to enter its private workspace.", ElementName = "Active Groups", ElementType = "List", MisManualPageId = 16L, RequiredRole = "" },
                new MisManualElement { Id = 33L, Description = "Post a new message to the group's discussion board.", ElementName = "New Discussion Post", ElementType = "Form", MisManualPageId = 17L, RequiredRole = "" },
                new MisManualElement { Id = 34L, Description = "Upload a document or file to share with the group.", ElementName = "Upload File", ElementType = "Button", MisManualPageId = 17L, RequiredRole = "" },
                new MisManualElement { Id = 35L, Description = "Fill out the required information to initialize a new group.", ElementName = "Create Form", ElementType = "Form", MisManualPageId = 18L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 36L, Description = "Modify the fundamental details of this group.", ElementName = "Edit Form", ElementType = "Form", MisManualPageId = 19L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 37L, Description = "Accept the invitation to join this group. You will either be granted immediate access or be placed in a pending queue for admin approval.", ElementName = "Join Request", ElementType = "Button", MisManualPageId = 20L, RequiredRole = "" },

                // --- MIS ALUMNI ELEMENTS ---
                new MisManualElement { Id = 38L, Description = "Click any analytical card to immediately navigate to a filtered view of the alumni roster.", ElementName = "Analytics Cards", ElementType = "Card", MisManualPageId = 21L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 39L, Description = "Review a pending user's request to join the Alumni network and grant them official access.", ElementName = "Verify Status", ElementType = "Button", MisManualPageId = 22L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 40L, Description = "Select an existing active user and explicitly grant them access to the Alumni Network.", ElementName = "Flag Alumni", ElementType = "Form", MisManualPageId = 23L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 41L, Description = "Quickly navigate to the searchable directory or the opportunities board.", ElementName = "Welcome Actions", ElementType = "Button", MisManualPageId = 24L, RequiredRole = "" },
                new MisManualElement { Id = 42L, Description = "Toggle this to only see members who have explicitly stated they are open to mentoring others.", ElementName = "Mentors Only Filter", ElementType = "Toggle", MisManualPageId = 25L, RequiredRole = "" },
                new MisManualElement { Id = 43L, Description = "Click to view the user's public LinkedIn profile in a new tab.", ElementName = "LinkedIn Connect", ElementType = "Button", MisManualPageId = 25L, RequiredRole = "" },
                new MisManualElement { Id = 44L, Description = "Filter the bulletin board to only show specific types of posts, like Job Openings.", ElementName = "Post Filters", ElementType = "Button", MisManualPageId = 26L, RequiredRole = "" },
                new MisManualElement { Id = 45L, Description = "Draft a new business or networking opportunity to share with the entire alumni community.", ElementName = "Create Post", ElementType = "Form", MisManualPageId = 26L, RequiredRole = "" },
                new MisManualElement { Id = 46L, Description = "Update your graduation year, current company, and professional title.", ElementName = "Profile Details", ElementType = "Form", MisManualPageId = 27L, RequiredRole = "" },
                new MisManualElement { Id = 47L, Description = "Enable this switch to let other alumni know you are willing to mentor them.", ElementName = "Mentorship Toggle", ElementType = "Toggle", MisManualPageId = 27L, RequiredRole = "" },

                // --- MIS REPORTS ELEMENTS ---
                new MisManualElement { Id = 48L, Description = "Visual representation of staff compliance over the last 7 days. Green indicates on-time submissions, yellow indicates late.", ElementName = "Submission Trends Chart", ElementType = "Chart", MisManualPageId = 28L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 49L, Description = "Toggle to immediately disable this report schema. Staff will no longer be required to submit it.", ElementName = "Active Toggle", ElementType = "Toggle", MisManualPageId = 29L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 50L, Description = "Add a staff member to the mandatory reporting list for this specific schema.", ElementName = "Enroll Staff", ElementType = "Form", MisManualPageId = 30L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 51L, Description = "View the full details of a submission, including time logs, file attachments, achievements, and challenges.", ElementName = "View Submission Modal", ElementType = "Button", MisManualPageId = 31L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 52L, Description = "Leave official feedback or a performance review directly on the user's report submission. The user will be able to see this comment.", ElementName = "Manager Comments Box", ElementType = "Form", MisManualPageId = 31L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 53L, Description = "A prominent card highlighting a report you must submit today to avoid a late penalty.", ElementName = "Pending Report Alert", ElementType = "Card", MisManualPageId = 32L, RequiredRole = "" },
                new MisManualElement { Id = 54L, Description = "Your historical log of previously submitted reports.", ElementName = "My History", ElementType = "Table", MisManualPageId = 32L, RequiredRole = "" },
                new MisManualElement { Id = 55L, Description = "Write and file your report. Based on the admin template, this form may dynamically ask you to upload a file or fill out an hourly log table.", ElementName = "Dynamic Submit Form", ElementType = "Form", MisManualPageId = 33L, RequiredRole = "" },
                new MisManualElement { Id = 56L, Description = "Check this box when creating a schema to force users to fill out an hourly breakdown of their activities.", ElementName = "Require Time Log Flag", ElementType = "Toggle", MisManualPageId = 29L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 57L, Description = "Check this box to require users to upload a PDF or Excel document alongside their report.", ElementName = "Require File Upload Flag", ElementType = "Toggle", MisManualPageId = 29L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 58L, Description = "A powerful shortcut to instantly assign a newly created report schema to every single active user in the organization.", ElementName = "Assign to All Active Users", ElementType = "Toggle", MisManualPageId = 29L, RequiredRole = "GlobalEditor" },

                // --- MIS FORMS ELEMENTS ---
                new MisManualElement { Id = 59L, Description = "Creates a new form definition that you can subsequently build out using the drag-and-drop interface.", ElementName = "New Form", ElementType = "Button", MisManualPageId = 34L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 60L, Description = "Draggable input fields like Textboxes, Checkboxes, and Date Pickers that you can drop onto the canvas.", ElementName = "Builder Toolbar", ElementType = "Panel", MisManualPageId = 36L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 61L, Description = "Generates the structural JSON payload and saves the entire layout to the database.", ElementName = "Save Form Layout", ElementType = "Button", MisManualPageId = 36L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 62L, Description = "Makes the form immediately available to the public or logged-in users depending on your security settings.", ElementName = "Publish Form", ElementType = "Button", MisManualPageId = 36L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 63L, Description = "Dynamically unpacks the complex JSON responses into clear, readable column headers matching your custom labels.", ElementName = "Dynamic Response Table", ElementType = "Table", MisManualPageId = 37L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 64L, Description = "Select a form from this dashboard to enter the dynamic submission interface.", ElementName = "Fill Form", ElementType = "Button", MisManualPageId = 38L, RequiredRole = "" },
                new MisManualElement { Id = 65L, Description = "Automatically packages your dynamic form inputs into a secure JSON string and saves it to the database.", ElementName = "Submit JSON Payload", ElementType = "Button", MisManualPageId = 39L, RequiredRole = "" },

                // --- REPORT WORKSPACE (Page 40) ---
                new MisManualElement { Id = 66L, Description = "Use the left and right arrows to navigate between months. The workspace will reload and display the analytics and calendar for the selected month.", ElementName = "Month Navigation Arrows", ElementType = "Button", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 67L, Description = "If a report is due today and the deadline has not passed, this banner will appear. Click 'Submit Today's Report' to immediately open the submission form.", ElementName = "Due Today Alert Banner", ElementType = "Card", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 68L, Description = "Shows the total number of reports submitted on time for the selected month. A high On-Time count reflects strong compliance.", ElementName = "On-Time KPI Card", ElementType = "Card", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 69L, Description = "Shows reports that were submitted after the daily deadline. Late submissions are still recorded but carry a warning flag in your compliance record.", ElementName = "Late KPI Card", ElementType = "Card", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 70L, Description = "Shows the count of due dates where no report was submitted at all. Missed reports negatively impact your overall compliance rate.", ElementName = "Missed KPI Card", ElementType = "Card", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 71L, Description = "Your overall submission compliance for the month. It is calculated as (On-Time + Late) / Total Due Days x 100. A 100% rate means you never missed a deadline.", ElementName = "Compliance Rate & Progress Bar", ElementType = "Chart", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 72L, Description = "A visual doughnut chart showing the proportion of On-Time (green), Late (orange), and Missed (red) reports for the selected month.", ElementName = "Month Performance Doughnut Chart", ElementType = "Chart", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 73L, Description = "Click 'Calendar View' to see a full monthly grid. Click 'List View' to see a table of all due dates. Your preference is saved automatically in your browser so it persists across sessions.", ElementName = "Calendar / List View Toggle", ElementType = "Toggle", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 74L, Description = "Each cell represents a day. Green cells (✓) are On-Time submissions. Orange cells (!) are Late submissions. Red cells (✗) are Missed deadlines. Blue dashed cells are pending today. White cells are non-due days.", ElementName = "Color-Coded Calendar Grid", ElementType = "Card", MisManualPageId = 40L, RequiredRole = "" },
                new MisManualElement { Id = 75L, Description = "Click on any green or orange calendar cell to jump directly to the submission detail page for that day's report.", ElementName = "Calendar Cell Clickable Links", ElementType = "Button", MisManualPageId = 40L, RequiredRole = "" },

                // --- REPORT DETAILS (Page 41) ---
                new MisManualElement { Id = 76L, Description = "Shows the full narrative report content as submitted by the user for that specific reporting period.", ElementName = "Report Content Body", ElementType = "Card", MisManualPageId = 41L, RequiredRole = "" },
                new MisManualElement { Id = 77L, Description = "If the report schema required hourly logs, this table displays every time slot with the submitted activity description. Break slots are shown as greyed-out rows.", ElementName = "Hourly Time Log Table", ElementType = "Table", MisManualPageId = 41L, RequiredRole = "" },
                new MisManualElement { Id = 78L, Description = "Displays the listed accomplishments and achievements the user reported for that day.", ElementName = "Achievements Section", ElementType = "Card", MisManualPageId = 41L, RequiredRole = "" },
                new MisManualElement { Id = 79L, Description = "Displays any issues, blockers, or challenges the user reported for that day.", ElementName = "Challenges Section", ElementType = "Card", MisManualPageId = 41L, RequiredRole = "" },
                new MisManualElement { Id = 80L, Description = "Shows the clickable attachment if a file was uploaded with this report submission.", ElementName = "File Attachment", ElementType = "Button", MisManualPageId = 41L, RequiredRole = "" },

                // --- EDIT REPORT SUBMISSION (Page 42) ---
                new MisManualElement { Id = 81L, Description = "Modify the main text content of your report. Changes can only be saved before the daily submission deadline.", ElementName = "Edit Report Content", ElementType = "Form", MisManualPageId = 42L, RequiredRole = "" },
                new MisManualElement { Id = 82L, Description = "Update the activity descriptions for each hourly time slot. Break rows are locked and cannot be edited.", ElementName = "Edit Time Log Entries", ElementType = "Form", MisManualPageId = 42L, RequiredRole = "" },
                new MisManualElement { Id = 83L, Description = "Save all changes. The system will re-evaluate whether the updated submission is still On-Time or Late based on the current server time.", ElementName = "Save Changes Button", ElementType = "Button", MisManualPageId = 42L, RequiredRole = "" },

                // --- MIS FINANCES ELEMENTS ---
                new MisManualElement { Id = 84L, Description = "Opens the 'New Payment Category' modal. Fill in the category title, description, type (compulsory or optional), frequency (One-Time, Monthly, Yearly, etc.), and optionally a fixed amount. If you leave the amount blank, the user or donor can choose their own amount when paying.", ElementName = "New Category Button", ElementType = "Button", MisManualPageId = 43L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 85L, Description = "When set to 'Compulsory Due', ALL active members are expected to pay this. It will appear in every user's 'Outstanding Payables' on their platform dashboard. Defaulters will automatically appear in the Ledger's Defaulters tab.", ElementName = "Payment Type: Compulsory Due", ElementType = "Setting", MisManualPageId = 43L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 86L, Description = "When set to 'Optional Donation', the payment is voluntary. It shows in the 'Support a Cause' section on the user's platform. No defaulter tracking is applied. Perfect for tithes, offerings, project-specific fundraising.", ElementName = "Payment Type: Optional Donation", ElementType = "Setting", MisManualPageId = 43L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 87L, Description = "Defines how often this payment is expected: One-Time (single occurrence), Daily, Weekly, Monthly, or Yearly. The system uses this for reporting and defaulter tracking.", ElementName = "Frequency", ElementType = "Setting", MisManualPageId = 43L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 88L, Description = "Each active category has a unique public payment link (e.g. /pay/5) that anyone — even visitors without an account — can use to make a payment. Copy the link and share it on WhatsApp, email, social media, or embed it on your website.", ElementName = "Public Link / Share Link", ElementType = "Column", MisManualPageId = 43L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 89L, Description = "Opens the payment ledger for this specific category. Shows who has paid, how much was collected, and (for compulsory dues) who the defaulters are.", ElementName = "Ledger Button", ElementType = "Button", MisManualPageId = 43L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 90L, Description = "Inactive categories no longer appear on the public payment portal or on the user's platform. Use this to pause a category without deleting its payment history.", ElementName = "Active / Inactive Toggle", ElementType = "Setting", MisManualPageId = 43L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 91L, Description = "Displays four key metrics: Total Payments, Revenue Collected, Users Paid, and Defaulters (only for compulsory categories). These are real-time calculations from the transaction log.", ElementName = "Summary Cards", ElementType = "Dashboard", MisManualPageId = 44L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 92L, Description = "Lists all users or guests who have successfully made a payment for this category, including the amount, date, and PixelPay transaction reference.", ElementName = "Paid Tab", ElementType = "Tab", MisManualPageId = 44L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 93L, Description = "Available only for Compulsory categories. Lists all registered platform members who have NOT yet paid. Use this to identify members to follow up with or remind via Mass Broadcast.", ElementName = "Defaulters Tab", ElementType = "Tab", MisManualPageId = 44L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 94L, Description = "Allows you to manually record a cash or offline payment on behalf of a member (e.g. someone paid at the office). This creates a verified payment entry in the ledger without going through PixelPay. Enter the payer's name, email, amount, and any relevant notes.", ElementName = "Record Manual Payment", ElementType = "Button", MisManualPageId = 44L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 95L, Description = "A unique code (e.g. TX-20260626-5-ABC123) that identifies every payment. Use this to trace a specific payment in your PixelPay merchant dashboard or resolve disputes.", ElementName = "Transaction Reference", ElementType = "Column", MisManualPageId = 45L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 96L, Description = "Three possible statuses: Pending (user started but hasn't completed payment), Paid (PixelPay confirmed success), Failed (payment was declined or cancelled). Status is updated automatically via the PixelPay webhook.", ElementName = "Payment Status", ElementType = "Column", MisManualPageId = 45L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 97L, Description = "If the payer was a registered member, their name links to their profile. If they were a guest (no account), their name and email as typed at checkout are displayed.", ElementName = "Donor vs. Member", ElementType = "Column", MisManualPageId = 45L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 98L, Description = "This section shows all compulsory dues that you have not yet paid. Each card shows the title, frequency, and fixed amount (if applicable). Click 'Pay Now' to be taken to the secure PixelPay checkout. Once your payment is confirmed, the card disappears automatically.", ElementName = "Outstanding Payables", ElementType = "Section", MisManualPageId = 46L, RequiredRole = "" },
                new MisManualElement { Id = 99L, Description = "Takes you to the PixelPay hosted checkout page. Because you are already logged in, your name and email are automatically filled in. You can pay by Card, Bank Transfer, USSD, or PayAttitude — all supported by PixelPay.", ElementName = "Pay Now Button", ElementType = "Button", MisManualPageId = 46L, RequiredRole = "" },
                new MisManualElement { Id = 100L, Description = "Copies the unique payment link to your clipboard. Share it with someone who wants to pay on your behalf, or use it to pay from a different device.", ElementName = "Share Button", ElementType = "Button", MisManualPageId = 46L, RequiredRole = "" },
                new MisManualElement { Id = 101L, Description = "Shows all optional donation categories set up by the admin. Contributions here are completely voluntary. Click 'Donate' to make a free-will contribution of any amount.", ElementName = "Support a Cause Section", ElementType = "Section", MisManualPageId = 46L, RequiredRole = "" },
                new MisManualElement { Id = 102L, Description = "A complete chronological log of all payments you have made. Shows the category, amount paid, transaction reference, payment status, and date. Pending payments will update to 'Paid' automatically once PixelPay confirms.", ElementName = "Payment History", ElementType = "Table", MisManualPageId = 46L, RequiredRole = "" },
                
                // --- NEW MIS FORMS SETTINGS ELEMENTS ---
                new MisManualElement { Id = 103L, Description = "Enable this switch to restrict form submission only to registered, logged-in users. When enabled, their user profile is permanently linked to their response. If disabled, the form is public to anyone with the link.", ElementName = "Require User Login Toggle", ElementType = "Setting", MisManualPageId = 35L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 104L, Description = "Enable this switch to allow users to submit this form/survey multiple times. If disabled, a user can only file one response; subsequent attempts will show a message that they have already submitted.", ElementName = "Allow Multiple Submissions Toggle", ElementType = "Setting", MisManualPageId = 35L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 105L, Description = "Enable this switch to allow logged-in users to access their past submissions. Once enabled, users will see a button to browse their submitted responses.", ElementName = "Allow User to View Responses Toggle", ElementType = "Setting", MisManualPageId = 35L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 106L, Description = "If you have already submitted a response and the administrator has enabled response viewing, this button will appear. Click it to view a summary of your past answers.", ElementName = "My Responses Link", ElementType = "Button", MisManualPageId = 38L, RequiredRole = "" },
                new MisManualElement { Id = 107L, Description = "Appears when you have already submitted a response to a single-submission form. The form is locked and cannot be filled out again.", ElementName = "Completed Badge", ElementType = "Badge", MisManualPageId = 38L, RequiredRole = "" },
                new MisManualElement { Id = 108L, Description = "A clear grid table showing the timestamp of each of your submissions along with the answers you provided for each question.", ElementName = "Submitted Responses List", ElementType = "Table", MisManualPageId = 47L, RequiredRole = "" },
                
                // --- ADDED MIS TICKETING ELEMENTS ---
                new MisManualElement { Id = 109L, Description = "Scan the QR code displayed on the guest's ticket.", ElementName = "QR Scanner", ElementType = "Scanner", MisManualPageId = 51L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 110L, Description = "Proceed to buy tickets for this event.", ElementName = "Get Tickets Button", ElementType = "Button", MisManualPageId = 52L, RequiredRole = "" },
                new MisManualElement { Id = 111L, Description = "Confirm order details and proceed to payment.", ElementName = "Pay Now Button", ElementType = "Button", MisManualPageId = 53L, RequiredRole = "" },
                new MisManualElement { Id = 112L, Description = "Browse upcoming events.", ElementName = "Events List", ElementType = "List", MisManualPageId = 54L, RequiredRole = "" },

                // --- ADDED MIS FINANCES ELEMENTS ---
                new MisManualElement { Id = 113L, Description = "View the global ledger of all system transactions.", ElementName = "Transaction Log", ElementType = "Table", MisManualPageId = 55L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 114L, Description = "Detailed receipt of the transaction.", ElementName = "Receipt Details", ElementType = "Card", MisManualPageId = 56L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 115L, Description = "High-level overview of revenue streams.", ElementName = "Financial Analytics", ElementType = "Dashboard", MisManualPageId = 57L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 116L, Description = "Your personal record of payments.", ElementName = "My Transaction Log", ElementType = "Table", MisManualPageId = 58L, RequiredRole = "" },

                // --- ADDED MIS USER MANAGEMENT ELEMENTS ---
                new MisManualElement { Id = 117L, Description = "Create the category.", ElementName = "Save Category", ElementType = "Button", MisManualPageId = 59L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 118L, Description = "Save the modifications.", ElementName = "Update Category", ElementType = "Button", MisManualPageId = 60L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 119L, Description = "Confirm deletion of category.", ElementName = "Delete Confirm", ElementType = "Button", MisManualPageId = 61L, RequiredRole = "SystemAdmin" },

                // --- ADDED MIS REPORTS ELEMENTS ---
                new MisManualElement { Id = 120L, Description = "Save the new schema definition.", ElementName = "Save Schema", ElementType = "Button", MisManualPageId = 62L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 121L, Description = "Update schema rules.", ElementName = "Update Schema", ElementType = "Button", MisManualPageId = 63L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 122L, Description = "View the specific data submitted in this report.", ElementName = "Submission Body", ElementType = "Card", MisManualPageId = 64L, RequiredRole = "GlobalViewer" },

                // --- ADDED MIS SETTING ELEMENTS ---
                new MisManualElement { Id = 123L, Description = "Apply global terminology updates.", ElementName = "Save Configuration", ElementType = "Button", MisManualPageId = 65L, RequiredRole = "SystemAdmin" },

                // --- ADDED MIS MANAGEMENT ELEMENTS ---
                new MisManualElement { Id = 124L, Description = "Save changes to feature configuration.", ElementName = "Save Config", ElementType = "Button", MisManualPageId = 66L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 125L, Description = "Create the new feature.", ElementName = "Register Feature", ElementType = "Button", MisManualPageId = 67L, RequiredRole = "SystemAdmin" },

                // --- ADDED MIS AUTH ELEMENTS ---
                new MisManualElement { Id = 126L, Description = "View the current 2FA status and available options.", ElementName = "2FA Dashboard", ElementType = "Card", MisManualPageId = 68L, RequiredRole = "" },
                new MisManualElement { Id = 127L, Description = "Store these codes safely.", ElementName = "Recovery Codes List", ElementType = "List", MisManualPageId = 69L, RequiredRole = "" },
                new MisManualElement { Id = 128L, Description = "Invalidate the current app keys.", ElementName = "Reset Keys", ElementType = "Button", MisManualPageId = 70L, RequiredRole = "" },
                new MisManualElement { Id = 129L, Description = "Create a new set of backup codes.", ElementName = "Generate Codes", ElementType = "Button", MisManualPageId = 71L, RequiredRole = "" },
                new MisManualElement { Id = 130L, Description = "Verify the app configuration.", ElementName = "Enable App", ElementType = "Button", MisManualPageId = 72L, RequiredRole = "" },
                new MisManualElement { Id = 131L, Description = "Disable 2FA entirely.", ElementName = "Disable 2FA", ElementType = "Button", MisManualPageId = 73L, RequiredRole = "" },

                // --- ADDED MIS GRANTS ELEMENTS ---
                new MisManualElement { Id = 132L, Description = "Export the grant ledger to CSV.", ElementName = "Export Ledger", ElementType = "Button", MisManualPageId = 74L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 133L, Description = "View or update detailed grant info.", ElementName = "Manage Grant", ElementType = "Button", MisManualPageId = 75L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 134L, Description = "Register a new external donor.", ElementName = "New Donor", ElementType = "Button", MisManualPageId = 77L, RequiredRole = "GlobalEditor" },

                // --- ADDED MIS PROPOSALS ELEMENTS ---
                new MisManualElement { Id = 135L, Description = "Approve a submitted proposal to move it to the execution phase.", ElementName = "Approve Proposal", ElementType = "Button", MisManualPageId = 80L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 136L, Description = "Reject a submitted proposal.", ElementName = "Reject Proposal", ElementType = "Button", MisManualPageId = 80L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 137L, Description = "Save changes to the proposal draft.", ElementName = "Save Draft", ElementType = "Button", MisManualPageId = 81L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 138L, Description = "Submit a new pitch to management.", ElementName = "Submit Pitch", ElementType = "Button", MisManualPageId = 84L, RequiredRole = "" },

                // --- MIS RECRUITMENT ELEMENTS ---
                new MisManualElement { Id = 139L, Description = "Provides an analytical overview of active jobs, applicant counts, and interviews.", ElementName = "ATS Metrics Dashboard", ElementType = "Card", MisManualPageId = 85L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 140L, Description = "Create a new job posting or manage an existing one.", ElementName = "Manage Job", ElementType = "Button", MisManualPageId = 86L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 141L, Description = "View the Kanban-style pipeline of all applicants for a specific role.", ElementName = "Applicant Pipeline", ElementType = "Dashboard", MisManualPageId = 88L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 142L, Description = "Progress an applicant to the next stage (e.g., Interview, Offer).", ElementName = "Change Status", ElementType = "Form", MisManualPageId = 89L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 143L, Description = "Schedule an offline or online interview and email the candidate.", ElementName = "Schedule Interview", ElementType = "Modal", MisManualPageId = 89L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 144L, Description = "Rate the candidate's interview performance based on predefined criteria.", ElementName = "Score Interview", ElementType = "Form", MisManualPageId = 89L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 145L, Description = "Automatically provision a real system account for an applicant who has been offered a role.", ElementName = "Employ Candidate", ElementType = "Button", MisManualPageId = 89L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 146L, Description = "Securely sign in or register to view your historical applications.", ElementName = "Applicant Auth Portal", ElementType = "Form", MisManualPageId = 90L, RequiredRole = "" },

                // --- MIS PROJECTS ELEMENTS ---
                new MisManualElement { Id = 147L, Description = "Top-level KPI tracking active Sprints, Total Tasks, and Items Done.", ElementName = "Agile Metrics", ElementType = "Card", MisManualPageId = 104L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 148L, Description = "Donut chart visualizing the distribution of tasks across statuses.", ElementName = "Global Task Status", ElementType = "Chart", MisManualPageId = 104L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 149L, Description = "Create a new top-level software or general tracking project.", ElementName = "New Project", ElementType = "Button", MisManualPageId = 105L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 150L, Description = "Quick jump to the active Kanban board for a project.", ElementName = "Active Board", ElementType = "Button", MisManualPageId = 105L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 151L, Description = "Unassigned tasks waiting to be picked up in future sprints.", ElementName = "Unassigned Backlog", ElementType = "List", MisManualPageId = 106L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 152L, Description = "Time-boxed iterations for work execution.", ElementName = "Sprints", ElementType = "List", MisManualPageId = 106L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 153L, Description = "Activate a planned sprint to transition it to the Board.", ElementName = "Start Sprint", ElementType = "Button", MisManualPageId = 106L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 154L, Description = "Drag a task from 'To Do' into 'In Progress'. The system auto-saves the new status.", ElementName = "Kanban Columns", ElementType = "Drag-and-Drop Area", MisManualPageId = 107L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 155L, Description = "Shows the Type (Bug/Story/Task), Points, and Assigned user for the item.", ElementName = "Task Card", ElementType = "Card", MisManualPageId = 107L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 163L, Description = "Takes you to the Collaborators page to assign team members.", ElementName = "Team / Settings", ElementType = "Button", MisManualPageId = 105L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 164L, Description = "Opens a form to add a new Epic, User Story, Task, or Bug to the backlog.", ElementName = "New Work Item", ElementType = "Button", MisManualPageId = 106L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 165L, Description = "Opens a form to define a new Sprint container.", ElementName = "New Sprint", ElementType = "Button", MisManualPageId = 106L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 166L, Description = "Add a user from the directory to the project team.", ElementName = "Add Team Member", ElementType = "Form", MisManualPageId = 201L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 167L, Description = "Remove a user's access to the project.", ElementName = "Remove User", ElementType = "Button", MisManualPageId = 201L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 168L, Description = "Quick jump to the active Kanban board for your assigned project.", ElementName = "Open Kanban Board", ElementType = "Button", MisManualPageId = 202L, RequiredRole = "" },
                new MisManualElement { Id = 169L, Description = "Save the new project details to the database.", ElementName = "Create Project", ElementType = "Button", MisManualPageId = 203L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 170L, Description = "Save the new work item to the unassigned backlog.", ElementName = "Add to Backlog", ElementType = "Button", MisManualPageId = 204L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 171L, Description = "Save the sprint definition to the project.", ElementName = "Create Sprint", ElementType = "Button", MisManualPageId = 205L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 172L, Description = "Navigate back to the Sprint Planning Backlog view.", ElementName = "Backlog & Planning", ElementType = "Button", MisManualPageId = 107L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 173L, Description = "Instantly add a new task directly to this active sprint.", ElementName = "New Task", ElementType = "Button", MisManualPageId = 107L, RequiredRole = "GlobalEditor" },

                // --- MIS SMART ASSIST ELEMENTS ---
                new MisManualElement { Id = 156L, Description = "Select which AI Provider (Gemini, OpenAI, Anthropic) will power the chat.", ElementName = "Active Provider Dropdown", ElementType = "Form Field", MisManualPageId = 108L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 157L, Description = "Save the API Keys to the database. These keys are never exposed directly to users.", ElementName = "Save Configuration", ElementType = "Button", MisManualPageId = 108L, RequiredRole = "SystemAdmin" },
                new MisManualElement { Id = 158L, Description = "Type your prompt or question here to speak to the AI.", ElementName = "Chat Input", ElementType = "Text Input", MisManualPageId = 109L, RequiredRole = "GlobalViewer" },
                new MisManualElement { Id = 159L, Description = "Send your message to the active AI provider.", ElementName = "Send Message", ElementType = "Button", MisManualPageId = 109L, RequiredRole = "GlobalViewer" },

                // --- MIS NEWSLETTER ELEMENTS ---
                new MisManualElement { Id = 160L, Description = "Transfers email quota directly from the global company pool to a specific user's wallet.", ElementName = "Share Quota Button", ElementType = "Button", MisManualPageId = 112L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 161L, Description = "Drag components from the right sidebar onto the canvas to design your email.", ElementName = "GrapesJS Editor", ElementType = "Drag-and-Drop Area", MisManualPageId = 117L, RequiredRole = "GlobalEditor" },
                new MisManualElement { Id = 162L, Description = "Choose a pricing package and securely checkout via Paystack to fund your wallet.", MisManualPageId = 119L, ElementName = "Buy Plan Button", ElementType = "Button", RequiredRole = "" }
            );
        }
    }
}
