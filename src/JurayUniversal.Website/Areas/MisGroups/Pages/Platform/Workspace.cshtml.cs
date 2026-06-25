using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
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
    [Authorize]
    public class WorkspaceModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IStorageService _storageService;

        public WorkspaceModel(DashboardDbContext context, UserManager<Profile> userManager, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _storageService = storageService;
        }

        public MisGroup MisGroup { get; set; }
        public string GroupAlias { get; set; } = "Group";
        
        public IList<MisGroupPost> Posts { get; set; }
        public IList<MisGroupFile> Files { get; set; }
        public IList<MisGroupMember> Members { get; set; }

        public MisGroupMember CurrentMembership { get; set; }

        [BindProperty]
        public string NewPostContent { get; set; }

        [BindProperty]
        public string FileTitle { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Forbid();

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
                GroupAlias = settings.GroupAlias;

            MisGroup = await _context.MisGroups.FirstOrDefaultAsync(m => m.Id == id);
            if (MisGroup == null) return NotFound();

            // Verify they are an active member
            CurrentMembership = await _context.MisGroupMembers
                .FirstOrDefaultAsync(m => m.MisGroupId == id && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            
            if (CurrentMembership == null)
            {
                TempData["error"] = "You do not have access to this workspace.";
                return RedirectToPage("./Index");
            }

            Posts = await _context.MisGroupPosts
                .Include(p => p.UserProfile)
                .Where(p => p.MisGroupId == id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            Files = await _context.MisGroupFiles
                .Include(f => f.UploadedByUser)
                .Where(f => f.MisGroupId == id)
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();

            Members = await _context.MisGroupMembers
                .Include(m => m.UserProfile)
                .Where(m => m.MisGroupId == id && m.MembershipStatus == "Active")
                .OrderBy(m => m.RolePosition)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddPostAsync(long id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || string.IsNullOrWhiteSpace(NewPostContent)) return RedirectToPage(new { id });

            var member = await _context.MisGroupMembers.FirstOrDefaultAsync(m => m.MisGroupId == id && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            if (member == null) return Forbid();

            var post = new MisGroupPost
            {
                MisGroupId = id,
                UserProfileId = currentUser.Id,
                Content = NewPostContent,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.MisGroupPosts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostAddFileAsync(long id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || string.IsNullOrWhiteSpace(FileTitle) || Upload == null) return RedirectToPage(new { id });

            var member = await _context.MisGroupMembers.FirstOrDefaultAsync(m => m.MisGroupId == id && m.UserProfileId == currentUser.Id && m.MembershipStatus == "Active");
            if (member == null) return Forbid();

            var xresult = await _storageService.MainUploadFileReturnUrlAsync("", Upload);
            if (xresult == null || string.IsNullOrEmpty(xresult.Url))
            {
                TempData["error"] = "Failed to upload file to AWS server.";
                return RedirectToPage(new { id });
            }

            var file = new MisGroupFile
            {
                MisGroupId = id,
                UploadedByUserId = currentUser.Id,
                FileName = FileTitle,
                FileUrl = xresult.Url,
                UploadedAt = DateTime.UtcNow
            };
            
            _context.MisGroupFiles.Add(file);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id });
        }
    }
}
