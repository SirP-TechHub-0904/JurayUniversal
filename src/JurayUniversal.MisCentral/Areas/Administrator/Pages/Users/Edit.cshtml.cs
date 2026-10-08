using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Users
{
    [Authorize(Roles = "MasterSuperAdmin,CentralAdmin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<string> AvailableRoles { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public class InputModel
        {
            public string Id { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            public string UserName { get; set; } = string.Empty;

            public bool IsLockedOut { get; set; }

            public List<string> SelectedRoles { get; set; } = new();

            [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Reset Password (leave blank to keep current)")]
            public string? NewPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var isLocked = await _userManager.IsLockedOutAsync(user);

            Input = new InputModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                IsLockedOut = isLocked,
                SelectedRoles = userRoles.ToList()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = Input.Email;
            user.UserName = Input.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // Update Roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = Input.SelectedRoles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(Input.SelectedRoles).ToList();

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }
            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            // Update Lockout status
            if (Input.IsLockedOut)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }

            // Reset password if provided
            if (!string.IsNullOrWhiteSpace(Input.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, Input.NewPassword);
                if (!passResult.Succeeded)
                {
                    foreach (var error in passResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            StatusMessage = "Central Administrator updated successfully.";
            return RedirectToPage("/Users/Index", new { area = "Administrator" });
        }
    }
}
