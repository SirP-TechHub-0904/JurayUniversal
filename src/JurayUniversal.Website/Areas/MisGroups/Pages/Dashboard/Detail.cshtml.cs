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

        public MisGroup MisGroup { get; set; }
        public IList<MisGroupMember> Members { get; set; }
        public IList<Profile> AvailableUsers { get; set; }
        public string GroupAlias { get; set; } = "Group";

        [BindProperty]
        public string SelectedUserId { get; set; }
        [BindProperty]
        public string RolePosition { get; set; }

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

            Members = await _context.MisGroupMembers
                .Include(m => m.UserProfile)
                .Where(m => m.MisGroupId == id)
                .OrderByDescending(m => m.DateJoined)
                .ToListAsync();

            var memberUserIds = Members.Select(m => m.UserProfileId).ToList();
            
            // Get users who are not yet members
            AvailableUsers = await _userManager.Users
                .Where(u => !memberUserIds.Contains(u.Id))
                .OrderBy(u => u.FirstName)
                .Take(100) // Limit for performance
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

            // Create Member Record (Pending)
            var newMember = new MisGroupMember
            {
                MisGroupId = id,
                UserProfileId = SelectedUserId,
                RolePosition = RolePosition,
                MembershipStatus = "Pending",
                DateJoined = DateTime.UtcNow
            };
            _context.MisGroupMembers.Add(newMember);

            // Create Notification
            var notification = new MisNotification
            {
                UserId = SelectedUserId,
                Title = "New " + "Group" + " Invite",
                Message = $"You have been invited to join the '{group.Title}' " + "group" + ".",
                ActionUrl = $"/MisGroups/Platform/Index",
                IsRead = false,
                Module = "MisGroups",
                CreatedAt = DateTime.UtcNow
            };
            _context.MisNotifications.Add(notification);

            await _context.SaveChangesAsync();
            
            TempData["success"] = $"Invite sent to {invitee.FirstName} {invitee.LastName} successfully!";
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
