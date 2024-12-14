using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class UpdatePasswordModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
 
        public UpdatePasswordModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
         }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
           
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        [BindProperty]
        public Profile UserData { get; set; }
        public async Task<IActionResult> OnGetAsync(string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserData = user;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            var user = await _userManager.FindByIdAsync(UserData.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (changePasswordResult.Succeeded)
            {
               
                var h = await _userManager.AddPasswordAsync(user, Input.NewPassword);
                if(h.Succeeded)
                {
                    TempData["success"] = "Your password has been changed.";
                    return RedirectToPage("./Details", new { id = user.Id });
                }
                return Page();
            }

            return Page();
        }
    }
}
