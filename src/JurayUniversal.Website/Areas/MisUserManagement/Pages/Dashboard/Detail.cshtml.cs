using JurayUniversal.Application.Dtos.MIS.UserManagement;
using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class DetailModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IMisUserManagementRepository _userManagementRepo;
        private readonly UserManager<Profile> _userManager;

        public DetailModel(DashboardDbContext context, IMisUserManagementRepository userManagementRepo, UserManager<Profile> userManager)
        {
            _context = context;
            _userManagementRepo = userManagementRepo;
            _userManager = userManager;
        }

        public Profile UserProfile { get; set; } = null!;
        public List<MisUserFeatureRole> FeatureRoles { get; set; } = new();
        public List<MisKpiUserScore> KpiHistory { get; set; } = new();
        public List<MisUserNote> Notes { get; set; } = new();
        public bool IsTwoFactorEnabled { get; set; }

        [BindProperty]
        public string NewNoteText { get; set; } = string.Empty;

        [BindProperty]
        public bool NewNoteIsPrivate { get; set; } = true;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UserProfile = await _context.Users.OfType<Profile>()
                .Include(u => u.UserCategory)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (UserProfile == null)
            {
                return NotFound();
            }

            IsTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(UserProfile);
            FeatureRoles = await _userManagementRepo.GetUserFeatureRolesAsync(id);
            KpiHistory = await _userManagementRepo.GetUserScoreHistoryAsync(id);
            Notes = await _userManagementRepo.GetUserNotesAsync(id, includePrivate: true);

            return Page();
        }

        public async Task<IActionResult> OnPostAddNoteAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(NewNoteText))
            {
                return RedirectToPage(new { id });
            }

            var currentAdmin = await _userManager.GetUserAsync(User);
            var adminName = currentAdmin?.GetFullName();
            if (string.IsNullOrWhiteSpace(adminName)) adminName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(adminName)) adminName = "Admin";

            var dto = new MisUserNoteDto
            {
                ProfileId = id,
                Note = NewNoteText,
                IsPrivate = NewNoteIsPrivate,
                CreatedBy = adminName
            };

            await _userManagementRepo.AddUserNoteAsync(dto);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteNoteAsync(string id, long noteId)
        {
            if (noteId > 0)
            {
                await _userManagementRepo.DeleteUserNoteAsync(noteId);
            }
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostResetPasswordAsync(string id, string newPassword)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newPassword))
            {
                TempData["error"] = "Invalid password reset request.";
                return RedirectToPage(new { id });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["success"] = "Password successfully reset.";
            }
            else
            {
                TempData["error"] = "Failed to reset password: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDisable2faAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "Invalid user ID.";
                return RedirectToPage(new { id });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);

            if (result.Succeeded)
            {
                TempData["success"] = "Two-Factor Authentication (2FA) has been successfully disabled for this user.";
            }
            else
            {
                TempData["error"] = "Failed to disable 2FA: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToPage(new { id });
        }
    }
}
