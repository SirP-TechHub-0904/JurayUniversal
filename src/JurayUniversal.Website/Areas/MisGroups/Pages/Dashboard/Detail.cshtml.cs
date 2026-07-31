using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Dashboard
{
    [Authorize]
    public class DetailModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IMisAuthorizationService _misAuth;

        public DetailModel(DashboardDbContext context, UserManager<Profile> userManager, IMisAuthorizationService misAuth)
        {
            _context = context;
            _userManager = userManager;
            _misAuth = misAuth;
        }

        public MisGroup MisGroup { get; set; } = null!;
        public IList<MisGroupMember> Members { get; set; } = new List<MisGroupMember>();
        public IList<Profile> AvailableUsers { get; set; } = new List<Profile>();
        public string GroupAlias { get; set; } = "Group";
        public string ObfuscatedInviteToken { get; set; } = string.Empty;

        [BindProperty]
        public string SelectedUserId { get; set; } = string.Empty;

        [BindProperty]
        public string? RolePosition { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (!await _misAuth.HasAccessAsync(User, "MisGroups"))
                return Forbid();

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
                GroupAlias = settings.GroupAlias;

            MisGroup = await _context.MisGroups.FirstOrDefaultAsync(m => m.Id == id);

            if (MisGroup == null)
                return NotFound();

            // Obfuscate ID for invite URL
            var plainBytes = System.Text.Encoding.UTF8.GetBytes($"group_{id}");
            ObfuscatedInviteToken = Convert.ToBase64String(plainBytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');

            var allMembers = await _context.MisGroupMembers
                .Include(m => m.UserProfile)
                .Where(m => m.MisGroupId == id)
                .ToListAsync();

            // Order Officials (non-Members) first, and standard Members last
            Members = allMembers
                .OrderBy(m => string.IsNullOrWhiteSpace(m.RolePosition) || m.RolePosition.Trim().Equals("Member", StringComparison.OrdinalIgnoreCase) ? 1 : 0)
                .ThenBy(m => m.RolePosition)
                .ThenByDescending(m => m.DateJoined)
                .ToList();

            var memberUserIds = Members.Select(m => m.UserProfileId).ToList();
            
            AvailableUsers = await _userManager.Users
                .Where(u => !memberUserIds.Contains(u.Id))
                .OrderBy(u => u.FirstName)
                .Take(100)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostInviteAsync(long id)
        {
            if (!await _misAuth.CanEditAsync(User, "MisGroups"))
                return Forbid();

            var group = await _context.MisGroups.FirstOrDefaultAsync(m => m.Id == id);
            if (group == null || string.IsNullOrEmpty(SelectedUserId))
                return RedirectToPage(new { id });

            var invitee = await _userManager.FindByIdAsync(SelectedUserId);
            if (invitee == null)
                return RedirectToPage(new { id });

            string assignedRole = string.IsNullOrWhiteSpace(RolePosition) ? "Member" : RolePosition.Trim();

            var newMember = new MisGroupMember
            {
                MisGroupId = id,
                UserProfileId = SelectedUserId,
                RolePosition = assignedRole,
                MembershipStatus = "Pending",
                DateJoined = DateTime.UtcNow
            };
            _context.MisGroupMembers.Add(newMember);

            var notification = new MisNotification
            {
                UserId = SelectedUserId,
                Title = "New " + GroupAlias + " Invite",
                Message = $"You have been invited to join the '{group.Title}' {GroupAlias}.",
                ActionUrl = $"/MisGroups/Platform/Index",
                IsRead = false,
                Module = "MisGroups",
                CreatedAt = DateTime.UtcNow
            };
            _context.MisNotifications.Add(notification);

            await _context.SaveChangesAsync();
            
            TempData["success"] = $"Invite sent to {invitee.FirstName} {invitee.LastName} as {assignedRole} successfully!";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(long memberId, long id)
        {
            if (!await _misAuth.CanEditAsync(User, "MisGroups"))
                return Forbid();

            var member = await _context.MisGroupMembers.FirstOrDefaultAsync(m => m.Id == memberId);
            if (member != null)
            {
                if (member.MembershipStatus == "Suspended")
                {
                    member.MembershipStatus = "Active";
                    TempData["success"] = "Member account activated.";
                }
                else
                {
                    member.MembershipStatus = "Suspended";
                    TempData["success"] = "Member account suspended.";
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRemoveAsync(long memberId, long id)
        {
            if (!await _misAuth.CanEditAsync(User, "MisGroups"))
                return Forbid();

            var member = await _context.MisGroupMembers.FirstOrDefaultAsync(m => m.Id == memberId);
            if (member != null)
            {
                _context.MisGroupMembers.Remove(member);
                await _context.SaveChangesAsync();
                TempData["success"] = "Member removed.";
            }

            return RedirectToPage(new { id });
        }
    }
}
