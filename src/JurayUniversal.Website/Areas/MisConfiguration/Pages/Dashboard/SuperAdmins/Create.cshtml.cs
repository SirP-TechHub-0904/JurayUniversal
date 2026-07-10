using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.SuperAdmins
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public CreateModel(UserManager<Profile> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "User Email")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "No user found with the provided email address.";
                    return Page();
                }

                // Ensure the mSuperAdmin role exists
                if (!await _roleManager.RoleExistsAsync("mSuperAdmin"))
                {
                    await _roleManager.CreateAsync(new AppRole { Name = "mSuperAdmin", RoleCategory = JurayUniversal.Domain.Enum.Enum.RoleCategory.Both });
                }

                if (!await _userManager.IsInRoleAsync(user, "mSuperAdmin"))
                {
                    await _userManager.AddToRoleAsync(user, "mSuperAdmin");
                    TempData["SuccessMessage"] = $"{user.Email} has been promoted to Super Admin successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "This user is already a Super Admin.";
                }
            }

            return Page();
        }
    }
}
