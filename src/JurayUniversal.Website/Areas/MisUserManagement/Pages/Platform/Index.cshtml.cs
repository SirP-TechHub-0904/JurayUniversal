using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Platform
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class IndexModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;

        public IndexModel(UserManager<Profile> userManager)
        {
            _userManager = userManager;
        }

        public Profile UserProfile { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            UserProfile = user;
            return Page();
        }
    }
}
