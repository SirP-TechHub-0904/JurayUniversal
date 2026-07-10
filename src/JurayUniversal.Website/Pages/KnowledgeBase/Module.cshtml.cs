using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Pages.KnowledgeBase
{
    [AllowAnonymous]
    public class ModuleModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ModuleModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisManual Manual { get; set; }
        public List<MisManualPage> AccessiblePages { get; set; } = new List<MisManualPage>();

        public async Task<IActionResult> OnGetAsync(string module)
        {
            if (string.IsNullOrEmpty(module)) return RedirectToPage("Index");

            Manual = await _context.MisManuals
                .Include(m => m.MisManualPages)
                    .ThenInclude(p => p.MisManualElements)
                .FirstOrDefaultAsync(m => m.ModuleFolder.ToLower() == module.ToLower());

            if (Manual == null) return NotFound();

            var isSuperAdmin = User.IsInRole("mSuperAdmin");
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            foreach (var page in Manual.MisManualPages.OrderBy(p => p.PageTitle))
            {
                if (page.MisManualElements == null || !page.MisManualElements.Any())
                {
                    AccessiblePages.Add(page);
                    continue;
                }

                bool hasAccess = false;
                foreach (var el in page.MisManualElements)
                {
                    if (isSuperAdmin || string.IsNullOrEmpty(el.RequiredRole) || userRoles.Contains(el.RequiredRole))
                    {
                        hasAccess = true;
                        break;
                    }
                }

                if (hasAccess)
                {
                    AccessiblePages.Add(page);
                }
            }

            // If user has no access to any pages here, they shouldn't see the module
            if (!AccessiblePages.Any())
                return RedirectToPage("Index");

            return Page();
        }
    }
}
