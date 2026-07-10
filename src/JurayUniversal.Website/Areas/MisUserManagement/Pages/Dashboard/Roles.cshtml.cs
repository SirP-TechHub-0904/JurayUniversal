using JurayUniversal.Application.Dtos.MIS.UserManagement;
using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class RolesModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IMisUserManagementRepository _userManagementRepo;
        private readonly UserManager<Profile> _userManager;

        public RolesModel(DashboardDbContext context, IMisUserManagementRepository userManagementRepo, UserManager<Profile> userManager)
        {
            _context = context;
            _userManagementRepo = userManagementRepo;
            _userManager = userManager;
        }

        public Profile UserProfile { get; set; } = null!;
        public List<MisUserFeatureRole> AssignedRoles { get; set; } = new();
        public List<string> SystemFeatures { get; set; } = new();

        [BindProperty]
        public string SelectedFeatureKey { get; set; } = string.Empty;

        [BindProperty]
        public int SelectedActionRole { get; set; } = 3; // ReadOnly default

        public bool IsReadOnly { get; set; }

        private async Task<bool> CheckAccessAndSetReadOnlyAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return false;

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor") || roles.Contains("UserManagement_Readonly");
            
            if (!hasAccess) return false;

            IsReadOnly = !roles.Contains("mSuperAdmin") && !roles.Contains("Admin") && !roles.Contains("UserManagement_GlobalEditor") && !roles.Contains("UserManagement_FeatureEditor");
            return true;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!await CheckAccessAndSetReadOnlyAsync())
            {
                return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UserProfile = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.Id == id);
            if (UserProfile == null)
            {
                return NotFound();
            }

            AssignedRoles = await _userManagementRepo.GetUserFeatureRolesAsync(id);

            // Populate system features list (18 active modules)
            SystemFeatures = new List<string>
            {
                "MisAlumni", "MisConfiguration", "MisFinances", "MisFleet", 
                "MisForms", "MisGrants", "MisGroups", "MisManagement", 
                "MisManual", "MisMassComm", "MisNewsletter", "MisProjects", 
                "MisProposals", "MisRecruitment", "MisReports", "MisTicketing", 
                "MisUserManagement", "MisVoting"
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAssignAsync(string id)
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            if (IsReadOnly)
            {
                TempData["error"] = "You do not have permission to assign roles.";
                return RedirectToPage(new { id });
            }

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(SelectedFeatureKey))
            {
                return RedirectToPage(new { id });
            }

            var currentAdmin = await _userManager.GetUserAsync(User);
            var adminName = currentAdmin?.GetFullName();
            if (string.IsNullOrWhiteSpace(adminName)) adminName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(adminName)) adminName = "Admin";

            var dto = new MisUserFeatureRoleDto
            {
                ProfileId = id,
                FeatureAreaKey = SelectedFeatureKey,
                ActionRole = SelectedActionRole,
                IsActive = true,
                AssignedBy = adminName
            };

            await _userManagementRepo.AssignFeatureRoleAsync(dto);

            TempData["success"] = "Feature permission assigned successfully!";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRevokeAsync(string id, long roleId)
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            if (IsReadOnly)
            {
                TempData["error"] = "You do not have permission to revoke roles.";
                return RedirectToPage(new { id });
            }

            if (roleId > 0)
            {
                await _userManagementRepo.RevokeFeatureRoleAsync(roleId);
                TempData["success"] = "Feature permission revoked successfully!";
            }
            return RedirectToPage(new { id });
        }
    }
}
