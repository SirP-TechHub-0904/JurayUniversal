using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution.Category
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public NutritionCategory NutritionCategory { get; set; }
        [BindProperty]
        public NutritionCategoryList NutritionCategoryList { get; set; }


        public IList<NutritionCategoryList> NutritionCategoryLists { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NutritionCategory = await _context.NutritionCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (NutritionCategory == null)
            {
                return NotFound();
            }

            NutritionCategoryLists = await _context.NutritionCategoryLists
                .Include(n => n.NutritionCategory)
                .Where(x=>x.NutritionCategoryId == id)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            _context.NutritionCategoryLists.Add(NutritionCategoryList);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";

            return RedirectToPage("./Details", new {id= NutritionCategoryList.NutritionCategoryId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NutritionCategoryList = await _context.NutritionCategoryLists.FindAsync(id);

            if (NutritionCategoryList != null)
            {
                _context.NutritionCategoryLists.Remove(NutritionCategoryList);
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            }

            return RedirectToPage("./Details", new { id = NutritionCategoryList.NutritionCategoryId });
        }
    }
}
