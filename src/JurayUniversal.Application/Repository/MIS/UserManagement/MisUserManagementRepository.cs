using JurayUniversal.Application.Dtos.MIS.UserManagement;
using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Application.Repository.MIS.UserManagement
{
    public class MisUserManagementRepository : IMisUserManagementRepository
    {
        private readonly DashboardDbContext _context;

        public MisUserManagementRepository(DashboardDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // USER CATEGORIES
        // =========================================================

        public async Task<List<MisUserCategory>> GetAllUserCategoriesAsync()
            => await _context.MisUserCategories.OrderBy(x => x.SortOrder).ToListAsync();

        public async Task CreateUserCategoryAsync(MisUserCategoryDto dto)
        {
            var entity = new MisUserCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive,
                Date = DateTime.UtcNow
            };
            _context.MisUserCategories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserCategoryAsync(MisUserCategoryDto dto)
        {
            var entity = await _context.MisUserCategories.FindAsync(dto.Id);
            if (entity == null) return;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.SortOrder = dto.SortOrder;
            entity.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserCategoryAsync(long id)
        {
            var entity = await _context.MisUserCategories.FindAsync(id);
            if (entity != null)
            {
                _context.MisUserCategories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // =========================================================
        // KPI INDICATORS
        // =========================================================

        public async Task<List<MisKpiIndicator>> GetAllKpiIndicatorsAsync()
            => await _context.MisKpiIndicators
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

        public async Task<MisKpiIndicator?> GetKpiIndicatorByIdAsync(long id)
            => await _context.MisKpiIndicators.FindAsync(id);

        public async Task CreateKpiIndicatorAsync(MisKpiIndicatorDto dto)
        {
            var entity = new MisKpiIndicator
            {
                Title = dto.Title,
                Description = dto.Description,
                MaxScore = dto.MaxScore,
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive,
                Date = DateTime.UtcNow
            };
            _context.MisKpiIndicators.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKpiIndicatorAsync(MisKpiIndicatorDto dto)
        {
            var entity = await _context.MisKpiIndicators.FindAsync(dto.Id);
            if (entity == null) return;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.MaxScore = dto.MaxScore;
            entity.SortOrder = dto.SortOrder;
            entity.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteKpiIndicatorAsync(long id)
        {
            var entity = await _context.MisKpiIndicators.FindAsync(id);
            if (entity != null)
            {
                _context.MisKpiIndicators.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // =========================================================
        // KPI SCORING
        // =========================================================

        public async Task<List<MisKpiScoreDto>> GetUserScoreSheetAsync(string profileId, string period)
        {
            var indicators = await GetAllKpiIndicatorsAsync();
            var existingScores = await _context.MisKpiUserScores
                .Where(x => x.ProfileId == profileId && x.Period == period)
                .ToListAsync();

            // Build a pre-filled score sheet: one row per indicator
            return indicators.Select(indicator =>
            {
                var existing = existingScores.FirstOrDefault(s => s.MisKpiIndicatorId == indicator.Id);
                return new MisKpiScoreDto
                {
                    Id = existing?.Id ?? 0,
                    ProfileId = profileId,
                    MisKpiIndicatorId = indicator.Id,
                    IndicatorTitle = indicator.Title,
                    MaxScore = indicator.MaxScore,
                    Score = existing?.Score ?? 0,
                    Period = period,
                    Remarks = existing?.Remarks
                };
            }).ToList();
        }

        public async Task SaveKpiScoresAsync(List<MisKpiScoreDto> scores, string evaluatedBy)
        {
            foreach (var dto in scores)
            {
                if (dto.Id > 0)
                {
                    // Update existing score
                    var existing = await _context.MisKpiUserScores.FindAsync(dto.Id);
                    if (existing != null)
                    {
                        existing.Score = dto.Score;
                        existing.Remarks = dto.Remarks;
                        existing.EvaluatedBy = evaluatedBy;
                        existing.Date = DateTime.UtcNow;
                    }
                }
                else
                {
                    // Create new score
                    _context.MisKpiUserScores.Add(new MisKpiUserScore
                    {
                        ProfileId = dto.ProfileId,
                        MisKpiIndicatorId = dto.MisKpiIndicatorId,
                        Score = dto.Score,
                        Period = dto.Period,
                        Remarks = dto.Remarks,
                        EvaluatedBy = evaluatedBy,
                        Date = DateTime.UtcNow
                    });
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<MisKpiUserScore>> GetUserScoreHistoryAsync(string profileId)
            => await _context.MisKpiUserScores
                .Include(x => x.MisKpiIndicator)
                .Where(x => x.ProfileId == profileId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

        public async Task<List<string>> GetAllPeriodsAsync()
            => await _context.MisKpiUserScores
                .Select(x => x.Period)
                .Distinct()
                .OrderByDescending(x => x)
                .ToListAsync();

        public async Task<List<MisKpiLeaderboardDto>> GetKpiLeaderboardAsync(string period)
        {
            var scores = await _context.MisKpiUserScores
                .Include(x => x.Profile)
                .Include(x => x.MisKpiIndicator)
                .Where(x => x.Period == period)
                .ToListAsync();

            var indicators = await GetAllKpiIndicatorsAsync();
            decimal maxPossible = indicators.Sum(i => i.MaxScore);

            var grouped = scores
                .GroupBy(x => x.ProfileId)
                .Select(g =>
                {
                    var profile = g.First().Profile;
                    var totalScore = g.Sum(s => s.Score);
                    return new MisKpiLeaderboardDto
                    {
                        ProfileId = g.Key,
                        FullName = profile?.GetFullName() ?? "Unknown",
                        PassportUrl = profile?.PassportFilePathUrl,
                        TotalScore = totalScore,
                        MaxPossibleScore = maxPossible,
                        Percentage = maxPossible > 0 ? Math.Round((totalScore / maxPossible) * 100, 2) : 0
                    };
                })
                .OrderByDescending(x => x.TotalScore)
                .ToList();

            // Assign rank
            for (int i = 0; i < grouped.Count; i++)
                grouped[i].Rank = i + 1;

            return grouped;
        }

        // =========================================================
        // FEATURE ROLE MANAGEMENT
        // =========================================================

        public async Task<List<MisUserFeatureRole>> GetUserFeatureRolesAsync(string profileId)
            => await _context.MisUserFeatureRoles
                .Where(x => x.ProfileId == profileId && x.IsActive)
                .OrderBy(x => x.FeatureAreaKey)
                .ToListAsync();

        public async Task AssignFeatureRoleAsync(MisUserFeatureRoleDto dto)
        {
            // Prevent duplicate: check if this user already has a role for this feature
            var existing = await _context.MisUserFeatureRoles
                .FirstOrDefaultAsync(x => x.ProfileId == dto.ProfileId && x.FeatureAreaKey == dto.FeatureAreaKey);

            if (existing != null)
            {
                // Update instead of creating a duplicate
                existing.ActionRole = (MisActionRole)dto.ActionRole;
                existing.IsActive = dto.IsActive;
                existing.AssignedBy = dto.AssignedBy;
                existing.Date = DateTime.UtcNow;
            }
            else
            {
                _context.MisUserFeatureRoles.Add(new MisUserFeatureRole
                {
                    ProfileId = dto.ProfileId,
                    FeatureAreaKey = dto.FeatureAreaKey,
                    ActionRole = (MisActionRole)dto.ActionRole,
                    IsActive = dto.IsActive,
                    AssignedBy = dto.AssignedBy,
                    Date = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeatureRoleAsync(MisUserFeatureRoleDto dto)
        {
            var entity = await _context.MisUserFeatureRoles.FindAsync(dto.Id);
            if (entity == null) return;
            entity.ActionRole = (MisActionRole)dto.ActionRole;
            entity.IsActive = dto.IsActive;
            entity.AssignedBy = dto.AssignedBy;
            entity.Date = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task RevokeFeatureRoleAsync(long roleId)
        {
            var entity = await _context.MisUserFeatureRoles.FindAsync(roleId);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserHasFeatureAccessAsync(string profileId, string featureAreaKey)
            => await _context.MisUserFeatureRoles
                .AnyAsync(x => x.ProfileId == profileId
                            && x.FeatureAreaKey == featureAreaKey
                            && x.IsActive);

        public async Task<MisActionRole?> GetUserActionRoleAsync(string profileId, string featureAreaKey)
        {
            var role = await _context.MisUserFeatureRoles
                .FirstOrDefaultAsync(x => x.ProfileId == profileId
                                       && x.FeatureAreaKey == featureAreaKey
                                       && x.IsActive);
            return role?.ActionRole;
        }

        // =========================================================
        // USER NOTES
        // =========================================================

        public async Task<List<MisUserNote>> GetUserNotesAsync(string profileId, bool includePrivate)
        {
            var query = _context.MisUserNotes.Where(x => x.ProfileId == profileId);
            if (!includePrivate)
                query = query.Where(x => x.IsPrivate == false);
            return await query.OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task AddUserNoteAsync(MisUserNoteDto dto)
        {
            _context.MisUserNotes.Add(new MisUserNote
            {
                ProfileId = dto.ProfileId,
                Note = dto.Note,
                IsPrivate = dto.IsPrivate,
                CreatedBy = dto.CreatedBy,
                Date = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserNoteAsync(long id)
        {
            var entity = await _context.MisUserNotes.FindAsync(id);
            if (entity != null)
            {
                _context.MisUserNotes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // =========================================================
        // DASHBOARD STATS
        // =========================================================

        public async Task<int> GetTotalUsersCountAsync()
            => await _context.Users.CountAsync();

        public async Task<int> GetActiveUsersCountAsync()
            => await _context.Users
                .OfType<Profile>()
                .CountAsync(x => x.UserStatus == UserStatus.Active);

        public async Task<int> GetSuspendedUsersCountAsync()
            => await _context.Users
                .OfType<Profile>()
                .CountAsync(x => x.UserStatus == UserStatus.Suspended);

        public async Task<int> GetPendingUsersCountAsync()
            => await _context.Users
                .OfType<Profile>()
                .CountAsync(x => x.UserStatus == UserStatus.Pending);
    }
}
