using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Platform
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string GroupAlias { get; set; } = "Group";

        public IList<MisGroup> MyGroups { get; set; } = new List<MisGroup>();

        public IList<MisGroupMember> PendingInvites { get; set; } = new List<MisGroupMember>();

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
            {
                GroupAlias = settings.GroupAlias;
            }

            // Get active groups
            MyGroups = await _context.MisGroupMembers
                .Include(m => m.MisGroup)
                .Where(m => m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active" && m.MisGroup.IsActive)
                .Select(m => m.MisGroup)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

            // Get pending invites
            PendingInvites = await _context.MisGroupMembers
                .Include(m => m.MisGroup)
                .Where(m => m.UserProfileId == currentUser.Id && m.MembershipStatus == "Pending")
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptInviteAsync(long inviteId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var invite = await _context.MisGroupMembers.Include(m => m.MisGroup).FirstOrDefaultAsync(m => m.Id == inviteId && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Pending");
            if (invite != null)
            {
                invite.MembershipStatus = "Active";
                invite.DateJoined = System.DateTime.UtcNow;

                // Also mark the notification as read
                var notif = await _context.MisNotifications.FirstOrDefaultAsync(n => n.UserId == currentUser.Id && n.ActionUrl.Contains("MisGroups") && n.Message.Contains(invite.MisGroup.Title));
                if (notif != null) notif.IsRead = true;

                await _context.SaveChangesAsync();
                TempData["success"] = "Welcome! You have joined the " + invite.MisGroup.Title;
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineInviteAsync(long inviteId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var invite = await _context.MisGroupMembers.Include(m => m.MisGroup).FirstOrDefaultAsync(m => m.Id == inviteId && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Pending");
            if (invite != null)
            {
                _context.MisGroupMembers.Remove(invite);

                // Also mark the notification as read
                var notif = await _context.MisNotifications.FirstOrDefaultAsync(n => n.UserId == currentUser.Id && n.ActionUrl.Contains("MisGroups") && n.Message.Contains(invite.MisGroup.Title));
                if (notif != null) notif.IsRead = true;

                await _context.SaveChangesAsync();
                TempData["success"] = "Invite declined.";
            }
            return RedirectToPage();
        }
    }
}
