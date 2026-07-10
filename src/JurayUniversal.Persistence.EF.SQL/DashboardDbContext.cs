using JurayUniversal.Domain;
using JurayUniversal.Domain.Models;
using JurayUniversal.Infrastructure.TenantSupport;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using JurayUniversal.Domain.Models.NIPSS;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;


namespace JurayUniversal.Persistence.EF.SQL
{
public class DashboardDbContext : IdentityDbContext<Profile, AppRole, string>
{
    private readonly ITenantProvider _tenantProvider;


    public DbSet<SmsMessage> SmsMessages { get; set; } = null!;
    public DbSet<SmsMessageCategory> SmsMessageCategories { get; set; } = null!;
    public DbSet<SenderId> SenderIds { get; set; } = null!;


    //public DbSet<--> -- { get; set; } = null!;
    public DbSet<SmsSetting> SmsSettings { get; set; } = null!;
    public DbSet<DialCode> DialCodes { get; set; } = null!;
    public DbSet<PriceSetting> PriceSettings { get; set; } = null!;
    public DbSet<SuperSetting> SuperSettings { get; set; } = null!;
    public DbSet<MisSetting> MisSettings { get; set; } = null!;
    public DbSet<MisWidget> MisWidgets { get; set; } = null!;
    public DbSet<MisWizardStep> MisWizardSteps { get; set; } = null!;
    public DbSet<MisLeaveRequest> MisLeaveRequests { get; set; } = null!;
    public DbSet<MisPhoneBook> MisPhoneBooks { get; set; } = null!;
    public DbSet<MisNewsletterPlan> MisNewsletterPlans { get; set; }
    public DbSet<MisNewsletterPlanTransaction> MisNewsletterPlanTransactions { get; set; }
    public DbSet<MisNewsletterWallet> MisNewsletterWallets { get; set; }
    public DbSet<MisNewsletterWalletTransaction> MisNewsletterWalletTransactions { get; set; }
    public DbSet<MisNewsletterCampaign> MisNewsletterCampaigns { get; set; }
    public DbSet<MisNewsletterRecipient> MisNewsletterRecipients { get; set; }
    public DbSet<MisNewsletterList> MisNewsletterLists { get; set; }
    public DbSet<MisNewsletterContact> MisNewsletterContacts { get; set; }
    public DbSet<MisContact> MisContacts { get; set; } = null!;
    public DbSet<MisUserRecentVisit> MisUserRecentVisits { get; set; } = null!;
    public DbSet<MisWallet> MisWallets { get; set; } = null!;
    public DbSet<MisWalletTransaction> MisWalletTransactions { get; set; } = null!;
    public DbSet<MisSmsTransaction> MisSmsTransactions { get; set; } = null!;
    public DbSet<MisSmsCampaign> MisSmsCampaigns { get; set; } = null!;
    public DbSet<MisSmsRecipient> MisSmsRecipients { get; set; } = null!;
    public DbSet<MisSmsSenderId> MisSmsSenderIds { get; set; } = null!;
    public DbSet<EnableDevice> EnableDevices { get; set; } = null!;
    public DbSet<TemplateCategory> TemplateCategories { get; set; } = null!;
    public DbSet<TemplateType> TemplateTypes { get; set; } = null!;
    public DbSet<MailingSystem> MailingSystems { get; set; } = null!;
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<WebPage> WebPages { get; set; }
    public DbSet<FAQ> FAQs { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<PageSection> PageSections { get; set; }
    public DbSet<PageSectionList> PageSectionLists { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<PageCategory> PageCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }
    public DbSet<ProductSample> ProductSamples { get; set; }
    public DbSet<Testimony> Testimonies { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<NewsLetter> NewsLetter { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }

    //project dbset
    public DbSet<ProjectCategory> ProjectCategories { get; set; }

    public DbSet<ProjectMain> ProjectMains { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<ProjectTeam> ProjectTeams { get; set; }
    public DbSet<ProjectPosition> ProjectPositions { get; set; }
    public DbSet<ProjectFile> ProjectFiles { get; set; }
    public DbSet<ProjectFeature> ProjectFeatures { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<GeneralFeature> GeneralFeatures { get; set; }
    public DbSet<PostModal> PostModal { get; set; }
    public DbSet<DataConfig> DataConfigs { get; set; }







    public DbSet<Message> Messages { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<ClientRequest> ClientRequest { get; set; }
    public DbSet<Report> Report { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<MeetingAttendance> MeetingAttendances { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<TrainingAttendance> TrainingAttendances { get; set; }
    public DbSet<TrainingSchedule> TrainingSchedules { get; set; }




    public DbSet<Plan> Plans { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<JobFeature> JobFeatures { get; set; }
    public DbSet<Job> Jobs { get; set; }

    public DbSet<FundSource> FundSources { get; set; }
    public DbSet<CompanyFund> CompanyFunds { get; set; }
    public DbSet<ExpensesCategory> ExpensesCategories { get; set; }
    public DbSet<CompayExpenses> CompayExpenses { get; set; }
    public DbSet<Salary> Salarys { get; set; }
    public DbSet<JobDesignation> JobDesignations { get; set; }


    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<ProposalFile> ProposalFiles { get; set; }
    public DbSet<ChatMessage> ChatMessage { get; set; }
    public DbSet<CareerFile> CareerFiles { get; set; }
    public DbSet<CareerPath> CareerPaths { get; set; }

    public DbSet<LocalGoverment> LocalGoverments { get; set; }
    public DbSet<State> States { get; set; }





    public DbSet<AdditionalProfile> AdditionalProfiles { get; set; }
    public DbSet<ProfileCategory> ProfileCategories { get; set; }
    public DbSet<ProfileCategoryList> ProfileCategoryLists { get; set; }
    public DbSet<ProfilePortfolio> ProfilePortfolios { get; set; }
    public DbSet<ProfilePortfolioSetting> ProfilePortfolioSetting { get; set; }
    public DbSet<PortfolioContactUs> PortfolioContactUses { get; set; }
    public DbSet<PortfolioTemplate> PortfolioTemplates { get; set; }


    public DbSet<OfficeActivityCategory> OfficeActivityCategories { get; set; }
    public DbSet<OfficeActivityDialy> OfficeActivityDialies { get; set; }



    public DbSet<FormSetting> FormSettings { get; set; }
    public DbSet<DashboardSetting> DashboardSettings { get; set; }
    public DbSet<DashboardData> DashboardDatas { get; set; }
    public DbSet<DashboardDataList> DashboardDataLists { get; set; }
    public DbSet<CompanyProgram> CompanyPrograms { get; set; }
    public DbSet<CompanyProgramCategory> CompanyProgramCategories { get; set; }
    public DbSet<ProgramUser> ProgramUsers { get; set; }
    public DbSet<UserCategory> UserCategories { get; set; }


    public DbSet<XProject> XProjects { get; set; }
    public DbSet<XProjectSection> XProjectSections { get; set; }
    public DbSet<XProjectTask> XProjectTasks { get; set; }
    public DbSet<XProjectMessage> XProjectMessages { get; set; }
    public DbSet<XProjectFile> XProjectFiles { get; set; }


    public DbSet<BudgetMainCategory> BudgetMainCategories { get; set; }
    public DbSet<BudgetSubCategory> BudgetSubCategories { get; set; }
    public DbSet<BudgetList> BudgetLists { get; set; }

    public DbSet<TitleList> TitleLists { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseSubTheme> CourseSubThemes { get; set; }
    public DbSet<ParticipantList> ParticipantLists { get; set; }
    public DbSet<CourseCategory> CourseCategories { get; set; }


    public DbSet<Accomodation> Accomodations { get; set; }
    public DbSet<StudyGroup> StudyGroups { get; set; }
    public DbSet<StudyGroupCategory> StudyGroupCategory { get; set; }
    public DbSet<StaffManager> StaffManagers { get; set; }


    public DbSet<NutritionCategory> NutritionCategories { get; set; }
    public DbSet<NutritionCategoryList> NutritionCategoryLists { get; set; }

    public DbSet<UserNutritionEvaluation> UserNutritionEvaluations { get; set; }
    public DbSet<TimeTable> TimeTables { get; set; }
    public DbSet<Moderator> Moderators { get; set; }



    public DbSet<Evoting> Evotings { get; set; }
    public DbSet<VoteCategory> VoteCategories { get; set; }
    public DbSet<UserVote> UserVotes { get; set; }
    public DbSet<VoteCondidate> VoteCondidates { get; set; }

    // MIS Projects
    public DbSet<MisProject> MisProjects { get; set; }
    public DbSet<MisProjectSprint> MisProjectSprints { get; set; }
    public DbSet<MisProjectTask> MisProjectTasks { get; set; }
    public DbSet<MisProjectUser> MisProjectUsers { get; set; }

    public DbSet<QuestionAndAnswer> QuestionAndAnswers { get; set; }


    public DbSet<Facility> Facilities { get; set; }

    public DbSet<EvaluationQuestion> EvaluationQuestions { get; set; }
    public DbSet<AccountModeratorEvaluation> AccountModeratorEvaluations { get; set; }



    public DbSet<PaymentCategory> PaymentCategories { get; set; }
    public DbSet<Payment> Payments { get; set; }

    // --- MIS: User Management Feature ---
    public DbSet<MisUserCategory> MisUserCategories { get; set; }
    public DbSet<MisKpiIndicator> MisKpiIndicators { get; set; }
    public DbSet<MisKpiUserScore> MisKpiUserScores { get; set; }
    public DbSet<MisUserFeatureRole> MisUserFeatureRoles { get; set; }
    public DbSet<MisUserNote> MisUserNotes { get; set; }
    public DbSet<MisActivityLog> MisActivityLogs { get; set; }

    // --- MIS: Committees & Groups Feature ---
    public DbSet<MisGroup> MisGroups { get; set; }
    public DbSet<MisGroupMember> MisGroupMembers { get; set; }

    // --- MIS: Unified Financials (Dues & Donations) Feature ---
    public DbSet<MisFinanceCategory> MisFinanceCategories { get; set; }
    public DbSet<MisFinanceTransaction> MisFinanceTransactions { get; set; }
    public DbSet<MisGroupPost> MisGroupPosts { get; set; }
    public DbSet<MisGroupFile> MisGroupFiles { get; set; }
    public DbSet<MisAlumniProfile> MisAlumniProfiles { get; set; }
    public DbSet<MisAlumniPost> MisAlumniPosts { get; set; }

    public DbSet<MisForm> MisForms { get; set; }
    public DbSet<MisFormResponse> MisFormResponses { get; set; }


    // --- MIS: Dynamic Interval Reporting ---
    public DbSet<MisReportSchema> MisReportSchemas { get; set; }
    public DbSet<MisReportParticipant> MisReportParticipants { get; set; }
    public DbSet<MisReportSubmission> MisReportSubmissions { get; set; }
    public DbSet<MisReportTimeLog> MisReportTimeLogs { get; set; }

    // Interactive User Manual Models
    public DbSet<MisManual> MisManuals { get; set; }
    public DbSet<MisManualPage> MisManualPages { get; set; }
    public DbSet<MisManualElement> MisManualElements { get; set; }

    // --- MIS: Management & Billing Feature ---
    public DbSet<MisFeature> MisFeatures { get; set; }
    public DbSet<MisFeatureInvoice> MisFeatureInvoices { get; set; }

    // --- MIS: Event & Ticketing Feature ---
    public DbSet<MisEvent> MisEvents { get; set; }
    public DbSet<MisTicketType> MisTicketTypes { get; set; }
    public DbSet<MisEventTicket> MisEventTickets { get; set; }

    // --- MIS: Proposals & Grants ---
    public DbSet<MisProposal> MisProposals { get; set; }
    public DbSet<MisProposalFile> MisProposalFiles { get; set; }
    public DbSet<MisProposalPhase> MisProposalPhases { get; set; }
    
    public DbSet<MisDonor> MisDonors { get; set; }
    public DbSet<MisGrant> MisGrants { get; set; }
    public DbSet<MisGrantMilestone> MisGrantMilestones { get; set; }

    // MIS Recruitment / ATS
    public DbSet<MisJobPosting> MisJobPostings { get; set; }
    public DbSet<MisJobApplicant> MisJobApplicants { get; set; }
    public DbSet<MisJobInterview> MisJobInterviews { get; set; }
    public DbSet<MisApplicantAccount> MisApplicantAccounts { get; set; }
    public DbSet<MisJobApplicantDocument> MisJobApplicantDocuments { get; set; }

    // --- MIS: Global Notifications ---
    public DbSet<MisNotification> MisNotifications { get; set; }

    // --- MIS: E-Voting ---
    public DbSet<MisElection> MisElections { get; set; }
    public DbSet<MisVoteCategory> MisVoteCategories { get; set; }
    public DbSet<MisCandidate> MisCandidates { get; set; }
    public DbSet<MisElectionVoter> MisElectionVoters { get; set; }
    public DbSet<MisUserVote> MisUserVotes { get; set; }

    // --- MIS: Fleet & Equipment Management ---
    public DbSet<MisEquipmentCategory> MisEquipmentCategories { get; set; }
    public DbSet<MisEquipment> MisEquipments { get; set; }
    public DbSet<MisMaintenanceLog> MisMaintenanceLogs { get; set; }

    // --- MIS: Email Logs ---
    public DbSet<MisNewsletterEmailLog> MisNewsletterEmailLogs { get; set; }

    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

    public DashboardDbContext(DbContextOptions options, ITenantProvider tenantProvider, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor = null) : base(options)
    {
        _tenantProvider = tenantProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditedEntities = new[] { typeof(UserCategory), typeof(MisUserFeatureRole), typeof(MisKpiUserScore), typeof(MisUserNote) };

        var entries = ChangeTracker.Entries()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                        && auditedEntities.Contains(e.Entity.GetType()))
            .ToList();

        if (entries.Any())
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = user?.Identity?.Name ?? "System";

            var logs = new List<MisActivityLog>();

            foreach (var entry in entries)
            {
                string actionType = entry.State.ToString();

                var log = new MisActivityLog
                {
                    ActionType = actionType,
                    Description = $"{actionType} a record.",
                    PerformedByProfileId = userId,
                    PerformedByFullName = userName,
                    Module = "User Management",
                    Date = DateTime.UtcNow
                };

                if (entry.Entity is UserCategory cat)
                {
                    log.ActionType = $"{actionType} Category";
                    log.Description = $"{actionType} Category: {cat.Title}";
                }
                else if (entry.Entity is MisUserFeatureRole role)
                {
                    log.ActionType = $"{actionType} Role";
                    log.Description = $"{actionType} Role '{role.ActionRole}' for module '{role.FeatureAreaKey}'.";
                    log.TargetProfileId = role.ProfileId;
                }
                else if (entry.Entity is MisUserNote note)
                {
                    log.ActionType = $"{actionType} Note";
                    log.Description = $"{actionType} HR Note (Private: {note.IsPrivate}).";
                    log.TargetProfileId = note.ProfileId;
                }
                else if (entry.Entity is MisKpiUserScore score)
                {
                    log.ActionType = $"{actionType} KPI";
                    log.Description = $"{actionType} KPI Score for Period: {score.Period}.";
                    log.TargetProfileId = score.ProfileId;
                }

                logs.Add(log);
            }

            if (logs.Any())
            {
                MisActivityLogs.AddRange(logs);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(
            _tenantProvider.ConnectionString,
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, /*_tenantProvider.DbSchemaName*/null));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        //configurationBuilder.Properties<string>()
        //    .HaveMaxLength(255);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<AccountModeratorEvaluation>(entity =>
        //{
        //    entity.HasKey(e => e.Id);

        //    entity.HasOne(e => e.Moderator)
        //        .WithMany()
        //        .HasForeignKey(e => e.ModeratorId);

        //    entity.HasOne(e => e.TimeTable)
        //        .WithMany()
        //        .HasForeignKey(e => e.TimeTableId);

        //    entity.HasOne(e => e.UserParticipant)
        //        .WithMany()
        //        .HasForeignKey(e => e.UserParticipantId); // Ensure this matches the property in your class

        //    entity.HasOne(e => e.EvaluationQuestion)
        //        .WithMany()
        //        .HasForeignKey(e => e.EvaluationQuestionId);
        //});



        //modelBuilder.HasDefaultSchema(_tenantProvider.DbSchemaName);   // set schema
        //modelBuilder.Entity<ParticipantList>()
        //   .HasOne(p => p.Accomodation)
        //   .WithOne(a => a.ParticipantList)
        //   .HasForeignKey<Accomodation>(a => a.ParticipantListId);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MisNewsletterRecipient>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed the User Manual
        modelBuilder.SeedMisManuals();
        modelBuilder.SeedMisWizards();

        // ConfigureCustomer(modelBuilder);
    }

    //private static void ConfigureCustomer(ModelBuilder builder)
    //{
    //    builder.Entity<Customer>(b =>
    //    {
    //        var table = b.ToTable("Customers");

    //        table.Property(p => p.CustomerId).ValueGeneratedOnAdd();
    //        table.HasKey(p => p.CustomerId).HasName("PK_CustomerId");
    //    });
    //}
}

}