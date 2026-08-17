using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Pages.Pay
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<MisFinanceCategory> Categories { get; set; } = new();
        public SuperSetting SuperSetting { get; set; }
        public string OrgName { get; set; } = "Payment Portal";
        public bool IsLoggedIn { get; set; }
        public string LoggedInFullName { get; set; }
        public List<string> UserGroupNames { get; set; } = new();
        public string UserCategoryName { get; set; }

        public async Task OnGetAsync()
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync() ?? new SuperSetting();
            OrgName = SuperSetting.CompanyName ?? OrgName;

            IsLoggedIn = User.Identity?.IsAuthenticated == true;

            var query = _context.MisFinanceCategories
                .Include(c => c.MisGroup)
                .Include(c => c.UserCategory)
                .Where(c => c.IsActive);

            if (IsLoggedIn)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    LoggedInFullName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : $"{user.FirstName} {user.LastName}".Trim();
                    if (string.IsNullOrEmpty(LoggedInFullName))
                    {
                        LoggedInFullName = user.Email ?? user.UserName;
                    }

                    // Retrieve active user group IDs and group names
                    var userGroups = await _context.MisGroupMembers
                        .Include(m => m.MisGroup)
                        .Where(m => m.UserProfileId == user.Id && m.MembershipStatus == "Active")
                        .ToListAsync();

                    var userGroupIds = userGroups.Select(m => m.MisGroupId).ToList();
                    UserGroupNames = userGroups.Where(g => g.MisGroup != null).Select(g => g.MisGroup.Title).ToList();

                    // Retrieve user category name
                    if (user.UserCategoryId.HasValue)
                    {
                        var userCat = await _context.UserCategories.FindAsync(user.UserCategoryId.Value);
                        UserCategoryName = userCat?.Title;
                    }

                    var userCatId = user.UserCategoryId;

                    // Filter categories: General OR matching user's group OR matching user's category
                    query = query.Where(c =>
                        (c.MisGroupId == null && c.UserCategoryId == null) ||
                        (c.MisGroupId != null && userGroupIds.Contains(c.MisGroupId.Value)) ||
                        (c.UserCategoryId != null && userCatId.HasValue && c.UserCategoryId == userCatId.Value));
                }
            }
            else
            {
                // Anonymous guests only see general public categories (not targeted to a specific group or category)
                query = query.Where(c => c.AllowGuestPayment && c.MisGroupId == null && c.UserCategoryId == null);
            }

            Categories = await query
                .OrderBy(c => c.IsCompulsory ? 0 : 1)
                .ThenBy(c => c.Title)
                .ToListAsync();
        }
    }
}
