using JurayUniversal.Application.Dtos.MIS.UserManagement;
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
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class KpiAssignModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IMisUserManagementRepository _userManagementRepo;
        private readonly UserManager<Profile> _userManager;

        public KpiAssignModel(DashboardDbContext context, IMisUserManagementRepository userManagementRepo, UserManager<Profile> userManager)
        {
            _context = context;
            _userManagementRepo = userManagementRepo;
            _userManager = userManager;
        }

        public Profile UserProfile { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string Period { get; set; } = string.Empty;

        [BindProperty]
        public List<MisKpiScoreDto> ScoreSheet { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UserProfile = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.Id == id);
            if (UserProfile == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(Period))
            {
                // Default to current quarter/year e.g., "Q2 2026"
                int quarter = (DateTime.UtcNow.Month - 1) / 3 + 1;
                Period = $"Q{quarter} {DateTime.UtcNow.Year}";
            }

            ScoreSheet = await _userManagementRepo.GetUserScoreSheetAsync(id, Period);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                UserProfile = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.Id == id);
                return Page();
            }

            var currentAdmin = await _userManager.GetUserAsync(User);
            var adminName = currentAdmin?.GetFullName();
            if (string.IsNullOrWhiteSpace(adminName)) adminName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(adminName)) adminName = "Admin";

            // Set period for all scores
            foreach (var score in ScoreSheet)
            {
                score.Period = Period;
                score.ProfileId = id;
            }

            await _userManagementRepo.SaveKpiScoresAsync(ScoreSheet, adminName);

            TempData["success"] = $"KPI scores for period {Period} saved successfully!";
            return RedirectToPage("/Dashboard/Detail", new { id });
        }
    }
}
