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

        public string CategoryAlias { get; set; } = "Category";

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor") || roles.Contains("UserManagement_Readonly");
            
            if (!hasAccess)
            {
                return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            }

            var misSetting = await _context.MisSettings.FirstOrDefaultAsync();
            if (misSetting != null && !string.IsNullOrEmpty(misSetting.CategoryAlias))
            {
                CategoryAlias = misSetting.CategoryAlias;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDataAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var canEdit = await _misAuth.CanEditAsync(User, "MisUserManagement");

            var dtParams = JurayUniversal.Website.Helpers.DataTablesHelper.GetParameters(Request);
            var searchValue = dtParams.SearchValue;
            var sortColumnDirection = dtParams.SortColumnDirection;
            var sortColumnIndex = dtParams.SortColumnIndex;
            int pageSize = dtParams.Length;
            int skip = dtParams.Start;

            var query = _context.UserCategories.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();
                query = query.Where(u => u.Title.ToLower().Contains(searchValue) || 
                                         (u.Description != null && u.Description.ToLower().Contains(searchValue)));
            }

            // Simple sorting
            if (sortColumnDirection == "desc")
            {
                query = query.OrderByDescending(c => c.Title);
            }
            else
            {
                query = query.OrderBy(c => c.Title);
            }

            var recordsTotal = await _context.UserCategories.CountAsync();
            var recordsFiltered = await query.CountAsync();

            List<UserCategory> categories;
            if (pageSize > 0)
            {
                categories = await query.Skip(skip).Take(pageSize).ToListAsync();
            }
            else
            {
                categories = await query.Skip(skip).ToListAsync();
            }

            var data = categories.Select((c, index) => new
            {
                sn = skip + index + 1,
                title = c.Title,
                description = c.Description ?? "",
                publish = c.Publish ? "<span class=\"badge bg-success\">Yes</span>" : "<span class=\"badge bg-secondary\">No</span>",
                showinmenu = c.ShowInMenu ? "<span class=\"badge bg-info\">Yes</span>" : "<span class=\"badge bg-secondary\">No</span>",
                showindashboard = c.ShowInDashboard ? "<span class=\"badge bg-primary\">Yes</span>" : "<span class=\"badge bg-secondary\">No</span>",
                manage = $"<div class=\"dropdown\"><button class=\"btn btn-light btn-sm shadow-none border dropdown-toggle fw-bold text-dark\" type=\"button\" data-bs-toggle=\"dropdown\" aria-expanded=\"false\">Manage</button><ul class=\"dropdown-menu dropdown-menu-end shadow border-0 rounded-3\"><li><a class=\"dropdown-item py-2\" href=\"/MisUserManagement/Dashboard/List?CategoryId={c.Id}\"><i class=\"bi bi-people me-2 text-success\"></i> View Users</a></li>" + 
                         (canEdit ? $"<li><a class=\"dropdown-item py-2\" href=\"/MisUserManagement/Dashboard/Categories/Edit?id={c.Id}\"><i class=\"bi bi-pencil me-2 text-primary\"></i> Edit</a></li><li><hr class=\"dropdown-divider\"></li><li><a class=\"dropdown-item py-2 text-danger\" href=\"/MisUserManagement/Dashboard/Categories/Delete?id={c.Id}\"><i class=\"bi bi-trash me-2\"></i> Delete</a></li>" : "") + 
                         "</ul></div>"
            });

            return new JsonResult(new
            {
                draw = dtParams.Draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = data
            });
        }
    }
}
