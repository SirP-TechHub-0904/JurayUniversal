using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Management.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly DashboardDbContext _context; // add your DbContext

        public DeleteModel(
            SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager,
            DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public Profile UserDatas { get; set; }
        public UserRelatedDataSummary RelatedData { get; set; }

        [BindProperty]
        public string ConfirmPhrase { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) return NotFound();

            UserDatas = await _userManager.FindByIdAsync(id);
            if (UserDatas == null) return NotFound();

            RelatedData = await LoadRelatedDataSummary(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ConfirmPhrase?.Trim().ToLower() != "walata")
            {
                ModelState.AddModelError("ConfirmPhrase", "Incorrect confirmation phrase. Please type 'walata' to proceed.");
                UserDatas = await _userManager.FindByIdAsync(UserId);
                RelatedData = await LoadRelatedDataSummary(UserId);
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            // ── Delete all related records ──────────────────────────────
            var profileId = user.Id;

            // Attendance
            _context.Attendances.RemoveRange(
                await _context.Attendances.Where(x => x.User.Id == profileId).ToListAsync());

            // BudgetMainCategory
            _context.BudgetMainCategories.RemoveRange(
                await _context.BudgetMainCategories.Where(x => x.User.Id == profileId).ToListAsync());

            // ChatMessage (sender & receiver)
            _context.ChatMessage.RemoveRange(
                await _context.ChatMessage.Where(x => x.User.Id == profileId || x.Receiver.Id == profileId).ToListAsync());

            // CompanyExpenses
            _context.CompayExpenses.RemoveRange(
                await _context.CompayExpenses.Where(x => x.User.Id == profileId).ToListAsync());

            // FundSource
            _context.FundSources.RemoveRange(
                await _context.FundSources.Where(x => x.User.Id == profileId).ToListAsync());

            // MeetingAttendance
            _context.MeetingAttendances.RemoveRange(
                await _context.MeetingAttendances.Where(x => x.User.Id == profileId).ToListAsync());

            // NIPSS
            _context.Moderators.RemoveRange(
                await _context.Moderators.Where(x => x.User.Id == profileId).ToListAsync());
            _context.ParticipantLists.RemoveRange(
                await _context.ParticipantLists.Where(x => x.User.Id == profileId).ToListAsync());
            _context.QuestionAndAnswers.RemoveRange(
                await _context.QuestionAndAnswers.Where(x => x.User.Id == profileId).ToListAsync());
            _context.StaffManagers.RemoveRange(
                await _context.StaffManagers.Where(x => x.User.Id == profileId).ToListAsync());
            _context.StudyGroups.RemoveRange(
                await _context.StudyGroups.Where(x => x.User.Id == profileId).ToListAsync());
            _context.UserNutritionEvaluations.RemoveRange(
                await _context.UserNutritionEvaluations.Where(x => x.User.Id == profileId).ToListAsync());
            _context.UserVotes.RemoveRange(
                await _context.UserVotes.Where(x => x.VoterUser.Id == profileId).ToListAsync());
            _context.VoteCondidates.RemoveRange(
                await _context.VoteCondidates.Where(x => x.Candidate.Id == profileId).ToListAsync());

            // Office & Portfolio
            _context.OfficeActivityDialies.RemoveRange(
                await _context.OfficeActivityDialies.Where(x => x.LastUpdateBy.Id == profileId).ToListAsync());
            _context.PortfolioContactUses.RemoveRange(
                await _context.PortfolioContactUses.Where(x => x.Profile.Id == profileId).ToListAsync());
            _context.ProfileCategories.RemoveRange(
                await _context.ProfileCategories.Where(x => x.Profile.Id == profileId).ToListAsync());
            _context.ProfileCategoryLists.RemoveRange(
                await _context.ProfileCategoryLists.Where(x => x.Profile.Id == profileId).ToListAsync());
            _context.ProfilePortfolios.RemoveRange(
                await _context.ProfilePortfolios.Where(x => x.Profile.Id == profileId).ToListAsync());
            _context.ProfilePortfolioSetting.RemoveRange(
                await _context.ProfilePortfolioSetting.Where(x => x.Profile.Id == profileId).ToListAsync());

            // Program, Project, Proposal, Report, Salary
            _context.ProgramUsers.RemoveRange(
                await _context.ProgramUsers.Where(x => x.User.Id == profileId).ToListAsync());
            _context.ProjectMains.RemoveRange(
                await _context.ProjectMains.Where(x => x.ProfileReferral.Id == profileId).ToListAsync());
            _context.ProjectTasks.RemoveRange(
                await _context.ProjectTasks.Where(x => x.Profile.Id == profileId).ToListAsync());
            _context.ProjectUsers.RemoveRange(
                await _context.ProjectUsers.Where(x => x.Profile.Id == profileId).ToListAsync());
            _context.Proposals.RemoveRange(
                await _context.Proposals.Where(x => x.SubmittedBy.Id == profileId).ToListAsync());
            _context.Report.RemoveRange(
                await _context.Report.Where(x => x.User.Id == profileId).ToListAsync());
            _context.Salarys.RemoveRange(
                await _context.Salarys.Where(x => x.User.Id == profileId).ToListAsync());

            // Training
            _context.Trainings.RemoveRange(
                await _context.Trainings.Where(x => x.Moderator.Id == profileId || x.Trainner.Id == profileId).ToListAsync());
            _context.TrainingAttendances.RemoveRange(
                await _context.TrainingAttendances.Where(x => x.User.Id == profileId).ToListAsync());

            // XProject
            _context.XProjectMessages.RemoveRange(
                await _context.XProjectMessages.Where(x => x.Sender.Id == profileId || x.Receiver.Id == profileId).ToListAsync());
            _context.XProjectTasks.RemoveRange(
                await _context.XProjectTasks.Where(x => x.User.Id == profileId || x.TestedBy.Id == profileId || x.ApprovedBy.Id == profileId).ToListAsync());

            // AdditionalProfile
            _context.AdditionalProfiles.RemoveRange(
                await _context.AdditionalProfiles.Where(x => x.Profile.Id == profileId).ToListAsync());

            await _context.SaveChangesAsync();

            // ── Finally delete the Identity user ───────────────────────
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                UserDatas = user;
                RelatedData = await LoadRelatedDataSummary(UserId);
                return Page();
            }

            _logger.LogInformation("User {UserId} deleted by admin.", profileId);
            return RedirectToPage("./Index");
        }

        private async Task<UserRelatedDataSummary> LoadRelatedDataSummary(string profileId)
        {
            return new UserRelatedDataSummary
            {
                AttendanceCount = await _context.Attendances.CountAsync(x => x.User.Id == profileId),
                BudgetCount = await _context.BudgetMainCategories.CountAsync(x => x.User.Id == profileId),
                ChatMessageCount = await _context.ChatMessage.CountAsync(x => x.User.Id == profileId || x.Receiver.Id == profileId),
                CompanyExpensesCount = await _context.CompayExpenses.CountAsync(x => x.User.Id == profileId),
                FundSourceCount = await _context.FundSources.CountAsync(x => x.User.Id == profileId),
                MeetingAttendanceCount = await _context.MeetingAttendances.CountAsync(x => x.User.Id == profileId),
                NipssCount = await _context.Moderators.CountAsync(x => x.User.Id == profileId)
                           + await _context.ParticipantLists.CountAsync(x => x.User.Id == profileId)
                           + await _context.QuestionAndAnswers.CountAsync(x => x.User.Id == profileId)
                           + await _context.StaffManagers.CountAsync(x => x.User.Id == profileId)
                           + await _context.StudyGroups.CountAsync(x => x.User.Id == profileId)
                           + await _context.UserNutritionEvaluations.CountAsync(x => x.User.Id == profileId),
                VotingCount = await _context.UserVotes.CountAsync(x => x.VoterUser.Id == profileId)
                            + await _context.VoteCondidates.CountAsync(x => x.Candidate.Id == profileId),
                OfficeCount = await _context.OfficeActivityDialies.CountAsync(x => x.LastUpdateBy.Id == profileId),
                PortfolioCount = await _context.ProfilePortfolios.CountAsync(x => x.Profile.Id == profileId)
                               + await _context.ProfilePortfolioSetting.CountAsync(x => x.Profile.Id == profileId)
                               + await _context.PortfolioContactUses.CountAsync(x => x.Profile.Id == profileId),
                ProjectCount = await _context.ProjectMains.CountAsync(x => x.ProfileReferral.Id == profileId)
                             + await _context.ProjectTasks.CountAsync(x => x.Profile.Id == profileId)
                             + await _context.ProjectUsers.CountAsync(x => x.Profile.Id == profileId),
                XProjectCount = await _context.XProjectMessages.CountAsync(x => x.Sender.Id == profileId || x.Receiver.Id == profileId)
                              + await _context.XProjectTasks.CountAsync(x => x.User.Id == profileId || x.TestedBy.Id == profileId || x.ApprovedBy.Id == profileId),
                TrainingCount = await _context.Trainings.CountAsync(x => x.Moderator.Id == profileId || x.Trainner.Id == profileId)
                              + await _context.TrainingAttendances.CountAsync(x => x.User.Id == profileId),
                SalaryCount = await _context.Salarys.CountAsync(x => x.User.Id == profileId),
                ReportCount = await _context.Report.CountAsync(x => x.User.Id == profileId),
                ProposalCount = await _context.Proposals.CountAsync(x => x.SubmittedBy.Id == profileId),
                AdditionalProfileCount = await _context.AdditionalProfiles.CountAsync(x => x.Profile.Id == profileId),
            };
        }
    }

    public class UserRelatedDataSummary
    {
        public int AttendanceCount { get; set; }
        public int BudgetCount { get; set; }
        public int ChatMessageCount { get; set; }
        public int CompanyExpensesCount { get; set; }
        public int FundSourceCount { get; set; }
        public int MeetingAttendanceCount { get; set; }
        public int NipssCount { get; set; }
        public int VotingCount { get; set; }
        public int OfficeCount { get; set; }
        public int PortfolioCount { get; set; }
        public int ProjectCount { get; set; }
        public int XProjectCount { get; set; }
        public int TrainingCount { get; set; }
        public int SalaryCount { get; set; }
        public int ReportCount { get; set; }
        public int ProposalCount { get; set; }
        public int AdditionalProfileCount { get; set; }

        public int Total => AttendanceCount + BudgetCount + ChatMessageCount + CompanyExpensesCount +
                            FundSourceCount + MeetingAttendanceCount + NipssCount + VotingCount +
                            OfficeCount + PortfolioCount + ProjectCount + XProjectCount +
                            TrainingCount + SalaryCount + ReportCount + ProposalCount + AdditionalProfileCount;
    }
}
