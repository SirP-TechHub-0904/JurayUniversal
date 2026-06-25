using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisAlumni.Pages.Platform
{
    [Authorize]
    public class OpportunitiesModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public OpportunitiesModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string AlumniAlias { get; set; } = "Alumni";
        public bool IsAlumni { get; set; }

        public IList<MisAlumniPost> Posts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterType { get; set; }

        [BindProperty]
        public string NewPostTitle { get; set; }

        [BindProperty]
        public string NewPostContent { get; set; }

        [BindProperty]
        public string NewPostType { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.AlumniAlias))
            {
                AlumniAlias = settings.AlumniAlias;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var profile = await _context.MisAlumniProfiles.FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id);
                IsAlumni = profile != null && profile.VerificationStatus == "Verified";
            }

            var query = _context.MisAlumniPosts
                .Include(p => p.UserProfile)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(FilterType))
            {
                query = query.Where(p => p.PostType == FilterType);
            }

            Posts = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var profile = await _context.MisAlumniProfiles.FirstOrDefaultAsync(p => p.UserProfileId == currentUser.Id);
            if (profile == null || profile.VerificationStatus != "Verified")
            {
                TempData["error"] = "Only verified alumni can post opportunities.";
                return RedirectToPage();
            }

            if (string.IsNullOrWhiteSpace(NewPostTitle) || string.IsNullOrWhiteSpace(NewPostContent))
            {
                TempData["error"] = "Title and Content are required.";
                return RedirectToPage();
            }

            var post = new MisAlumniPost
            {
                UserProfileId = currentUser.Id,
                Title = NewPostTitle,
                Content = NewPostContent,
                PostType = string.IsNullOrEmpty(NewPostType) ? "General" : NewPostType,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.MisAlumniPosts.Add(post);
            await _context.SaveChangesAsync();

            TempData["success"] = "Your opportunity was posted successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var post = await _context.MisAlumniPosts.FindAsync(id);
            if (post != null && post.UserProfileId == currentUser.Id)
            {
                _context.MisAlumniPosts.Remove(post);
                await _context.SaveChangesAsync();
                TempData["success"] = "Post deleted successfully.";
            }

            return RedirectToPage();
        }
    }
}
