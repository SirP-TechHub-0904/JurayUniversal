using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MIS.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class IndexModel : PageModel
    {
        private readonly IMisUserManagementRepository _userManagementRepo;

        public IndexModel(IMisUserManagementRepository userManagementRepo)
        {
            _userManagementRepo = userManagementRepo;
        }

        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public int PendingUsers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TotalUsers = await _userManagementRepo.GetTotalUsersCountAsync();
            ActiveUsers = await _userManagementRepo.GetActiveUsersCountAsync();
            SuspendedUsers = await _userManagementRepo.GetSuspendedUsersCountAsync();
            PendingUsers = await _userManagementRepo.GetPendingUsersCountAsync();

            return Page();
        }
    }
}
