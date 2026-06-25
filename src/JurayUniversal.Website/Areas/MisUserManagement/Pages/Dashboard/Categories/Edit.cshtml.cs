using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard.Categories
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public EditModel(DashboardDbContext context, UserManager<Profile> userManager)
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

        public async Task<IActionResult> OnPostAsync()
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

            ModelState.Remove("UserCategory.Profiles");

            if (!ModelState.IsValid)
            {
                var misSetting = await _context.MisSettings.FirstOrDefaultAsync();
                if (misSetting != null && !string.IsNullOrEmpty(misSetting.CategoryAlias))
                {
                    CategoryAlias = misSetting.CategoryAlias;
                }
                return Page();
            }

            _context.Attach(UserCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCategoryExists(UserCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Category updated successfully.";
            return RedirectToPage("./Index");
        }

        private bool UserCategoryExists(long id)
        {
            return _context.UserCategories.Any(e => e.Id == id);
        }
    }
}
