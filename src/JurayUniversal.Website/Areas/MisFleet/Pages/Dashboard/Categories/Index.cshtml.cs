using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFleet.Pages.Dashboard.Categories
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisEquipmentCategory> Categories { get; set; } = new List<MisEquipmentCategory>();

        [BindProperty]
        public MisEquipmentCategory NewCategory { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.MisEquipmentCategories
                .Include(c => c.MisEquipments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NewCategory.Name))
            {
                TempData["error"] = "Category name is required.";
                return RedirectToPage();
            }

            _context.MisEquipmentCategories.Add(NewCategory);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category created successfully!";
            return RedirectToPage();
        }
    }
}
