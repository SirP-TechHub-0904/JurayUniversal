using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard.Categories
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class DeleteModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DeleteModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UserCategory UserCategory { get; set; }

        public string CategoryAlias { get; set; } = "Category";

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor");
            
            if (!hasAccess)
            {
                return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            }

            if (id == null)
            {
                return NotFound();
            }

            UserCategory = await _context.UserCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (UserCategory == null)
            {
                return NotFound();
            }

            var misSetting = await _context.MisSettings.FirstOrDefaultAsync();
            if (misSetting != null && !string.IsNullOrEmpty(misSetting.CategoryAlias))
            {
                CategoryAlias = misSetting.CategoryAlias;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor");
            
            if (!hasAccess)
            {
                return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            }

            if (id == null)
            {
                return NotFound();
            }

            UserCategory = await _context.UserCategories.FindAsync(id);

            if (UserCategory != null)
            {
                _context.UserCategories.Remove(UserCategory);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully.";
            }

            return RedirectToPage("./Index");
        }
    }
}
