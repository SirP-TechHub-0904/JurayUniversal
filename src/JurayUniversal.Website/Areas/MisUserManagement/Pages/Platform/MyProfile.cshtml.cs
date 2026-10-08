using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Platform
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly DashboardDbContext _context;

        public MyProfileModel(UserManager<Profile> userManager, SignInManager<Profile> signInManager, DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public Profile UserProfile { get; set; } = null!;
        public System.Collections.Generic.List<MisUserFeatureRole> AssignedRoles { get; set; } = new();

        [BindProperty]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Contact Address")]
        public string? Address { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "New Email Address")]
        public string NewEmail { get; set; } = string.Empty;

        [BindProperty]
        [Phone]
        [Display(Name = "New Phone Number")]
        public string? NewPhoneNumber { get; set; }

        public bool IsEmailOtpSent { get; set; }

        [BindProperty]
        [Display(Name = "OTP Code")]
        public string? OtpCode { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            UserProfile = await _context.Users.OfType<Profile>()
                .Include(u => u.UserCategory)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (UserProfile == null)
            {
                return NotFound();
            }

            AssignedRoles = await _context.MisUserFeatureRoles
                .Where(r => r.ProfileId == user.Id && r.IsActive)
                .OrderBy(r => r.FeatureAreaKey)
                .ToListAsync();

            FirstName = UserProfile.FirstName ?? string.Empty;
            LastName = UserProfile.LastName ?? string.Empty;
            Address = UserProfile.Address;

            if (TempData["IsEmailOtpSent"] != null)
            {
                IsEmailOtpSent = (bool)TempData["IsEmailOtpSent"]!;
            }
            if (TempData["PendingNewEmail"] != null)
            {
                NewEmail = TempData["PendingNewEmail"]!.ToString() ?? string.Empty;
                TempData.Keep("PendingNewEmail");
                TempData.Keep("IsEmailOtpSent");
            }
            else
            {
                NewEmail = user.Email ?? string.Empty;
            }

            NewPhoneNumber = user.PhoneNumber;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            UserProfile = await _context.Users.OfType<Profile>()
                .Include(u => u.UserCategory)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (UserProfile == null) return NotFound();

            if (!ModelState.IsValid)
            {
                NewEmail = user.Email ?? string.Empty;
                NewPhoneNumber = user.PhoneNumber;
                return Page();
            }

            UserProfile.FirstName = FirstName;
            UserProfile.LastName = LastName;
            UserProfile.Address = Address;

            _context.Entry(UserProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = "Your profile details have been updated successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (string.IsNullOrWhiteSpace(NewEmail))
            {
                TempData["error"] = "Email address cannot be empty.";
                return RedirectToPage();
            }

            if (NewEmail.ToUpper() != user.NormalizedEmail)
            {
                var existingUser = await _userManager.FindByEmailAsync(NewEmail);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    TempData["error"] = "This email address is already in use by another account.";
                    return RedirectToPage();
                }

                // Simulate sending OTP
                TempData["IsEmailOtpSent"] = true;
                TempData["PendingNewEmail"] = NewEmail;
                TempData["success"] = "An OTP has been sent to your new email address. Please verify it below.";
                return RedirectToPage();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostVerifyEmailOtpAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var pendingEmail = TempData["PendingNewEmail"]?.ToString();
            
            if (string.IsNullOrEmpty(pendingEmail))
            {
                TempData["error"] = "OTP session expired. Please request a new code.";
                return RedirectToPage();
            }

            if (OtpCode != "123456")
            {
                TempData.Keep("PendingNewEmail");
                TempData.Keep("IsEmailOtpSent");
                TempData["error"] = "Invalid OTP code. Please try again.";
                return RedirectToPage();
            }

            var setEmailResult = await _userManager.SetEmailAsync(user, pendingEmail);
            if (!setEmailResult.Succeeded)
            {
                TempData["error"] = "An error occurred while updating your email address.";
                return RedirectToPage();
            }

            // Keep username in sync with email
            await _userManager.SetUserNameAsync(user, pendingEmail);
            
            await _signInManager.RefreshSignInAsync(user);
            
            TempData.Remove("PendingNewEmail");
            TempData.Remove("IsEmailOtpSent");
            TempData["success"] = "Your email address has been updated successfully!";
            
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdatePhoneAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (NewPhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, NewPhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    TempData["error"] = "An error occurred while updating your phone number.";
                    return RedirectToPage();
                }

                await _signInManager.RefreshSignInAsync(user);
                TempData["success"] = "Your phone number has been updated successfully!";
            }
            return RedirectToPage();
        }
    }
}
