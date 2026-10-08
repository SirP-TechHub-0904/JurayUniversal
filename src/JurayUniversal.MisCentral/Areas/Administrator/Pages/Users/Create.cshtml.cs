using System.ComponentModel.DataAnnotations;
using JurayUniversal.MisCentral.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Users
{
    [Authorize(Roles = "MasterSuperAdmin,CentralAdmin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<string> AvailableRoles { get; set; } = new();

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            public List<string> SelectedRoles { get; set; } = new();
        }

        public async Task OnGetAsync()
        {
            AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();

            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.SelectedRoles.Any())
                    {
                        await _userManager.AddToRolesAsync(user, Input.SelectedRoles);
                    }
                    else
                    {
                        // Default to CentralAdmin
                        await _userManager.AddToRoleAsync(user, CentralRoles.CentralAdmin);
                    }

                    return RedirectToPage("/Users/Index", new { area = "Administrator" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
