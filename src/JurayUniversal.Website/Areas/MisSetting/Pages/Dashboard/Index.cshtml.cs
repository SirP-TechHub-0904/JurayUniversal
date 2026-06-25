using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisSetting.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Domain.Models.MisSetting Setting { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Setting = await _context.MisSettings.FirstOrDefaultAsync();
            if (Setting == null)
            {
                Setting = new Domain.Models.MisSetting { CategoryAlias = "Category", SubCategoryAlias = "Sub-Category" };
                _context.MisSettings.Add(Setting);
                await _context.SaveChangesAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existing = await _context.MisSettings.FirstOrDefaultAsync();
            if (existing != null)
            {
                // Entity is already tracked by EF — just update the properties directly
                existing.CategoryAlias = Setting.CategoryAlias;
                existing.SubCategoryAlias = Setting.SubCategoryAlias;
                existing.EnableListDownload = Setting.EnableListDownload;
            }
            else
            {
                _context.MisSettings.Add(Setting);
            }

            await _context.SaveChangesAsync();

            StatusMessage = "MIS Settings successfully updated.";
            return RedirectToPage();
        }
    }
}
