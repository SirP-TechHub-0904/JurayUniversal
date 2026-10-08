using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Services
{
    public class MisAuthorizationService : IMisAuthorizationService
    {
        private readonly UserManager<Profile> _userManager;
        private readonly DashboardDbContext _context;

        public MisAuthorizationService(UserManager<Profile> userManager, DashboardDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> CanEditAsync(ClaimsPrincipal user, string featureAreaKey)
        {
            if (user == null) return false;

            // System-level admins bypass the checks and have full access
            if (user.IsInRole("mSuperAdmin") || user.IsInRole("Admin") || user.IsInRole("Administrator"))
            {
                return true;
            }

            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null) return false;

            var roles = await _context.MisUserFeatureRoles
                .Where(r => r.ProfileId == appUser.Id && r.IsActive)
                .ToListAsync();

            // If Admin role is assigned with Readonly, they are strictly ReadOnly across features
            var adminRole = roles.FirstOrDefault(r => string.Equals(r.FeatureAreaKey, "Admin", System.StringComparison.OrdinalIgnoreCase));
            if (adminRole != null && adminRole.ActionRole == MisActionRole.Readonly)
            {
                return false;
            }

            // If they have a GlobalEditor role for ANY feature, they are a GlobalEditor
            if (roles.Any(r => r.ActionRole == MisActionRole.GlobalEditor))
            {
                return true;
            }

            // Otherwise, check if they are a FeatureEditor for THIS specific feature
            var featureRole = roles.FirstOrDefault(r => string.Equals(r.FeatureAreaKey, featureAreaKey, System.StringComparison.OrdinalIgnoreCase));
            if (featureRole != null && featureRole.ActionRole == MisActionRole.FeatureEditor)
            {
                return true;
            }

            // They are either Readonly or have no assigned role for this feature
            return false;
        }

        public async Task<bool> HasAccessAsync(ClaimsPrincipal user, string featureAreaKey)
        {
            if (user == null) return false;

            // System-level admins bypass the checks and have full access
            if (user.IsInRole("mSuperAdmin") || user.IsInRole("Admin") || user.IsInRole("Administrator"))
            {
                return true;
            }

            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null) return false;

            var roles = await _context.MisUserFeatureRoles
                .Where(r => r.ProfileId == appUser.Id && r.IsActive)
                .ToListAsync();

            // If they have a GlobalEditor role for ANY feature or Admin role, they have access to everything
            if (roles.Any(r => r.ActionRole == MisActionRole.GlobalEditor || string.Equals(r.FeatureAreaKey, "Admin", System.StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            // Otherwise, check if they have ANY role (FeatureEditor or Readonly) for this specific feature
            return roles.Any(r => string.Equals(r.FeatureAreaKey, featureAreaKey, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
