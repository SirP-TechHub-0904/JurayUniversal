using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class UpdateEmailModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;

        public UpdateEmailModel(
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
            public string NewEmail { get; set; }
             
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

            var checkemail = await _userManager.FindByEmailAsync(Input.NewEmail);
            if(checkemail == null)
            {
                var emailtoken = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                var cheangeemail = await _userManager.ChangeEmailAsync(user, Input.NewEmail, emailtoken);
                if (cheangeemail.Succeeded)
                {
                    return RedirectToPage("./Details", new {id = user.Id});
                }

                TempData["error"] = "Unable to change Email";
                return Page();
            }
            else
            {
                TempData["error"] = "Email Already Taken";
                return Page();
            }

            
        }
    }
}
