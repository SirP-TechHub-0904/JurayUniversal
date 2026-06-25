using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MIS.Pages.Platform
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class IndexModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly IMisUserManagementRepository _userManagementRepo;

        public IndexModel(UserManager<Profile> userManager, IMisUserManagementRepository userManagementRepo)
        {
            _userManager = userManager;
            _userManagementRepo = userManagementRepo;
        }

        public Profile CurrentUser { get; set; } = null!;
        public List<MisUserFeatureRole> FeatureRoles { get; set; } = new();
        public List<MisKpiUserScore> KpiHistory { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            CurrentUser = user;
            FeatureRoles = await _userManagementRepo.GetUserFeatureRolesAsync(user.Id);
            KpiHistory = await _userManagementRepo.GetUserScoreHistoryAsync(user.Id);

            return Page();
        }
    }
}
