using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Content.Pages.IPages
{
    public class PreviewModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public PreviewModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }
        public WebPage WebPage { get; set; }
        public PageSection PageSection { get; set; }
        public string Templatechoose { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id, long? secid)
        {
            var check = await _context.SuperSettings.FirstOrDefaultAsync();
            if (check == null)
            {
                return RedirectToPage("/AuthVadmin/Readonly", new { area = "V2" });
            }
            Templatechoose = check.TemplateLayoutKey;
            if (id != null)
            {


                WebPage = await _context.WebPages
                    .Include(w => w.PageCategory)
                    .Include(w => w.PageSections)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (WebPage == null)
                {
                    return NotFound();
                }
            }
            if (secid != null)
            {


                PageSection = await _context.PageSections
                    .Include(w => w.PageSectionLists)
                    .Include(w => w.WebPage)
                    .FirstOrDefaultAsync(m => m.Id == secid);

                if (PageSection == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
    }
}
