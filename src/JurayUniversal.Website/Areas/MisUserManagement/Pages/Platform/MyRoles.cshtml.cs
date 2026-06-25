using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Platform
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class MyRolesModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly IMisUserManagementRepository _userManagementRepo;

        public MyRolesModel(UserManager<Profile> userManager, IMisUserManagementRepository userManagementRepo)
        {
            _userManager = userManager;
            _userManagementRepo = userManagementRepo;
        }

        public Profile UserProfile { get; set; } = null!;
        public List<MisUserFeatureRole> FeatureRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            UserProfile = user;
            FeatureRoles = await _userManagementRepo.GetUserFeatureRolesAsync(user.Id);

            return Page();
        }
    }
}
