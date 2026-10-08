using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Platform
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly System.Text.Encodings.Web.UrlEncoder _urlEncoder;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ChangePasswordModel(
            UserManager<Profile> userManager, 
            SignInManager<Profile> signInManager, 
            System.Text.Encodings.Web.UrlEncoder urlEncoder,
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _urlEncoder = urlEncoder;
            _context = context;
        }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [BindProperty]
        public string VerificationCode { get; set; } = string.Empty;

        public bool Is2faEnabled { get; set; }
        public string SharedKey { get; set; } = string.Empty;
        public string AuthenticatorUri { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            if (!Is2faEnabled)
            {
                var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                }
                SharedKey = FormatKey(unformattedKey!);
                var email = await _userManager.GetEmailAsync(user);

                var superSetting = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.SuperSettings);
                string businessName = !string.IsNullOrWhiteSpace(superSetting?.CompanyName) 
                    ? superSetting.CompanyName 
                    : (!string.IsNullOrWhiteSpace(superSetting?.CompanyNameAbbreviation) 
                        ? superSetting.CompanyNameAbbreviation 
                        : (!string.IsNullOrWhiteSpace(superSetting?.DashboardTitle) ? superSetting.DashboardTitle : "Assembly"));

                AuthenticatorUri = string.Format(
                    "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
                    _urlEncoder.Encode(businessName.Trim()),
                    _urlEncoder.Encode(email!),
                    unformattedKey);
            }

            return Page();
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new System.Text.StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition));
            }
            return result.ToString().ToLowerInvariant();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["success"] = "Your password has been changed successfully!";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEnable2FAAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var verificationCode = VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            if (string.IsNullOrEmpty(verificationCode))
            {
                TempData["error"] = "Please enter the verification code.";
                return RedirectToPage();
            }

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                TempData["error"] = "Verification code is invalid.";
                return RedirectToPage();
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            TempData["success"] = "Your authenticator app has been verified and 2FA is now enabled.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDisable2FAAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (disable2faResult.Succeeded)
            {
                 await _userManager.ResetAuthenticatorKeyAsync(user);
                 TempData["success"] = "2FA has been disabled. You can re-enable it anytime.";
            }
            return RedirectToPage();
        }
    }
}
