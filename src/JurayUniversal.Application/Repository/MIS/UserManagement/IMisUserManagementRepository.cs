using JurayUniversal.Application.Dtos.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Application.Repository.MIS.UserManagement
{
    /// <summary>
    /// Feature-specific interface for the User Management MIS Area.
    /// Each method group corresponds to a specific page or partial view in the Area.
    /// Used via @inject in Partial Views and via constructor injection in PageModels.
    /// </summary>
    public interface IMisUserManagementRepository
    {
        // =========================================================
        // USER CATEGORIES (Settings)
        // =========================================================
        Task<List<MisUserCategory>> GetAllUserCategoriesAsync();
        Task CreateUserCategoryAsync(MisUserCategoryDto dto);
        Task UpdateUserCategoryAsync(MisUserCategoryDto dto);
        Task DeleteUserCategoryAsync(long id);

        // =========================================================
        // KPI INDICATORS (Settings — Admin defines what to measure)
        // =========================================================
        Task<List<MisKpiIndicator>> GetAllKpiIndicatorsAsync();
        Task<MisKpiIndicator?> GetKpiIndicatorByIdAsync(long id);
        Task CreateKpiIndicatorAsync(MisKpiIndicatorDto dto);
        Task UpdateKpiIndicatorAsync(MisKpiIndicatorDto dto);
        Task DeleteKpiIndicatorAsync(long id);

        // =========================================================
        // KPI SCORING (Admin scores a specific user)
        // =========================================================

        /// <summary>
        /// Returns a pre-filled list of score DTOs for a user in a given period,
        /// ready to be rendered as input fields on the KpiAssign page.
        /// If no score exists yet for an indicator, returns a blank DTO for it.
        /// </summary>
        Task<List<MisKpiScoreDto>> GetUserScoreSheetAsync(string profileId, string period);

        Task SaveKpiScoresAsync(List<MisKpiScoreDto> scores, string evaluatedBy);
        Task<List<MisKpiUserScore>> GetUserScoreHistoryAsync(string profileId);
        Task<List<string>> GetAllPeriodsAsync();  // Returns distinct periods e.g. ["Q1 2025", "Q2 2025"]

        /// <summary>
        /// Returns all users ranked by total KPI score for a given period.
        /// Used to populate the leaderboard table and charts.
        /// </summary>
        Task<List<MisKpiLeaderboardDto>> GetKpiLeaderboardAsync(string period);

        // =========================================================
        // FEATURE ROLE MANAGEMENT (Permission Matrix)
        // =========================================================
        Task<List<MisUserFeatureRole>> GetUserFeatureRolesAsync(string profileId);
        Task AssignFeatureRoleAsync(MisUserFeatureRoleDto dto);
        Task UpdateFeatureRoleAsync(MisUserFeatureRoleDto dto);
        Task RevokeFeatureRoleAsync(long roleId);

        /// <summary>
        /// Core security check — called in every Area's PageModel and Partial View.
        /// Returns true if the user has access to the given feature Area.
        /// </summary>
        Task<bool> UserHasFeatureAccessAsync(string profileId, string featureAreaKey);

        /// <summary>
        /// Returns the action role (GlobalEditor, FeatureEditor, Readonly) for a user in a feature.
        /// Returns null if the user has no access at all.
        /// Used to hide/show Edit and Delete buttons in views.
        /// </summary>
        Task<MisActionRole?> GetUserActionRoleAsync(string profileId, string featureAreaKey);

        // =========================================================
        // USER NOTES (Private HR notes on a user's profile)
        // =========================================================
        Task<List<MisUserNote>> GetUserNotesAsync(string profileId, bool includePrivate);
        Task AddUserNoteAsync(MisUserNoteDto dto);
        Task DeleteUserNoteAsync(long id);

        // =========================================================
        // DASHBOARD STATS (For AdminMIS Index page widgets)
        // =========================================================
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetSuspendedUsersCountAsync();
        Task<int> GetPendingUsersCountAsync();
    }
}
