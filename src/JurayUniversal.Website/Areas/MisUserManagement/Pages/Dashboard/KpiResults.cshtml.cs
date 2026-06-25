using JurayUniversal.Application.Dtos.MIS.UserManagement;
using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class KpiResultsModel : PageModel
    {
        private readonly IMisUserManagementRepository _userManagementRepo;

        public KpiResultsModel(IMisUserManagementRepository userManagementRepo)
        {
            _userManagementRepo = userManagementRepo;
        }

        public List<string> Periods { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SelectedPeriod { get; set; }

        public List<MisKpiLeaderboardDto> Leaderboard { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Periods = await _userManagementRepo.GetAllPeriodsAsync();

            if (string.IsNullOrEmpty(SelectedPeriod) && Periods.Count > 0)
            {
                SelectedPeriod = Periods[0];
            }

            if (!string.IsNullOrEmpty(SelectedPeriod))
            {
                Leaderboard = await _userManagementRepo.GetKpiLeaderboardAsync(SelectedPeriod);
            }

            return Page();
        }
    }
}
