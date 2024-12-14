using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Content.Pages.IPages
{
    public class PreviewAllModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public PreviewAllModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }
        public WebPage WebPage { get; set; }
        public IList<PageSection> PageSections { get; set; }
        public string Templatechoose { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var check = await _context.SuperSettings.FirstOrDefaultAsync();
            if (check == null)
            {
                return RedirectToPage("/AuthVadmin/Readonly", new { area = "V2" });
            }
            Templatechoose = check.TemplateLayoutKey;


            PageSections = await _context.PageSections
                .Include(w => w.PageSectionLists)
                .Include(w => w.WebPage)
                .OrderBy(x=>x.WebPage.Id).ToListAsync();
              
            return Page();
        }
    }
}
