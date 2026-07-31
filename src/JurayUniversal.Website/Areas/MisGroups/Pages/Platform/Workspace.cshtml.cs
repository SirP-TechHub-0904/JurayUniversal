using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Platform
{
    public class ChatMessageInput
    {
        public string Content { get; set; } = string.Empty;
    }

    [Authorize]
    public class WorkspaceModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IStorageService _storageService;
        private readonly IMisAuthorizationService _misAuth;

        public WorkspaceModel(DashboardDbContext context, UserManager<Profile> userManager, IStorageService storageService, IMisAuthorizationService misAuth)
        {
            _context = context;
            _userManager = userManager;
            _storageService = storageService;
            _misAuth = misAuth;
        }

        public MisGroup MisGroup { get; set; } = null!;
        public string GroupAlias { get; set; } = "Group";
        public string CurrentUserId { get; set; } = string.Empty;
        public string CurrentGroupToken { get; set; } = string.Empty;
        public bool IsAdminReadOnly { get; set; } = false;
        
        public IList<MisGroupPost> Posts { get; set; } = new List<MisGroupPost>();
        public IList<MisGroupFile> Files { get; set; } = new List<MisGroupFile>();
        public IList<MisGroupMember> Members { get; set; } = new List<MisGroupMember>();

        public MisGroupMember? CurrentMembership { get; set; }
        public Dictionary<string, string> MemberRoleMap { get; set; } = new();

        [BindProperty]
        public string NewPostContent { get; set; } = string.Empty;

        [BindProperty]
        public string FileTitle { get; set; } = string.Empty;
        [BindProperty]
        public IFormFile Upload { get; set; } = null!;

        public static string GetGroupToken(long groupId)
        {
            var plainBytes = System.Text.Encoding.UTF8.GetBytes($"grp_{groupId}_sec");
            return Convert.ToBase64String(plainBytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public static long? ResolveGroupIdFromToken(string? token, long? fallbackId = null)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    string padded = token.Replace("-", "+").Replace("_", "/");
                    switch (padded.Length % 4)
                    {
                        case 2: padded += "=="; break;
                        case 3: padded += "="; break;
                    }
                    byte[] bytes = Convert.FromBase64String(padded);
                    string decoded = System.Text.Encoding.UTF8.GetString(bytes);

                    if (decoded.StartsWith("grp_") && decoded.EndsWith("_sec"))
                    {
                        string idStr = decoded.Substring(4, decoded.Length - 8);
                        if (long.TryParse(idStr, out long parsedId)) return parsedId;
                    }

                    if (decoded.StartsWith("group_"))
                    {
                        string idStr = decoded.Substring(6);
                        if (long.TryParse(idStr, out long parsedId)) return parsedId;
                    }

                    if (long.TryParse(decoded, out long directId)) return directId;
                }
                catch { }

                if (long.TryParse(token, out long rawTokenId)) return rawTokenId;
            }
            return fallbackId;
        }

        public async Task<IActionResult> OnGetAsync(string? token, long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Forbid();

            CurrentUserId = currentUser.Id;

            long? targetId = ResolveGroupIdFromToken(token, id);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;
            CurrentGroupToken = GetGroupToken(groupId);

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
                GroupAlias = settings.GroupAlias;

            MisGroup = await _context.MisGroups.FirstOrDefaultAsync(m => m.Id == groupId);
            if (MisGroup == null) return NotFound();

            CurrentMembership = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            
            bool hasAdminAccess = await _misAuth.HasAccessAsync(User, "MisGroups") || 
                                 User.IsInRole("mSuperAdmin") || 
                                 User.IsInRole("SuperAdmin") || 
                                 User.IsInRole("Administrator") || 
                                 User.IsInRole("Admin");

            if (CurrentMembership == null)
            {
                if (hasAdminAccess)
                {
                    IsAdminReadOnly = true;
                }
                else
                {
                    TempData["error"] = "You do not have access to this workspace.";
                    return RedirectToPage("./Index");
                }
            }

            Posts = await _context.MisGroupPosts
                .Include(p => p.UserProfile)
                .Where(p => p.MisGroupId == groupId)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();

            Files = await _context.MisGroupFiles
                .Include(f => f.UploadedByUser)
                .Where(f => f.MisGroupId == groupId)
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();

            Members = await _context.MisGroupMembers
                .Include(m => m.UserProfile)
                .Where(m => m.MisGroupId == groupId && m.MembershipStatus == "Active")
                .OrderBy(m => m.RolePosition)
                .ToListAsync();

            foreach (var m in Members)
            {
                if (!string.IsNullOrEmpty(m.UserProfileId))
                {
                    MemberRoleMap[m.UserProfileId] = string.IsNullOrWhiteSpace(m.RolePosition) ? "Member" : m.RolePosition.Trim();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSendJsonPostAsync(string? token, long? id, [FromBody] ChatMessageInput input)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || string.IsNullOrWhiteSpace(input?.Content))
                return new JsonResult(new { success = false, message = "Invalid input or unauthenticated." });

            long? targetId = ResolveGroupIdFromToken(token, id);
            if (!targetId.HasValue) return new JsonResult(new { success = false, message = "Invalid group token." });

            long groupId = targetId.Value;

            var member = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            if (member == null)
                return new JsonResult(new { success = false, message = "Only active committee members can post messages." });

            string cleanContent = input.Content.Trim();
            if (cleanContent.Length > 1000) cleanContent = cleanContent.Substring(0, 1000);

            var post = new MisGroupPost
            {
                MisGroupId = groupId,
                UserProfileId = currentUser.Id,
                Content = cleanContent,
                CreatedAt = DateTime.UtcNow
            };

            _context.MisGroupPosts.Add(post);
            await _context.SaveChangesAsync();

            string role = string.IsNullOrWhiteSpace(member.RolePosition) ? "Member" : member.RolePosition.Trim();

            return new JsonResult(new
            {
                success = true,
                postId = post.Id,
                senderId = currentUser.Id,
                senderName = currentUser.GetFullName(),
                senderRole = role,
                content = post.Content,
                createdAt = post.CreatedAt.ToString("MMM dd, h:mm tt")
            });
        }

        public async Task<IActionResult> OnPostTogglePinJsonAsync(long postId, string? token, long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return new JsonResult(new { success = false, message = "Unauthenticated." });

            long? targetId = ResolveGroupIdFromToken(token, id);
            if (!targetId.HasValue) return new JsonResult(new { success = false, message = "Invalid token." });

            long groupId = targetId.Value;

            var post = await _context.MisGroupPosts
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(p => p.Id == postId && p.MisGroupId == groupId);

            if (post == null) return new JsonResult(new { success = false, message = "Post not found." });

            bool nowPinned = false;
            if (!post.IsPinned)
            {
                var existingPinned = await _context.MisGroupPosts.Where(p => p.MisGroupId == groupId && p.IsPinned).ToListAsync();
                foreach (var p in existingPinned) p.IsPinned = false;
                post.IsPinned = true;
                nowPinned = true;
            }
            else
            {
                post.IsPinned = false;
            }

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                postId = post.Id,
                isPinned = nowPinned,
                content = post.Content,
                senderName = post.UserProfile?.GetFullName() ?? "User"
            });
        }

        public async Task<IActionResult> OnPostAddPostAsync(string? token, long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || string.IsNullOrWhiteSpace(NewPostContent)) return RedirectToPage(new { token });

            long? targetId = ResolveGroupIdFromToken(token, id);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;

            var member = await _context.MisGroupMembers.FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            if (member == null)
            {
                TempData["error"] = "Admin Oversight Mode: Only active committee members can post messages.";
                return RedirectToPage(new { token = GetGroupToken(groupId) });
            }

            string cleanContent = NewPostContent.Trim();
            if (cleanContent.Length > 1000)
            {
                cleanContent = cleanContent.Substring(0, 1000);
            }

            var post = new MisGroupPost
            {
                MisGroupId = groupId,
                UserProfileId = currentUser.Id,
                Content = cleanContent,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.MisGroupPosts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { token = GetGroupToken(groupId) });
        }

        public async Task<IActionResult> OnPostTogglePinAsync(long postId, string? token, long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            long? targetId = ResolveGroupIdFromToken(token, id);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;

            var post = await _context.MisGroupPosts.FirstOrDefaultAsync(p => p.Id == postId && p.MisGroupId == groupId);
            if (post != null)
            {
                if (!post.IsPinned)
                {
                    var existingPinned = await _context.MisGroupPosts.Where(p => p.MisGroupId == groupId && p.IsPinned).ToListAsync();
                    foreach (var p in existingPinned) p.IsPinned = false;
                    post.IsPinned = true;
                    TempData["success"] = "Message pinned as group announcement!";
                }
                else
                {
                    post.IsPinned = false;
                    TempData["info"] = "Message unpinned.";
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { token = GetGroupToken(groupId) });
        }

        public async Task<IActionResult> OnPostAddFileAsync(string? token, long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || string.IsNullOrWhiteSpace(FileTitle) || Upload == null) return RedirectToPage(new { token });

            long? targetId = ResolveGroupIdFromToken(token, id);
            if (!targetId.HasValue) return NotFound();

            long groupId = targetId.Value;

            var member = await _context.MisGroupMembers.FirstOrDefaultAsync(m => m.MisGroupId == groupId && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            if (member == null)
            {
                TempData["error"] = "Admin Oversight Mode: Only active committee members can upload files.";
                return RedirectToPage(new { token = GetGroupToken(groupId) });
            }

            var xresult = await _storageService.MainUploadFileReturnUrlAsync("", Upload);
            if (xresult == null || string.IsNullOrEmpty(xresult.Url))
            {
                TempData["error"] = "Failed to upload file to AWS server.";
                return RedirectToPage(new { token = GetGroupToken(groupId) });
            }

            var file = new MisGroupFile
            {
                MisGroupId = groupId,
                UploadedByUserId = currentUser.Id,
                FileName = FileTitle,
                FileUrl = xresult.Url,
                UploadedAt = DateTime.UtcNow
            };
            _context.MisGroupFiles.Add(file);

            var post = new MisGroupPost
            {
                MisGroupId = groupId,
                UserProfileId = currentUser.Id,
                Content = $"📎 Shared a file: [{FileTitle}]({xresult.Url})",
                CreatedAt = DateTime.UtcNow
            };
            _context.MisGroupPosts.Add(post);

            await _context.SaveChangesAsync();
            return RedirectToPage(new { token = GetGroupToken(groupId) });
        }
    }
}
