using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JurayUniversal.Website.V2.Pages.Authv2.Account
{
    [AllowAnonymous]
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public LoginWithRecoveryCodeModel(SignInManager<Profile> signInManager, ILogger<LoginWithRecoveryCodeModel> logger, Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        [BindProperty]
        public SuperSetting SuperSetting { get; set; }
        [BindProperty]
        public Setting Setting { get; set; }
        public class InputModel
        {
            [BindProperty]
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
        }

            public string TemplateLogin { get; set; }
               public async Task<IActionResult> OnGetAsync(string returnUrl = null)
         {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Setting = await _context.Settings.FirstOrDefaultAsync();

            if (SuperSetting == null)
            {
                return RedirectToPage("/AuthVadmin/Readonly", new { area = "V2" });
            }

            if (SuperSetting.ActivateLogin == false && SuperSetting.ActivateLoginInMenu == false)
            {
                return NotFound();
            }
            if (SuperSetting == null)
            {
                return RedirectToPage("/AuthVadmin/Readonly", new { area = "V2" });
            }
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                var superrole = await _userManager.IsInRoleAsync(user, "mSuperAdmin");
                var SubAdmin = await _userManager.IsInRoleAsync(user, "SubAdmin");
                var adminrole = await _userManager.IsInRoleAsync(user, "Admin");
                var editorrole = await _userManager.IsInRoleAsync(user, "Editor");
                var managerrole = await _userManager.IsInRoleAsync(user, "Manager");
                var administrator = await _userManager.IsInRoleAsync(user, "Administrator");
                var useracc = await _userManager.IsInRoleAsync(user, "User");
                var Participant = await _userManager.IsInRoleAsync(user, "Participant");

                if (SuperSetting.UserNipssArea == true)
                {
                    if (user.UpdateProfile == false)
                    {
                        return RedirectToPage("/Account/UpdateProfile", new { area = "NIPSS" });

                    }
                    if (adminrole.Equals(true) || administrator.Equals(true) || superrole.Equals(true) || editorrole.Equals(true))
                    {
                        return RedirectToPage("/Dashboard/Index", new { area = "NIPSSADMIN" });
                    }
                    else if (Participant.Equals(true))
                    {
                        if (user.ResetPassword == true)
                        {
                            return RedirectToPage("/AuthV2/Auth/ChangePassword", new { area = "V2" });
                        }
                        return RedirectToPage("/Dashboard/Index", new { area = "NIPSS" });
                    }

                }
                else
                {


                    if (adminrole.Equals(true) || administrator.Equals(true) || superrole.Equals(true) || editorrole.Equals(true))
                    {
                        return RedirectToPage("/Dashboard/Index", new { area = "Admin" });
                    }
                    else if (useracc.Equals(true))
                    {
                        return RedirectToPage("/Dashboard/Index", new { area = "Staff" });
                    }

                }
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return Page();
            }
        }
    }
}
