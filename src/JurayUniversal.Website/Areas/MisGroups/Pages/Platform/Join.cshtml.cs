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

        public MisGroup MisGroup { get; set; }
        public string GroupAlias { get; set; } = "Group";
        public MisGroupMember ExistingMembership { get; set; }

        public async Task<IActionResult> OnGetAsync(long code)
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
                GroupAlias = settings.GroupAlias;

            MisGroup = await _context.MisGroups.FirstOrDefaultAsync(g => g.Id == code);
            if (MisGroup == null) return NotFound("The requested group invite link is invalid or the group has been deleted.");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ExistingMembership = await _context.MisGroupMembers
                    .FirstOrDefaultAsync(m => m.MisGroupId == code && m.UserProfileId == currentUser.Id);
                
                if (ExistingMembership != null && ExistingMembership.MembershipStatus == "Active")
                {
                    // They are already in the group, skip the join page.
                    return RedirectToPage("./Workspace", new { id = code });
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long code)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var group = await _context.MisGroups.FirstOrDefaultAsync(g => g.Id == code);
            if (group == null) return NotFound();

            var existing = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == code && m.UserProfileId == currentUser.Id);

            if (existing == null)
            {
                var membership = new MisGroupMember
                {
                    MisGroupId = code,
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
