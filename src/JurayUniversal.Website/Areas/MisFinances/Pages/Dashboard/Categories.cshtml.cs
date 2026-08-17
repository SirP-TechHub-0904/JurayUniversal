using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class CategoriesModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CategoriesModel(DashboardDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<MisFinanceCategory> Categories { get; set; }
        public SuperSetting SuperSetting { get; set; }

        public async Task OnGetAsync()
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Categories = await _context.MisFinanceCategories
                .Include(c => c.MisGroup)
                .Include(c => c.UserCategory)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            // Auto-assign keys for legacy records if missing
            bool modified = false;
            foreach (var cat in Categories.Where(c => string.IsNullOrEmpty(c.Key)))
            {
                cat.Key = "CAT-" + System.Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                _context.Entry(cat).State = EntityState.Modified;
                modified = true;
            }
            if (modified)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
