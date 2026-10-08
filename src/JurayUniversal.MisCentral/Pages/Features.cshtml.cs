using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Pages
{
    public class FeaturesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FeaturesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FeatureItemDto> Features { get; set; } = new();

        public async Task OnGetAsync()
        {
            var dbFeatures = await _context.MasterFeatureCatalogs
                .Where(f => f.IsActive)
                .OrderBy(f => f.Category)
                .ThenBy(f => f.SortOrder)
                .ToListAsync();

            var defaultMap = GetAll16Features().ToDictionary(f => f.Code.ToUpperInvariant(), f => f);

            if (dbFeatures.Any())
            {
                Features = dbFeatures.Select(f =>
                {
                    var key = f.FeatureKey.ToUpperInvariant();
                    if (defaultMap.TryGetValue(key, out var meta))
                    {
                        return new FeatureItemDto
                        {
                            Code = f.FeatureKey,
                            Name = !string.IsNullOrWhiteSpace(f.Name) ? f.Name : meta.Name,
                            Category = !string.IsNullOrWhiteSpace(f.Category) ? f.Category : meta.Category,
                            Description = meta.Description,
                            LongDescription = meta.LongDescription,
                            KeyCapabilities = meta.KeyCapabilities,
                            BestFor = meta.BestFor,
                            Icon = meta.Icon
                        };
                    }

                    return new FeatureItemDto
                    {
                        Code = f.FeatureKey,
                        Name = f.Name,
                        Category = f.Category ?? "Enterprise Solution",
                        Description = !string.IsNullOrWhiteSpace(f.Description) ? f.Description : "Plug-and-play organizational management feature.",
                        LongDescription = "This feature provides specialized automation, granular staff access permissions, and automated reporting tailored for your institution.",
                        KeyCapabilities = new List<string> { "Fast cloud deployment", "Automated daily backups", "Granular role-based permissions", "Exportable Excel and PDF reports" },
                        BestFor = "Schools, Hospitals, and Growing Businesses",
                        Icon = "flaticon-it-service"
                    };
                }).ToList();
            }
            else
            {
                Features = GetAll16Features();
            }
        }

        public static List<FeatureItemDto> GetAll16Features() => new()
        {
            new()
            {
                Code = "MISACADEMICS",
                Name = "School & Student Portal",
                Category = "Education & Academics",
                Description = "Complete student lifecycle management from new admissions and class rosters to automated termly report cards and parent access.",
                LongDescription = "The School & Student Portal completely digitizes student records across primary, secondary, and tertiary institutions. Teachers input continuous assessments (CA) and exam scores online, while the system automatically calculates cumulative averages, positions, grade classifications, and generates print-ready graphical report cards with digital stamps.",
                KeyCapabilities = new List<string>
                {
                    "Online student admission applications and student ID generation",
                    "Automated termly and annual report cards with custom grading scales",
                    "Class attendance tracking with instant absentee alerts to parents",
                    "Secure parent/guardian portal to check grades and download result sheets"
                },
                BestFor = "Nursery, Primary, Secondary Schools, Colleges & Universities",
                Icon = "flaticon-inspiration"
            },
            new()
            {
                Code = "MISSTAFF",
                Name = "Staff Records & Automated Payroll",
                Category = "Human Resources & Payroll",
                Description = "Manage staff employment profiles, departments, monthly salary computations, tax deductions, allowances, and digital payslips.",
                LongDescription = "Eliminate manual salary calculations and payroll errors. Exwhyzee MIS Staff & Payroll automates monthly salary disbursements, PAYE tax, pension contributions, loans, and allowances. Staff members can view their monthly payslips and leave balances from their private mobile portal.",
                KeyCapabilities = new List<string>
                {
                    "One-click monthly payroll calculation across all staff ranks and scales",
                    "Automated tax (PAYE), pension, cooperative, and loan deduction rules",
                    "Self-service staff portal for leave requests and payslip downloads",
                    "Bank schedule generation compatible with all Nigerian commercial banks"
                },
                BestFor = "Schools, Corporate Enterprises, Hospitals & Government Agencies",
                Icon = "flaticon-technical-support"
            },
            new()
            {
                Code = "MISFINANCES",
                Name = "Fees, Invoicing & Card Payments",
                Category = "Finance & Accounting",
                Description = "Track school fees, generate electronic invoices, accept online card payments, track expenses, and view real-time financial statements.",
                LongDescription = "Say goodbye to fraudulent bank tellers and unrecorded payments. This feature generates unique student/customer fee schedules, accepts direct debit card payments, and issues instant electronic receipts while updating cashbooks and ledgers in real time.",
                KeyCapabilities = new List<string>
                {
                    "Online card and bank transfer payments with instant automated receipting",
                    "Custom fee structures for different classes, terms, and departments",
                    "Real-time revenue, expense tracking, debtor lists, and audit trails",
                    "Automatic SMS notification to parents upon verified payment confirmation"
                },
                BestFor = "Schools, Faith-Based Institutions, Hospitals & SMEs",
                Icon = "flaticon-data-access"
            },
            new()
            {
                Code = "MISCBT",
                Name = "Online CBT Exams & Instant Results",
                Category = "Assessments & Testing",
                Description = "Conduct computer-based tests and entrance examinations with randomized question banks, automatic scoring, and live result tallying.",
                LongDescription = "Run secure computer-based examinations on laptops, desktops, or tablets. The system supports multiple-choice, true/false, and short-answer questions with anti-cheating timers and question randomization. Results and answer scripts are scored immediately with zero human error.",
                KeyCapabilities = new List<string>
                {
                    "Central question bank with image, formula, and rich-text support",
                    "Anti-cheating exam timers, auto-submission, and randomized questions",
                    "Instant automated scoring with performance analytics by subject and class",
                    "Entrance screening exam mode for prospective applicant evaluation"
                },
                BestFor = "Schools, Screening Committees, Recruitment Agencies & Professional Institutes",
                Icon = "flaticon-creativity"
            },
            new()
            {
                Code = "MISVOTING",
                Name = "Digital E-Voting & Elections",
                Category = "Governance & Elections",
                Description = "Conduct 100% transparent, mobile-based electronic voting for alumni associations, student unions, cooperatives, and board elections.",
                LongDescription = "Exwhyzee Digital E-Voting enables thousands of eligible voters to cast their ballots from anywhere on their mobile phones. Featuring voter token authentication, live visual vote tallying, and tamper-proof audit trails to guarantee total trust and transparency.",
                KeyCapabilities = new List<string>
                {
                    "Unique one-time voter verification codes delivered via SMS/Email",
                    "Live, real-time visual vote tally dashboard with interactive charts",
                    "Multi-post ballot ballots (President, Secretary, Treasurer, etc.)",
                    "Tamper-proof audit logs and downloadable post-election certification"
                },
                BestFor = "Alumni Associations, Student Unions, Trade Unions, Clubs & Cooperatives",
                Icon = "flaticon-technology"
            },
            new()
            {
                Code = "MISNOTIFICATIONS",
                Name = "Instant Bulk SMS & Email Alerts",
                Category = "Communication & Alerts",
                Description = "Send instant transactional receipts, exam result notifications, emergency announcements, and termly notices to parents, students, and staff.",
                LongDescription = "Keep your entire community informed in seconds. Send branded SMS alerts (e.g. using your school's sender ID) and rich email bulletins directly from your dashboard for school reopening notices, fee reminders, bus arrival alerts, or emergency memos.",
                KeyCapabilities = new List<string>
                {
                    "Custom Sender ID delivery across all Nigerian mobile telecom networks",
                    "Targeted broadcast groups (All Parents, JSS 1 Class, Academic Staff, Debtors)",
                    "Automated triggers for fee payment receipts and published exam results",
                    "Detailed delivery reports and SMS unit wallet management"
                },
                BestFor = "All Institutions, Schools, Associations & Business Enterprises",
                Icon = "flaticon-extension"
            },
            new()
            {
                Code = "MISINVENTORY",
                Name = "Store, Supplies & Asset Tracking",
                Category = "Operations & Logistics",
                Description = "Monitor physical assets, uniforms, textbooks, laboratory equipment, office supplies, purchase orders, and store requisition approvals.",
                LongDescription = "Eliminate loss of institutional property and supplies. Track inventory levels, store issuing logs, vendor purchase orders, and fixed asset depreciation across multiple branches or campuses with automated reorder alerts.",
                KeyCapabilities = new List<string>
                {
                    "Multi-store inventory tracking with low-stock automatic alerts",
                    "Store requisition and staff approval workflows",
                    "Fixed asset tagging, location tracking, and depreciation schedules",
                    "Uniform, bookshop, and canteen point-of-sale management"
                },
                BestFor = "Schools, Hospitals, Manufacturing Firms & Corporate Offices",
                Icon = "flaticon-settings"
            },
            new()
            {
                Code = "MISTIMETABLE",
                Name = "Smart Class & Exam Scheduler",
                Category = "Academics & Operations",
                Description = "Automatically generate conflict-free weekly master timetables for subjects, teachers, classrooms, and exam invigilation halls.",
                LongDescription = "Creating a school timetable by hand can take weeks. The Smart Timetable Engine automatically balances teacher availability, classroom allocations, subject periods, and lab sessions to generate conflict-free schedules in minutes.",
                KeyCapabilities = new List<string>
                {
                    "Conflict-detection engine for teacher allocations and shared laboratory spaces",
                    "Printable master timetables, teacher-specific schedules, and class charts",
                    "Exam invigilation scheduling and hall capacity allocation",
                    "Mobile view for students and teachers to check daily periods"
                },
                BestFor = "Primary, Secondary Schools, Polytechnics & Universities",
                Icon = "flaticon-coding"
            },
            new()
            {
                Code = "MISHEALTH",
                Name = "Clinic & Healthcare Operations",
                Category = "Medical & Health",
                Description = "Patient and student health records, clinic consultations, pharmacy dispensing logs, medical vitals, and emergency contact alerts.",
                LongDescription = "Designed for school sickbays, corporate clinics, and healthcare centers. Track student health history, allergies, daily clinic visits, doctor prescriptions, and pharmacy stock with total patient confidentiality.",
                KeyCapabilities = new List<string>
                {
                    "Electronic medical records with allergy alerts and emergency contacts",
                    "Daily sickbay visit logs, consultation notes, and doctor prescriptions",
                    "Pharmacy dispensary tracking with medication expiry monitoring",
                    "Instant parent/guardian SMS alert when a student is admitted to the clinic"
                },
                BestFor = "School Sickbays, University Health Centers, Hospitals & Clinics",
                Icon = "flaticon-it-service"
            },
            new()
            {
                Code = "MISALUMNI",
                Name = "Alumni Community & Directory",
                Category = "Community & Network",
                Description = "Engage old students and graduates with a verified alumni database, graduation set directories, event tickets, and fundraising campaigns.",
                LongDescription = "Strengthen bonds with your graduating classes. The Alumni Portal gives graduates a dedicated platform to network, mentor current students, update their employment records, and contribute to institutional endowment projects.",
                KeyCapabilities = new List<string>
                {
                    "Graduation set directory with verified alumni search and profiles",
                    "Endowment fundraising, dues collection, and reunion ticket sales",
                    "Mentorship matching and job opportunity noticeboards",
                    "Executive committee election and chapter management"
                },
                BestFor = "Secondary School Old Students, University Alumni & Professional Bodies",
                Icon = "flaticon-it-service"
            },
            new()
            {
                Code = "MISGROUPS",
                Name = "Committees & Board Workspaces",
                Category = "Governance & Collaboration",
                Description = "Collaborative spaces for Board of Governors, PTA committees, academic councils, and departmental teams to share agendas and minutes.",
                LongDescription = "Streamline committee governance with dedicated board rooms. Members can review meeting agendas, vote on resolutions, access confidential documents, and maintain official historical minutes in one protected place.",
                KeyCapabilities = new List<string>
                {
                    "Board and committee workspaces with role-based document access",
                    "Meeting agenda builder, attendance registers, and minutes archiving",
                    "Resolution tracking and confidential electronic voting",
                    "PTA executive management and parent forum communication"
                },
                BestFor = "School Boards, PTA Executives, Academic Councils & Non-Profits",
                Icon = "flaticon-technology"
            },
            new()
            {
                Code = "MISUSERMGMT",
                Name = "Staff Roles & Security Permissions",
                Category = "Security & Administration",
                Description = "Centralized identity management, role-based access control (RBAC), multi-factor security, and complete audit trail logging.",
                LongDescription = "Ensure strict security compliance across your organization. Define custom roles (Principal, Accountant, Class Teacher, Exam Officer, Receptionist) and ensure each staff member accesses only the specific screens and reports they are authorized to see.",
                KeyCapabilities = new List<string>
                {
                    "Granular permission matrix across every menu, screen, and button",
                    "Two-factor authentication (2FA) for sensitive administrative roles",
                    "Comprehensive user activity audit trails with IP and timestamp logs",
                    "Instant one-click staff account deactivation upon departure"
                },
                BestFor = "All Organizations Requiring Strict Security & Data Compliance",
                Icon = "flaticon-technical-support"
            },
            new()
            {
                Code = "MISMANUAL",
                Name = "Operations Manual & SOP Guides",
                Category = "Standard Operating Procedures",
                Description = "Embedded institutional documentation, staff operating procedures (SOPs), employee handbooks, and step-by-step guides.",
                LongDescription = "Standardize how your organization runs. Embed institutional handbooks, academic policies, safety guidelines, and software tutorials directly into your portal so new staff onboard rapidly and consistently.",
                KeyCapabilities = new List<string>
                {
                    "Searchable institutional knowledge base and employee handbooks",
                    "Step-by-step Standard Operating Procedures (SOPs) for all departments",
                    "Video walkthroughs and downloadable operational templates",
                    "Version control and policy change acknowledgments"
                },
                BestFor = "Growing Schools, Corporate Firms, Government Parastatals & NGOs",
                Icon = "flaticon-extension"
            },
            new()
            {
                Code = "MISREPORTS",
                Name = "Executive Analytics & BI Insights",
                Category = "Intelligence & Analytics",
                Description = "Visual executive dashboards with revenue charts, student enrollment trends, staff attendance metrics, and compliance scorecards.",
                LongDescription = "Empower leadership with instant operational intelligence. Vice Chancellors, Principals, and CEOs get visual dashboards showing real-time revenue collection, student retention curves, staff performance, and budget utilization.",
                KeyCapabilities = new List<string>
                {
                    "Executive visual dashboards with real-time financial and enrollment charts",
                    "Comparative term-over-term and year-over-year performance trends",
                    "Customizable metric widgets and automated email summaries to leadership",
                    "One-click export to presentation-ready PDF and Excel summaries"
                },
                BestFor = "Executive Directors, School Owners, Boards & Managing Directors",
                Icon = "flaticon-data-access"
            },
            new()
            {
                Code = "MISHOSTEL",
                Name = "Hostel & Accommodation Manager",
                Category = "Campus Operations",
                Description = "Room allocation, hostel bed spaces, boarding house attendance, maintenance tickets, and residential student tracking.",
                LongDescription = "Manage boarding facilities effortlessly. Track room capacities, bed allocations, warden inspection logs, maintenance requests, and student exeat permits with automated parental notifications.",
                KeyCapabilities = new List<string>
                {
                    "Hostel building, room, and bed-space reservation matrix",
                    "Boarding student exeat permit workflow with parental SMS approvals",
                    "Hostel facility maintenance and repair ticket tracking",
                    "Daily boarding attendance and roll-call logs"
                },
                BestFor = "Boarding Schools, Colleges, Polytechnics & Universities",
                Icon = "flaticon-settings"
            },
            new()
            {
                Code = "MISTRANSPORT",
                Name = "Fleet & School Bus Logistics",
                Category = "Logistics & Transport",
                Description = "School bus route mapping, passenger student manifests, vehicle maintenance schedules, fuel tracking, and driver logs.",
                LongDescription = "Ensure student safety and optimize transport logistics. Assign students to bus routes and pickup points, track vehicle servicing schedules, and generate daily driver manifests with emergency contact numbers.",
                KeyCapabilities = new List<string>
                {
                    "Bus route mapping and student pickup/drop-off point assignment",
                    "Daily driver manifests with passenger roll calls",
                    "Vehicle maintenance reminders, insurance renewals, and fuel logs",
                    "Emergency parent notification for bus route delays"
                },
                BestFor = "Schools with Transport Services, Logistics & Fleet Operators",
                Icon = "flaticon-it-service"
            }
        };

        public class FeatureItemDto
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Category { get; set; } = "Enterprise Solution";
            public string Description { get; set; } = string.Empty;
            public string LongDescription { get; set; } = string.Empty;
            public List<string> KeyCapabilities { get; set; } = new();
            public string BestFor { get; set; } = string.Empty;
            public string Icon { get; set; } = "flaticon-it-service";
        }
    }
}
