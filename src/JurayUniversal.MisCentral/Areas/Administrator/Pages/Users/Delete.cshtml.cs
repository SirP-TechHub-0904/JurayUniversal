using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Users
{
    [Authorize(Roles = "MasterSuperAdmin")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityUser? TargetUser { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            TargetUser = await _userManager.FindByIdAsync(id);
            if (TargetUser == null)
            {
                return NotFound();
            }

            Roles = await _userManager.GetRolesAsync(TargetUser);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == id)
            {
                ErrorMessage = "Security constraint: You cannot delete your own active Master Super Administrator account.";
                return RedirectToPage("/Users/Index", new { area = "Administrator" });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToPage("/Users/Index", new { area = "Administrator" });
        }
    }
}
