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
                new MisManual { Id = 5L, ModuleFolder = "MisForms", Overview = "The Form Builder module allows administrators to create custom surveys, registration forms, or feedback modules using a drag-and-drop interface, with built-in analytical tracking for submissions.", Title = "Form Builder & Surveys Guide" }
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
                new MisManualPage { Id = 33L, MisManualId = 4L, PageDescription = "The dynamic form where you write and file your required report before the deadline. The fields you see are strictly defined by the Admin's template flags.", PageTitle = "Submit Report", RouteKey = "/MisReports/Platform/Submit" },
                new MisManualPage { Id = 40L, MisManualId = 4L, PageDescription = "Your dedicated workspace for this specific report schedule. View monthly analytics, check the visual calendar, and submit or edit your reports.", PageTitle = "Report Workspace", RouteKey = "/MisReports/Platform/SchemaDetails" },
                new MisManualPage { Id = 41L, MisManualId = 4L, PageDescription = "Detailed view of your filed report submission, detailing times logs, achievements, challenges, and attachments.", PageTitle = "Report Details", RouteKey = "/MisReports/Platform/Details" },
                new MisManualPage { Id = 42L, MisManualId = 4L, PageDescription = "Update your submitted report data and active time logs before the daily submission deadline cut-off.", PageTitle = "Edit Report Submission", RouteKey = "/MisReports/Platform/Edit" },

                // --- MIS FORMS PAGES ---
                new MisManualPage { Id = 34L, MisManualId = 5L, PageDescription = "The master list of all custom forms and surveys. You can track whether they are currently published or in draft mode.", PageTitle = "Forms Directory", RouteKey = "/MisForms/Dashboard/Index" },
                new MisManualPage { Id = 35L, MisManualId = 5L, PageDescription = "Initial setup page to define a new form's basic details like title, description, and whether it requires a user login.", PageTitle = "Create Form", RouteKey = "/MisForms/Dashboard/Create" },
                new MisManualPage { Id = 36L, MisManualId = 5L, PageDescription = "The visual Drag-and-Drop Form Builder interface. Drag elements from the panel to construct the exact data schema you want to collect.", PageTitle = "Drag-and-Drop Builder", RouteKey = "/MisForms/Dashboard/Builder" },
                new MisManualPage { Id = 37L, MisManualId = 5L, PageDescription = "A dynamically generated table that unpacks user JSON responses and displays them in a readable format matching your custom form labels.", PageTitle = "Form Responses Ledger", RouteKey = "/MisForms/Dashboard/Responses" },
                new MisManualPage { Id = 38L, MisManualId = 5L, PageDescription = "The user-facing dashboard displaying all active, published forms they are eligible to fill out.", PageTitle = "Available Forms", RouteKey = "/MisForms/Platform/Index" },
                new MisManualPage { Id = 39L, MisManualId = 5L, PageDescription = "The dynamic form-filling interface. It instantly translates the Admin's drag-and-drop schema into a functional HTML form.", PageTitle = "Submit Form", RouteKey = "/MisForms/Platform/Submit" }
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
                new MisManualElement { Id = 65L, Description = "Automatically packages your dynamic form inputs into a secure JSON string and saves it to the database.", ElementName = "Submit JSON Payload", ElementType = "Button", MisManualPageId = 39L, RequiredRole = "" }
            );
        }
    }
}
