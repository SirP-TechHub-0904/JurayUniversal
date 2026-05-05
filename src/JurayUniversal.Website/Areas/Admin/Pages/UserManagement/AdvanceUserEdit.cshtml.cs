using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]
    public class AdvanceUserEditModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<AdvanceUserEditModel> _logger;

        public AdvanceUserEditModel(
            UserManager<Profile> userManager,
            ILogger<AdvanceUserEditModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty] public Profile UserDatas { get; set; }

        // ── Security binds ──────────────────────────────────────────────
        [BindProperty] public bool TwoFactorEnabled { get; set; }
        [BindProperty] public bool LockoutEnabled { get; set; }
        [BindProperty] public bool EmailConfirmed { get; set; }
        [BindProperty] public bool PhoneNumberConfirmed { get; set; }
        [BindProperty] public bool ForcePasswordReset { get; set; }
        [BindProperty] public string? NewTempPassword { get; set; }
        [BindProperty] public string? AssignRole { get; set; }

        public List<string> AllRoles { get; set; } = new();
        public string CurrentRole { get; set; } = "";
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) return NotFound();

            UserDatas = await _userManager.FindByIdAsync(id);
            if (UserDatas == null) return NotFound();

            // Security state
            TwoFactorEnabled = UserDatas.TwoFactorEnabled;
            LockoutEnabled = UserDatas.LockoutEnabled;
            EmailConfirmed = UserDatas.EmailConfirmed;
            PhoneNumberConfirmed = UserDatas.PhoneNumberConfirmed;
            ForcePasswordReset = UserDatas.ResetPassword;

            // Current role
            var roles = await _userManager.GetRolesAsync(UserDatas);
            CurrentRole = roles.FirstOrDefault() ?? "None";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(UserDatas.Id);
            if (user == null) return NotFound();

            // ── Section 1: Security & Account ──────────────────────────
            if (user.TwoFactorEnabled != TwoFactorEnabled)
                await _userManager.SetTwoFactorEnabledAsync(user, TwoFactorEnabled);

            if (user.LockoutEnabled != LockoutEnabled)
            {
                await _userManager.SetLockoutEnabledAsync(user, LockoutEnabled);
                if (!LockoutEnabled)
                    await _userManager.ResetAccessFailedCountAsync(user);
            }

            user.EmailConfirmed = EmailConfirmed;
            user.PhoneNumberConfirmed = PhoneNumberConfirmed;
            user.ResetPassword = ForcePasswordReset;

            if (!string.IsNullOrWhiteSpace(NewTempPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, NewTempPassword);
                if (passResult.Succeeded)
                {
                    user.TempPass = NewTempPassword;
                    user.ResetPassword = true;
                }
                else
                {
                    foreach (var e in passResult.Errors)
                        ModelState.AddModelError("", e.Description);
                    return Page();
                }
            }

            // ── Section 2: Role ─────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(AssignRole))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, AssignRole);
                user.Role = AssignRole;
            }

            // ── Section 3: Profile Status Flags ────────────────────────
            user.UserStatus = UserDatas.UserStatus;
            user.UserCategoryId = UserDatas.UserCategoryId;
            user.JobDesignationId = UserDatas.JobDesignationId;
            user.PortfolioProfile = UserDatas.PortfolioProfile;
            user.DateOfBirthStatus = UserDatas.DateOfBirthStatus;

            // ── Section 4: Communication ────────────────────────────────
            user.EmailSent = UserDatas.EmailSent;
            user.SmsSent = UserDatas.SmsSent;
            user.ContactMailEmail = UserDatas.ContactMailEmail;

            // ── Section 5: Accommodation ────────────────────────────────
            user.HasAccomodation = UserDatas.HasAccomodation;
            user.LastAccomodation = UserDatas.LastAccomodation;
            user.AccomodationUpdateStatus = UserDatas.AccomodationUpdateStatus;

            // ── Section 6: Profile Completion Overrides ─────────────────
            user.UpdateProfile = UserDatas.UpdateProfile;
            user.UpdateEducation = UserDatas.UpdateEducation;
            user.UpdateExperience = UserDatas.UpdateExperience;
            user.UpdateCertificate = UserDatas.UpdateCertificate;
            user.UpdateAwards = UserDatas.UpdateAwards;
            user.UpdateSkills = UserDatas.UpdateSkills;
            user.UpdateLanguage = UserDatas.UpdateLanguage;
            user.UpdateInterest = UserDatas.UpdateInterest;
            user.UpdateReference = UserDatas.UpdateReference;

            // ── Section 7: Identity & Sensitive Info ────────────────────
            user.UniqueId = UserDatas.UniqueId;
            user.Title = UserDatas.Title;
            user.PositionOrRank = UserDatas.PositionOrRank;
            user.Nationality = UserDatas.Nationality;
            user.BloodGroup = UserDatas.BloodGroup;
            user.Genotype = UserDatas.Genotype;
            user.BVN = UserDatas.BVN;
            user.InternationalPassportNumber = UserDatas.InternationalPassportNumber;
            user.InternationalPassportExpiringDate = UserDatas.InternationalPassportExpiringDate;
            user.Sponsor = UserDatas.Sponsor;
            user.SponsorAddress = UserDatas.SponsorAddress;
            user.Referee = UserDatas.Referee;
            user.PreviousAppointment = UserDatas.PreviousAppointment;
            user.RefCode = UserDatas.RefCode;
            user.EmergencyContactName = UserDatas.EmergencyContactName;
            user.EmergencyContactNumber = UserDatas.EmergencyContactNumber;
            user.EmergencyContactRelationship = UserDatas.EmergencyContactRelationship;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);
                return Page();
            }

            _logger.LogInformation("Advanced edit saved for user {Id}", user.Id);
            TempData["Success"] = "Advanced profile updated successfully.";
            return RedirectToPage("./AdvanceUserEdit", new { id = user.Id });
        }
    }
}