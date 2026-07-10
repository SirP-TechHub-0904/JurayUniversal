using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.KnowledgeBase
{
    [Authorize(Roles = "mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<JurayUniversal.Domain.Models.MisManual> Manuals { get; set; } = new List<JurayUniversal.Domain.Models.MisManual>();

        public async Task<IActionResult> OnGetAsync()
        {
            Manuals = await _context.MisManuals
                .Include(m => m.MisManualPages)
                .ThenInclude(p => p.MisManualElements)
                .OrderBy(m => m.Title)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSaveManualAsync(long? id, string moduleFolder, string title, string overview)
        {
            if (id.HasValue && id.Value > 0)
            {
                var manual = await _context.MisManuals.FindAsync(id.Value);
                if (manual != null)
                {
                    manual.ModuleFolder = moduleFolder;
                    manual.Title = title;
                    manual.Overview = overview;
                    _context.Update(manual);
                }
            }
            else
            {
                var manual = new JurayUniversal.Domain.Models.MisManual
                {
                    ModuleFolder = moduleFolder,
                    Title = title,
                    Overview = overview
                };
                _context.MisManuals.Add(manual);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Manual saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSavePageAsync(long? id, long misManualId, string routeKey, string pageTitle, string pageDescription)
        {
            if (id.HasValue && id.Value > 0)
            {
                var page = await _context.MisManualPages.FindAsync(id.Value);
                if (page != null)
                {
                    page.MisManualId = misManualId;
                    page.RouteKey = routeKey;
                    page.PageTitle = pageTitle;
                    page.PageDescription = pageDescription;
                    _context.Update(page);
                }
            }
            else
            {
                var page = new MisManualPage
                {
                    MisManualId = misManualId,
                    RouteKey = routeKey,
                    PageTitle = pageTitle,
                    PageDescription = pageDescription
                };
                _context.MisManualPages.Add(page);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Page saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveElementAsync(long? id, long misManualPageId, string elementName, string elementType, string description, string requiredRole)
        {
            if (id.HasValue && id.Value > 0)
            {
                var element = await _context.MisManualElements.FindAsync(id.Value);
                if (element != null)
                {
                    element.MisManualPageId = misManualPageId;
                    element.ElementName = elementName;
                    element.ElementType = elementType;
                    element.Description = description;
                    element.RequiredRole = requiredRole;
                    _context.Update(element);
                }
            }
            else
            {
                var element = new MisManualElement
                {
                    MisManualPageId = misManualPageId,
                    ElementName = elementName,
                    ElementType = elementType,
                    Description = description,
                    RequiredRole = requiredRole
                };
                _context.MisManualElements.Add(element);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Element saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id, string type)
        {
            if (type == "Manual")
            {
                var item = await _context.MisManuals.FindAsync(id);
                if (item != null) _context.MisManuals.Remove(item);
            }
            else if (type == "Page")
            {
                var item = await _context.MisManualPages.FindAsync(id);
                if (item != null) _context.MisManualPages.Remove(item);
            }
            else if (type == "Element")
            {
                var item = await _context.MisManualElements.FindAsync(id);
                if (item != null) _context.MisManualElements.Remove(item);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = $"{type} deleted successfully.";
            return RedirectToPage();
        }
    }
}
