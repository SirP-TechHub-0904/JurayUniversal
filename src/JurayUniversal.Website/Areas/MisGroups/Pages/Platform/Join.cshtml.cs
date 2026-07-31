using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Platform
{
    [Authorize]
    public class JoinModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public JoinModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisGroup MisGroup { get; set; } = null!;
        public string GroupAlias { get; set; } = "Group";
        public MisGroupMember? ExistingMembership { get; set; }
        public string CurrentToken { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string? token, long? code)
        {
            long? targetId = WorkspaceModel.ResolveGroupIdFromToken(token, code);
            if (!targetId.HasValue) return NotFound("The requested group invite link is invalid or the group has been deleted.");

            long groupId = targetId.Value;
            CurrentToken = WorkspaceModel.GetGroupToken(groupId);

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
                GroupAlias = settings.GroupAlias;

            MisGroup = await _context.MisGroups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (MisGroup == null) return NotFound("The requested group invite link is invalid or the group has been deleted.");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ExistingMembership = await _context.MisGroupMembers
                    .FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id);
                
                if (ExistingMembership != null && ExistingMembership.MembershipStatus == "Active")
                {
                    return RedirectToPage("./Workspace", new { token = CurrentToken });
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync(string? token, long? code)
        {
            long? targetId = WorkspaceModel.ResolveGroupIdFromToken(token, code);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var membership = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id);

            if (membership != null)
            {
                membership.MembershipStatus = "Active";
                await _context.SaveChangesAsync();
                TempData["success"] = $"Welcome! You have accepted the invitation and joined {MisGroup?.Title ?? GroupAlias}.";
            }

            return RedirectToPage("./Workspace", new { token = WorkspaceModel.GetGroupToken(groupId) });
        }

        public async Task<IActionResult> OnPostDeclineAsync(string? token, long? code)
        {
            long? targetId = WorkspaceModel.ResolveGroupIdFromToken(token, code);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var membership = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id);

            if (membership != null)
            {
                _context.MisGroupMembers.Remove(membership);
                await _context.SaveChangesAsync();
                TempData["info"] = "Invitation declined.";
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRequestJoinAsync(string? token, long? code)
        {
            long? targetId = WorkspaceModel.ResolveGroupIdFromToken(token, code);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var group = await _context.MisGroups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null) return NotFound();

            var existing = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id);

            if (existing == null)
            {
                var membership = new MisGroupMember
                {
                    MisGroupId = groupId,
                    UserProfileId = currentUser.Id,
                    MembershipStatus = "Pending",
                    RolePosition = "Member",
                    DateJoined = DateTime.UtcNow
                };
                _context.MisGroupMembers.Add(membership);

                var notif = new MisNotification
                {
                    UserId = currentUser.Id,
                    Title = "Join Request Sent",
                    Message = $"Your request to join {group.Title} has been sent and is awaiting admin approval.",
                    ActionUrl = $"/MisGroups/Platform/Index",
                    Module = "MisGroups",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                _context.MisNotifications.Add(notif);

                await _context.SaveChangesAsync();
                TempData["success"] = $"Your request to join {group.Title} has been sent successfully.";
            }

            return RedirectToPage("./Index");
        }
    }
}
