using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly JurayUniversal.Website.Services.IMisAuthorizationService _misAuth;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager, JurayUniversal.Website.Services.IMisAuthorizationService misAuth)
        {
            _context = context;
            _userManager = userManager;
            _misAuth = misAuth;
        }

        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int PendingUsers { get; set; }
        public int SuspendedUsers { get; set; }

        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DateRange { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool Is2FAEnabled { get; set; }
        public bool IsReadOnly { get; set; }

        public List<MisActivityLog> RecentActivities { get; set; } = new List<MisActivityLog>();

        // Graph Data
        public List<string> GraphLabels { get; set; } = new List<string>();
        public List<int> GraphNewUsers { get; set; } = new List<int>();
        public List<int> GraphSuspendedUsers { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }

            // Check if user has UserManagement role or is SuperAdmin
            var roles = await _userManager.GetRolesAsync(user);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor") || roles.Contains("UserManagement_Readonly");
            
            if (!hasAccess)
            {
                return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            }

            // Resolve ReadOnly status using IMisAuthorizationService
            IsReadOnly = !(await _misAuth.CanEditAsync(User, "MisUserManagement"));

            // Check 2FA
            Is2FAEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            // Parse DateRange
            if (!string.IsNullOrEmpty(DateRange))
            {
                var dates = DateRange.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParse(dates[0], out DateTime start)) StartDate = start;
                    if (DateTime.TryParse(dates[1], out DateTime end)) EndDate = end.AddDays(1).AddTicks(-1);
                }
            }

            // Calculate Statistics with Date Filtering
            var query = _context.Users.OfType<Profile>()
                .Where(u => u.UserName != "superadmin" && u.UserName != "msuperadmin" && u.Email != "superadmin@juray.ng")
                .AsQueryable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                query = query.Where(u => u.Date >= StartDate.Value && u.Date <= EndDate.Value);
            }

            var allUsers = await query.ToListAsync();
            TotalUsers = allUsers.Count;
            ActiveUsers = allUsers.Count(u => u.UserStatus == UserStatus.Active);
            PendingUsers = allUsers.Count(u => u.UserStatus == UserStatus.Pending);
            SuspendedUsers = allUsers.Count(u => u.UserStatus == UserStatus.Suspended);

            MaleCount = allUsers.Count(u => u.Gender == GenderStatus.Male);
            FemaleCount = allUsers.Count(u => u.Gender == GenderStatus.Female);

            // Graph Data Calculation (Group by Month)
            var groupedData = allUsers
                .GroupBy(u => new { u.Date.Year, u.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .ToList();

            foreach (var group in groupedData)
            {
                var monthName = new DateTime(group.Key.Year, group.Key.Month, 1).ToString("MMM yyyy");
                GraphLabels.Add(monthName);
                GraphNewUsers.Add(group.Count());
                GraphSuspendedUsers.Add(group.Count(u => u.UserStatus == UserStatus.Suspended));
            }
            
            // Default if empty
            if (!GraphLabels.Any())
            {
                GraphLabels.Add(DateTime.Now.ToString("MMM yyyy"));
                GraphNewUsers.Add(0);
                GraphSuspendedUsers.Add(0);
            }

            // Fetch Activities
            RecentActivities = await _context.MisActivityLogs
                .OrderByDescending(x => x.Date)
                .Take(10)
                .ToListAsync();

            return Page();
        }
    }
}
