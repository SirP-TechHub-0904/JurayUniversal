using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.SuperAdmins
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;

        public IndexModel(UserManager<Profile> userManager)
        {
            _userManager = userManager;
        }

        public IList<Profile> SuperAdmins { get; set; } = new List<Profile>();

        public async Task OnGetAsync()
        {
            // Fetch users with the mSuperAdmin role
            var mSuperAdmins = await _userManager.GetUsersInRoleAsync("mSuperAdmin");
            foreach (var user in mSuperAdmins)
            {
                if (!SuperAdmins.Contains(user))
                {
                    SuperAdmins.Add(user);
                }
            }
            
            // Optionally fetch "SuperAdmin" if used
            var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            foreach (var user in superAdmins)
            {
                if (!SuperAdmins.Contains(user))
                {
                    SuperAdmins.Add(user);
                }
            }
        }

        public async Task<IActionResult> OnPostRemoveAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Prevent revoking oneself
                if (user.Email == User.Identity.Name)
                {
                    TempData["ErrorMessage"] = "You cannot revoke your own Super Admin access.";
                    return RedirectToPage();
                }

                if (await _userManager.IsInRoleAsync(user, "mSuperAdmin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "mSuperAdmin");
                }
                if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "SuperAdmin");
                }
                
                TempData["SuccessMessage"] = "Super Admin access revoked successfully.";
            }

            return RedirectToPage();
        }
    }
}
