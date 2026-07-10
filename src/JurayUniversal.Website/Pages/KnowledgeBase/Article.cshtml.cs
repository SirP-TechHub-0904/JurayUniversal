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
    public class ArticleModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ArticleModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisManualPage Article { get; set; }
        public List<MisManualElement> AccessibleElements { get; set; } = new List<MisManualElement>();

        public async Task<IActionResult> OnGetAsync(string module, long id)
        {
            Article = await _context.MisManualPages
                .Include(p => p.MisManual)
                .Include(p => p.MisManualElements)
                .FirstOrDefaultAsync(p => p.Id == id && p.MisManual.ModuleFolder.ToLower() == module.ToLower());

            if (Article == null) return NotFound();

            var isSuperAdmin = User.IsInRole("mSuperAdmin");
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            foreach (var el in Article.MisManualElements)
            {
                if (isSuperAdmin || string.IsNullOrEmpty(el.RequiredRole) || userRoles.Contains(el.RequiredRole))
                {
                    AccessibleElements.Add(el);
                }
            }

            // If the article has elements but user can't see ANY, deny access
            if (Article.MisManualElements.Any() && !AccessibleElements.Any())
            {
                return RedirectToPage("Module", new { module = module });
            }

            return Page();
        }
    }
}
