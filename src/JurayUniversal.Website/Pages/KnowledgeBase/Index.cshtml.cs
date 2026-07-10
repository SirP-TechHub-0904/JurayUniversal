using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Pages.KnowledgeBase
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public List<MisManual> Manuals { get; set; } = new List<MisManual>();

        public async Task OnGetAsync()
        {
            var isSuperAdmin = User.IsInRole("mSuperAdmin");
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var allManuals = await _context.MisManuals
                .Include(m => m.MisManualPages)
                    .ThenInclude(p => p.MisManualElements)
                .OrderBy(m => m.Title)
                .ToListAsync();

            // Filter manuals based on whether the user has access to at least one element
            foreach (var manual in allManuals)
            {
                bool hasAccess = false;

                if (!manual.MisManualPages.Any()) 
                    continue; // Skip empty manuals

                foreach (var page in manual.MisManualPages)
                {
                    if (page.MisManualElements == null || !page.MisManualElements.Any())
                    {
                        hasAccess = true; // Page with no elements is considered public
                        break;
                    }

                    foreach (var el in page.MisManualElements)
                    {
                        if (isSuperAdmin || string.IsNullOrEmpty(el.RequiredRole) || userRoles.Contains(el.RequiredRole))
                        {
                            hasAccess = true;
                            break;
                        }
                    }
                    if (hasAccess) break;
                }

                if (hasAccess)
                {
                    Manuals.Add(manual);
                }
            }
        }
    }
}
