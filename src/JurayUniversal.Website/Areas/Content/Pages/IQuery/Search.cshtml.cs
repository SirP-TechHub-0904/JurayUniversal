using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace JurayUniversal.Website.Areas.Content.Pages.IQuery
{
    [Authorize]
    public class SearchModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public List<SearchResultDto> Results { get; set; } = new();
        public int TotalMatches { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling((double)TotalMatches / PageSize);
        public SearchModel(DashboardDbContext context)
        {
            _context = context;
        }

        public void OnGet(string query, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(query))
                return;

            Results = new List<SearchResultDto>();
            if (User.Identity.IsAuthenticated && User.IsInRole("mSuperAdmin"))
            {
                SearchSettings(query);
            }
            SearchContent(query);
            SearchBlogs(query);
            SearchBlogCategories(query);
            SearchProducts(query);
            SearchProfiles(query);
            //SearchProjects(query);
            //SearchXProjects(query);
            //SearchJobs(query);
            //SearchReportsAndMeetings(query);
            //SearchTraining(query);
            //SearchCommunication(query);
            //SearchOperations(query);
            //SearchMisc(query);

            TotalMatches = Results.Count;
            CurrentPage = page < 1 ? 1 : page;

            Results = Results
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        #region Search Groups
        private string HighlightText(string text, string keyword)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(keyword))
                return text;

            return System.Text.RegularExpressions.Regex.Replace(
                text,
                $"({System.Text.RegularExpressions.Regex.Escape(keyword)})",
                "<span class='highlight'>$1</span>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }
        private void SearchSettings(string query)
        {
            var setting = _context.Settings.FirstOrDefault();
            if (setting != null)
                SearchObject(setting, query, "Setting", Results);

            var superSetting = _context.SuperSettings.FirstOrDefault();
            if (superSetting != null)
                SearchObject(superSetting, query, "SuperSetting", Results);

            var dashboardSetting = _context.DashboardSettings.FirstOrDefault();
            if (dashboardSetting != null)
                SearchObject(dashboardSetting, query, "DashboardSetting", Results);

            var dataConfig = _context.DataConfigs.FirstOrDefault();
            if (dataConfig != null)
                SearchObject(dataConfig, query, "DataConfig", Results);

            var smsSetting = _context.SmsSettings.FirstOrDefault();
            if (smsSetting != null)
                SearchObject(smsSetting, query, "SmsSetting", Results);

            var formSetting = _context.FormSettings.FirstOrDefault();
            if (formSetting != null)
                SearchObject(formSetting, query, "FormSetting", Results);

            var profilePortfolioSetting = _context.ProfilePortfolioSetting
                .Include(x => x.Profile)
                .ToList();

            foreach (var item in profilePortfolioSetting)
            {
                var profileName = item.Profile != null ? item.Profile.GetFullName() : "Unknown Profile";
                SearchObject(item, query, $"ProfilePortfolioSetting: {profileName}", Results);
            }
        }

        private void SearchContent(string query)
        {
            // =============================
            // LOAD EVERYTHING ONCE
            // =============================
            var allWebPages = _context.WebPages
                .Include(wp => wp.PageSections)
                    .ThenInclude(ps => ps.PageSectionLists)
                .ToList();

            var allSections = _context.PageSections
                .Include(ps => ps.PageSectionLists)
                .ToList();

            var allLists = _context.PageSectionLists.ToList();

            var processedWebPages = new HashSet<long>();
            var processedSections = new HashSet<long>();
            var processedLists = new HashSet<long>();

            // =============================
            // 1. CATEGORY → FULL TREE
            // =============================
            var categories = _context.PageCategories
                .Include(c => c.WebPages)
                .ToList();

            foreach (var cat in categories)
            {
                SearchObject(cat, query, $"PageCategory: {cat.Title}", Results);

                foreach (var wp in cat.WebPages ?? new List<WebPage>())
                {
                    var fullWp = allWebPages.FirstOrDefault(x => x.Id == wp.Id);
                    if (fullWp == null) continue;

                    processedWebPages.Add(fullWp.Id);

                    SearchObject(fullWp, query, $"PageCategory: {cat.Title} > WebPage: {fullWp.Title}", Results);

                    foreach (var sec in fullWp.PageSections ?? new List<PageSection>())
                    {
                        processedSections.Add(sec.Id);

                        SearchObject(sec, query, $"PageCategory: {cat.Title} > WebPage: {fullWp.Title} > Section: {sec.Title}", Results);

                        foreach (var list in sec.PageSectionLists ?? new List<PageSectionList>())
                        {
                            processedLists.Add(list.Id);

                            SearchObject(list, query,
                                $"PageCategory: {cat.Title} > WebPage: {fullWp.Title} > Section: {sec.Title} > List: {list.Title}",
                                Results);
                        }
                    }
                }
            }

            //
            // =============================
            // 2. WEBPAGES WITHOUT CATEGORY
            // =============================
            foreach (var wp in allWebPages)
            {
                if (processedWebPages.Contains(wp.Id))
                    continue;

                SearchObject(wp, query, $"WebPage (No Category): {wp.Title}", Results);

                foreach (var sec in wp.PageSections ?? new List<PageSection>())
                {
                    processedSections.Add(sec.Id);

                    SearchObject(sec, query, $"WebPage: {wp.Title} > Section: {sec.Title}", Results);

                    foreach (var list in sec.PageSectionLists ?? new List<PageSectionList>())
                    {
                        processedLists.Add(list.Id);

                        SearchObject(list, query,
                            $"WebPage: {wp.Title} > Section: {sec.Title} > List: {list.Title}",
                            Results);
                    }
                }
            }

            //
            // =============================
            // 3. SECTIONS WITHOUT WEBPAGE
            // =============================
            foreach (var sec in allSections)
            {
                if (processedSections.Contains(sec.Id))
                    continue;

                SearchObject(sec, query, $"PageSection (No WebPage): {sec.Title}", Results);

                foreach (var list in sec.PageSectionLists ?? new List<PageSectionList>())
                {
                    processedLists.Add(list.Id);

                    SearchObject(list, query,
                        $"PageSection: {sec.Title} > List: {list.Title}",
                        Results);
                }
            }

            //
            // =============================
            // 4. LISTS WITHOUT SECTION
            // =============================
            foreach (var list in allLists)
            {
                if (processedLists.Contains(list.Id))
                    continue;

                SearchObject(list, query, $"PageSectionList (Orphan): {list.Title}", Results);
            }

            var faqs = _context.FAQs.ToList();
            foreach (var faq in faqs)
                SearchObject(faq, query, $"FAQ: {faq.Title}", Results);

            var sliders = _context.Sliders.ToList();
            foreach (var slider in sliders)
                SearchObject(slider, query, $"Slider: {slider.Title}", Results);

            var testimonies = _context.Testimonies.ToList();
            foreach (var testimony in testimonies)
                SearchObject(testimony, query, $"Testimony: {testimony.Title}", Results);

            var postModals = _context.PostModal.ToList();
            foreach (var modal in postModals)
                SearchObject(modal, query, $"PostModal: {modal.Title}", Results);

            var templateCategories = _context.TemplateCategories
                .Include(x => x.TemplateTypes)
                .ToList();

            foreach (var category in templateCategories)
            {
                SearchObject(category, query, $"TemplateCategory: {category.TemplateName}", Results);

                foreach (var type in category.TemplateTypes ?? new List<TemplateType>())
                {
                    SearchObject(type, query, $"TemplateCategory: {category.TemplateName} > TemplateType: {type.Name}", Results);
                }
            }

            var portfolioTemplates = _context.PortfolioTemplates.ToList();
            foreach (var template in portfolioTemplates)
                SearchObject(template, query, $"PortfolioTemplate: {template.Name}", Results);
        }

        private void SearchBlogs(string query)
        {
            var blogs = _context.Blogs.ToList();

            foreach (var blog in blogs)
            {
                SearchObject(blog, query, $"Blog: {blog.Title}", Results);
            }
        }
        private void SearchBlogCategories(string query)
        {
            var blogCategories = _context.BlogCategories.ToList();

            foreach (var category in blogCategories)
            {
                SearchObject(category, query, $"BlogCategory: {category.Title}", Results);
            }
        }

        private void SearchProducts(string query)
        {
            var productCategories = _context.ProductCategories
                .Include(pc => pc.Product)
                    .ThenInclude(p => p.ProductFeatures)
                .Include(pc => pc.Product)
                    .ThenInclude(p => p.ProductSamples)
                .ToList();

            foreach (var category in productCategories)
            {
                SearchObject(category, query, $"ProductCategory: {category.Title}", Results);

                foreach (var product in category.Product ?? new List<Product>())
                {
                    SearchObject(product, query, $"ProductCategory: {category.Title} > Product: {product.Title}", Results);

                    foreach (var feature in product.ProductFeatures ?? new List<ProductFeature>())
                    {
                        SearchObject(feature, query, $"ProductCategory: {category.Title} > Product: {product.Title} > ProductFeature: {feature.Title}", Results);
                    }

                    foreach (var sample in product.ProductSamples ?? new List<ProductSample>())
                    {
                        SearchObject(sample, query, $"ProductCategory: {category.Title} > Product: {product.Title} > ProductSample: {sample.Title}", Results);
                    }
                }
            }
        }

        private void SearchProfiles(string query)
        {
            var profiles = _context.Users
                .Include(x => x.UserCategory)
                .Include(x => x.JobDesignation)
                .ToList();

            foreach (var profile in profiles)
            {
                SearchObject(profile, query, $"Profile: {profile.GetFullName()}", Results);
            }

            var userCategories = _context.UserCategories.ToList();
            foreach (var category in userCategories)
            {
                SearchObject(category, query, $"UserCategory: {category.Title}", Results);
            }
             
        }

        private void SearchProjects(string query)
        {
            var projectCategories = _context.ProjectCategories
                .Include(x => x.ProjectMains)
                    .ThenInclude(pm => pm.ProjectFeatures)
                        .ThenInclude(pf => pf.ProjectTasks)
                .Include(x => x.ProjectMains)
                    .ThenInclude(pm => pm.ProjectTeams)
                        .ThenInclude(pt => pt.ProjectUsers)
                .Include(x => x.ProjectMains)
                    .ThenInclude(pm => pm.ProjectFiles)
                .ToList();

            foreach (var category in projectCategories)
            {
                SearchObject(category, query, $"ProjectCategory: {category.Title}", Results);

                foreach (var project in category.ProjectMains ?? new List<ProjectMain>())
                {
                    SearchObject(project, query, $"ProjectCategory: {category.Title} > ProjectMain: {project.Title}", Results);

                    foreach (var feature in project.ProjectFeatures ?? new List<ProjectFeature>())
                    {
                        var featureTitle = feature.GeneralFeature != null ? feature.GeneralFeature.Title : feature.Id.ToString();
                        SearchObject(feature, query, $"ProjectMain: {project.Title} > ProjectFeature: {featureTitle}", Results);

                        foreach (var task in feature.ProjectTasks ?? new List<ProjectTask>())
                        {
                            SearchObject(task, query, $"ProjectMain: {project.Title} > ProjectFeature: {featureTitle} > ProjectTask: {task.Id}", Results);
                        }
                    }

                    foreach (var team in project.ProjectTeams ?? new List<ProjectTeam>())
                    {
                        SearchObject(team, query, $"ProjectMain: {project.Title} > ProjectTeam: {team.Title}", Results);

                        foreach (var user in team.ProjectUsers ?? new List<ProjectUser>())
                        {
                            SearchObject(user, query, $"ProjectMain: {project.Title} > ProjectTeam: {team.Title} > ProjectUser: {user.ProfileId}", Results);
                        }
                    }

                    foreach (var file in project.ProjectFiles ?? new List<ProjectFile>())
                    {
                        SearchObject(file, query, $"ProjectMain: {project.Title} > ProjectFile: {file.Title}", Results);
                    }
                }
            }

            var projectPositions = _context.ProjectPositions.ToList();
            foreach (var item in projectPositions)
                SearchObject(item, query, $"ProjectPosition: {item.Title}", Results);

            var generalFeatures = _context.GeneralFeatures.ToList();
            foreach (var item in generalFeatures)
                SearchObject(item, query, $"GeneralFeature: {item.Title}", Results);
        }

        private void SearchXProjects(string query)
        {
            var xProjects = _context.XProjects
                .Include(x => x.Sections)
                    .ThenInclude(s => s.Tasks)
                        .ThenInclude(t => t.Files)
                .Include(x => x.Messages)
                .ToList();

            foreach (var project in xProjects)
            {
                SearchObject(project, query, $"XProject: {project.Title}", Results);

                foreach (var section in project.Sections ?? new List<XProjectSection>())
                {
                    SearchObject(section, query, $"XProject: {project.Title} > Section: {section.Title}", Results);

                    foreach (var task in section.Tasks ?? new List<XProjectTask>())
                    {
                        SearchObject(task, query, $"XProject: {project.Title} > Section: {section.Title} > Task: {task.Title}", Results);

                        foreach (var file in task.Files ?? new List<XProjectFile>())
                        {
                            SearchObject(file, query, $"XProject: {project.Title} > Section: {section.Title} > Task: {task.Title} > File: {file.Title}", Results);
                        }
                    }
                }

                foreach (var message in project.Messages ?? new List<XProjectMessage>())
                {
                    SearchObject(message, query, $"XProject: {project.Title} > Message: {message.Id}", Results);
                }
            }
        }

        private void SearchJobs(string query)
        {
            var jobCategories = _context.JobCategories
                .Include(x => x.Plans)
                .Include(x => x.Jobs)
                    .ThenInclude(j => j.Features)
                .ToList();

            foreach (var category in jobCategories)
            {
                SearchObject(category, query, $"JobCategory: {category.Title}", Results);

                foreach (var plan in category.Plans ?? new List<Plan>())
                {
                    SearchObject(plan, query, $"JobCategory: {category.Title} > Plan: {plan.Title}", Results);
                }

                foreach (var job in category.Jobs ?? new List<Job>())
                {
                    SearchObject(job, query, $"JobCategory: {category.Title} > Job: {job.Title}", Results);

                    foreach (var feature in job.Features ?? new List<JobFeature>())
                    {
                        SearchObject(feature, query, $"JobCategory: {category.Title} > Job: {job.Title} > JobFeature: {feature.Title}", Results);
                    }
                }
            }
        }

        private void SearchReportsAndMeetings(string query)
        {
            var reports = _context.Report
                .Include(x => x.User)
                .ToList();

            foreach (var report in reports)
            {
                var fullName = report.User?.GetFullName() ?? report.UserId;
                SearchObject(report, query, $"Report: {fullName} > {report.Date:yyyy-MM-dd}", Results);
            }

            var meetings = _context.Meetings
                .Include(x => x.MeetingAttendances)
                .ToList();

            foreach (var meeting in meetings)
            {
                SearchObject(meeting, query, $"Meeting: {meeting.Id}", Results);
            }

            var proposals = _context.Proposals
                .Include(x => x.ProposalFile)
                .Include(x => x.SubmittedBy)
                .ToList();

            foreach (var proposal in proposals)
            {
                var fullName = proposal.SubmittedBy?.GetFullName() ?? proposal.SubmittedById;
                SearchObject(proposal, query, $"Proposal: {proposal.Title} > SubmittedBy: {fullName}", Results);

                foreach (var file in proposal.ProposalFile ?? new List<ProposalFile>())
                {
                    SearchObject(file, query, $"Proposal: {proposal.Title} > File: {file.Title}", Results);
                }
            }

            var salaries = _context.Salarys
                .Include(x => x.User)
                .ToList();

            foreach (var salary in salaries)
            {
                var fullName = salary.User?.GetFullName() ?? salary.UserId;
                SearchObject(salary, query, $"Salary: {fullName} > {salary.Month}", Results);
            }
        }

        private void SearchTraining(string query)
        {
            var trainings = _context.Trainings
                .Include(x => x.TrainingAttendances)
                .Include(x => x.Moderator)
                .Include(x => x.Trainner)
                .ToList();

            foreach (var training in trainings)
            {
                SearchObject(training, query, $"Training: {training.Topic}", Results);
            }

            var trainingSchedules = _context.TrainingSchedules.ToList();
            foreach (var schedule in trainingSchedules)
            {
                SearchObject(schedule, query, $"TrainingSchedule: {schedule.Topic}", Results);
            }
        }

        private void SearchCommunication(string query)
        {
            var smsCategories = _context.SmsMessageCategories
                .Include(x => x.SmsList)
                .ToList();

            foreach (var category in smsCategories)
            {
                SearchObject(category, query, $"SmsMessageCategory: {category.Title}", Results);
            }

            var smsMessages = _context.SmsMessages
                .Include(x => x.SmsMessageCategory)
                .ToList();

            foreach (var sms in smsMessages)
            {
                SearchObject(sms, query, $"SmsMessage: {sms.SenderId}", Results);
            }

            var senderIds = _context.SenderIds.ToList();
            foreach (var sender in senderIds)
            {
                SearchObject(sender, query, $"SenderId: {sender.Title}", Results);
            }

            var messages = _context.Messages.ToList();
            foreach (var message in messages)
            {
                SearchObject(message, query, $"Message: {message.Title}", Results);
            }

            var mailingSystems = _context.MailingSystems.ToList();
            foreach (var mail in mailingSystems)
            {
                SearchObject(mail, query, $"MailingSystem: {mail.Title}", Results);
            }

            var newsletters = _context.NewsLetter.ToList();
            foreach (var item in newsletters)
            {
                SearchObject(item, query, $"NewsLetter: {item.Email}", Results);
            }

            var priceSettings = _context.PriceSettings
                .Include(x => x.DialCodes)
                .ToList();

            foreach (var price in priceSettings)
            {
                SearchObject(price, query, $"PriceSetting: {price.Country} - {price.NetworkProvider}", Results);

                foreach (var dial in price.DialCodes ?? new List<DialCode>())
                {
                    SearchObject(dial, query, $"PriceSetting: {price.Country} - {price.NetworkProvider} > DialCode: {dial.NumberPrefix}", Results);
                }
            }
        }

        private void SearchOperations(string query)
        {
            var officeActivities = _context.OfficeActivityCategories
                .Include(x => x.OfficeActivityDialies)
                .ToList();

            foreach (var category in officeActivities)
            {
                SearchObject(category, query, $"OfficeActivityCategory: {category.Title}", Results);

                foreach (var daily in category.OfficeActivityDialies ?? new List<OfficeActivityDialy>())
                {
                    SearchObject(daily, query, $"OfficeActivityCategory: {category.Title} > OfficeActivityDaily: {daily.Date:yyyy-MM-dd}", Results);
                }
            }

            var expensesCategories = _context.ExpensesCategories.ToList();
            foreach (var expense in expensesCategories)
            {
                SearchObject(expense, query, $"ExpensesCategory: {expense.Title}", Results);
            }

            var fundSources = _context.FundSources
                .Include(x => x.User)
                .ToList();

            foreach (var source in fundSources)
            {
                var fullName = source.User?.GetFullName() ?? source.UserId;
                SearchObject(source, query, $"FundSource: {fullName}", Results);
            }

            var facilities = _context.Facilities.ToList();
            foreach (var facility in facilities)
            {
                SearchObject(facility, query, $"Facility: {facility.State} > {facility.LGA}", Results);
            }

            var enableDevices = _context.EnableDevices.ToList();
            foreach (var device in enableDevices)
            {
                SearchObject(device, query, $"EnableDevice: {device.Id}", Results);
            }
        }

        private void SearchMisc(string query)
        {
            var dashboardData = _context.DashboardDatas
                .Include(x => x.DashboardDataLists)
                .ToList();

            foreach (var data in dashboardData)
            {
                SearchObject(data, query, $"DashboardData: {data.Title}", Results);

                foreach (var item in data.DashboardDataLists ?? new List<DashboardDataList>())
                {
                    SearchObject(item, query, $"DashboardData: {data.Title} > DashboardDataList: {item.Id}", Results);
                }
            }

            var states = _context.States.ToList();
            foreach (var state in states)
            {
                SearchObject(state, query, $"State: {state.StateName}", Results);
            }

            var lgas = _context.LocalGoverments.ToList();
            foreach (var lga in lgas)
            {
                SearchObject(lga, query, $"LocalGovernment: {lga.LGAName}", Results);
            }
        }

        #endregion

        #region Recursive WebPage Search

        private void SearchWebPageRecursive(WebPage wp, string query, string path, List<SearchResultDto> results)
        {
            SearchObject(wp, query, path, results);

            foreach (var section in wp.PageSections ?? new List<PageSection>())
            {
                SearchObject(section, query, $"{path} > PageSection: {section.Title}", results);

                foreach (var list in section.PageSectionLists ?? new List<PageSectionList>())
                {
                    SearchObject(list, query, $"{path} > PageSection: {section.Title} > ListItem: {list.Title}", results);
                }
            }


        }

        #endregion

        #region Reflection Search

        private void SearchObject(object obj, string keyword, string path, List<SearchResultDto> results)
        {
            if (obj == null || string.IsNullOrWhiteSpace(keyword))
                return;

            var normalizedKeyword = keyword.Trim().ToLowerInvariant();

            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.PropertyType != typeof(string))
                    continue;

                var rawValue = prop.GetValue(obj) as string;
                if (string.IsNullOrWhiteSpace(rawValue))
                    continue;

                var normalizedValue = rawValue.ToLowerInvariant();

                if (normalizedValue.Contains(normalizedKeyword))
                {
                    var pathUrl = GetEditPath(obj);

                    results.Add(new SearchResultDto
                    {
                        Path = pathUrl,
                        PathText = path,
                        EntityType = obj.GetType().Name,
                        Field = prop.Name,
                        Value = HighlightText(rawValue, keyword),
                        MatchedText = keyword
                    });
                }
            }
        }

        private string GetEditPath(object obj)
        {
            return obj switch
            {
                PageCategory x => $"/Content/IPages/Category/edit?id={x.Id}",
                WebPage x => $"/Content/IPages/Main/edit?id={x.Id}",
                PageSection x => $"/Content/IPages/Section/edit?id={x.Id}",
                PageSectionList x => $"/Content/IPages/List/edit?id={x.Id}",

                BlogCategory x => $"/Content/IBlogs/Category/Edit?id={x.Id}",
                Blog x => $"/Content/IBlogs/Post/Edit?id={x.Id}",


                Setting => "/Content/ISettings",
                SuperSetting => "/Management/Dashboard",
                DashboardSetting => "Management/IDashboardSettings",
                DataConfig => "Management/IConfigSettings",

                FAQ x => $"/Content/IFaq/edit?id={x.Id}",
                Slider x => $"/Content/ISliders/edit?id={x.Id}",
                Testimony x => $"/Content/ITestimony/edit?id={x.Id}",
                PostModal x => $"//Content/IModal/edit?id={x.Id}",


                TemplateCategory x => $"/admin/templatecategory/edit?id={x.Id}",
                TemplateType x => $"/admin/templatetype/edit?id={x.Id}",
                PortfolioTemplate x => $"/admin/portfoliotemplate/edit?id={x.Id}",

                ProductCategory x => $"/Content/IProduct/Category/edit?id={x.Id}",
                Product x => $"/Content/IProduct/Main/edit?id={x.Id}",
                ProductFeature x => $"/Content/IProduct/IFeatures/edit?id={x.Id}",
                ProductSample x => $"/Content/IProduct/ISample/edit?id={x.Id}",

                Profile x => $"/admin/UserManagement/edit?id={x.Id}",
                UserCategory x => $"/content/IUserCategory/edit?id={x.Id}",

                
                ProjectCategory x => $"/content/IContinue/Project/Category/edit?id={x.Id}",
                


                XProject x => $"/admin/xproject/edit?id={x.Id}",
                XProjectSection x => $"/admin/xprojectsection/edit?id={x.Id}",
                XProjectTask x => $"/admin/xprojecttask/edit?id={x.Id}",
                XProjectFile x => $"/admin/xprojectfile/edit?id={x.Id}",
                XProjectMessage x => $"/admin/xprojectmessage/edit?id={x.Id}",

                JobCategory x => $"/admin/jobcategory/edit?id={x.Id}",
                Job x => $"/admin/job/edit?id={x.Id}",
                JobFeature x => $"/admin/jobfeature/edit?id={x.Id}",
                Plan x => $"/admin/plan/edit?id={x.Id}",

                Report x => $"/admin/report/edit?id={x.Id}",
                Meeting x => $"/admin/meeting/edit?id={x.Id}",
                Proposal x => $"/admin/proposal/edit?id={x.Id}",
                ProposalFile x => $"/admin/proposalfile/edit?id={x.Id}",
                Salary x => $"/admin/salary/edit?id={x.Id}",

                Training x => $"/admin/training/edit?id={x.Id}",
                TrainingSchedule x => $"/admin/trainingschedule/edit?id={x.Id}",

                SmsMessageCategory x => $"/admin/smsmessagecategory/edit?id={x.Id}",
                SmsMessage x => $"/admin/smsmessage/edit?id={x.Id}",
                SenderId x => $"/admin/senderid/edit?id={x.Id}",
                Message x => $"/admin/message/edit?id={x.Id}",
                MailingSystem x => $"/admin/mailingsystem/edit?id={x.Id}",
                NewsLetter x => $"/admin/newsletter/edit?id={x.Id}",
                PriceSetting x => $"/admin/pricesetting/edit?id={x.Id}",
                DialCode x => $"/admin/dialcode/edit?id={x.Id}",

                OfficeActivityCategory x => $"/admin/officeactivitycategory/edit?id={x.Id}",
                OfficeActivityDialy x => $"/admin/officeactivitydaily/edit?id={x.Id}",
                ExpensesCategory x => $"/admin/expensescategory/edit?id={x.Id}",
                FundSource x => $"/admin/fundsource/edit?id={x.Id}",
                Facility x => $"/admin/facility/edit?id={x.Id}",
                EnableDevice x => $"/admin/enabledevice/edit?id={x.Id}",

                DashboardData x => $"/admin/dashboarddata/edit?id={x.Id}",
                DashboardDataList x => $"/admin/dashboarddatalist/edit?id={x.Id}",
                State x => $"/admin/state/edit?id={x.Id}",
                LocalGoverment x => $"/admin/localgovernment/edit?id={x.Id}",

                _ => "#"
            };
        }

        #endregion
    }

    public class SearchResultDto
    {
        public string Path { get; set; } = "#";
        public string PathText { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string MatchedText { get; set; } = string.Empty;
    }
}
