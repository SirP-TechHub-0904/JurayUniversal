using JurayUniversal.Application.Repository.MIS.UserManagement;
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

namespace JurayUniversal.Website.Areas.MIS.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class IndexModel : PageModel
    {
        private readonly IMisUserManagementRepository _userManagementRepo;
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(IMisUserManagementRepository userManagementRepo, DashboardDbContext context, UserManager<Profile> userManager)
        {
            _userManagementRepo = userManagementRepo;
            _context = context;
            _userManager = userManager;
        }

        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public int PendingUsers { get; set; }

        public List<MisFeature> MisFeatures { get; set; } = new List<MisFeature>();
        public List<MisWidget> MisWidgets { get; set; } = new List<MisWidget>();
        public List<JurayUniversal.Website.Areas.MisManagement.Pages.IndexModel.PredefinedFeature> AvailableFeatures { get; set; } = JurayUniversal.Website.Areas.MisManagement.Pages.IndexModel.AvailableFeatures ?? new();
        
        public List<string> RecentVisits { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync()
        {
            TotalUsers = await _userManagementRepo.GetTotalUsersCountAsync();
            ActiveUsers = await _userManagementRepo.GetActiveUsersCountAsync();
            SuspendedUsers = await _userManagementRepo.GetSuspendedUsersCountAsync();
            PendingUsers = await _userManagementRepo.GetPendingUsersCountAsync();

            AvailableFeatures = JurayUniversal.Website.Areas.MisManagement.Pages.IndexModel.AvailableFeatures ?? new();
            MisFeatures = await _context.MisFeatures.ToListAsync();

            // Auto-seed User Management if system is brand new
            if (!MisFeatures.Any())
            {
                var coreFeature = new MisFeature 
                { 
                    ModuleFolder = "MisUserManagement", 
                    Status = JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Active 
                };
                _context.MisFeatures.Add(coreFeature);
                await _context.SaveChangesAsync();
                MisFeatures.Add(coreFeature);
            }

            MisWidgets = await _context.MisWidgets.OrderBy(w => w.SortOrder).ToListAsync();

            // Load user's recent visits from DB
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                RecentVisits = await _context.MisUserRecentVisits
                    .Where(v => v.UserId == userId)
                    .OrderByDescending(v => v.LastVisitedAt)
                    .Take(6)
                    .Select(v => v.ModuleFolder)
                    .ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRecordVisitAsync(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return new JsonResult(new { success = false });

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return new JsonResult(new { success = false });

            var existingVisit = await _context.MisUserRecentVisits
                .FirstOrDefaultAsync(v => v.UserId == userId && v.ModuleFolder == moduleId);

            if (existingVisit != null)
            {
                existingVisit.LastVisitedAt = DateTime.UtcNow;
                _context.Update(existingVisit);
            }
            else
            {
                var newVisit = new MisUserRecentVisit
                {
                    UserId = userId,
                    ModuleFolder = moduleId,
                    LastVisitedAt = DateTime.UtcNow
                };
                _context.MisUserRecentVisits.Add(newVisit);
            }

            // Optional: Housekeeping to keep only latest 10 per user
            var allVisits = await _context.MisUserRecentVisits
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.LastVisitedAt)
                .ToListAsync();

            if (allVisits.Count > 10)
            {
                var toRemove = allVisits.Skip(10).ToList();
                _context.MisUserRecentVisits.RemoveRange(toRemove);
            }

            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostClearVisitsAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return new JsonResult(new { success = false });

            var allVisits = await _context.MisUserRecentVisits
                .Where(v => v.UserId == userId)
                .ToListAsync();

            _context.MisUserRecentVisits.RemoveRange(allVisits);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
