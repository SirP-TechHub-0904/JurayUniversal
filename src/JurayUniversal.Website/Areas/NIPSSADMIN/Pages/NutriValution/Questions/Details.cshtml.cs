using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution.Questions
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public NutritionCategoryList NutritionCategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NutritionCategoryList = await _context.NutritionCategoryLists
                .Include(n => n.NutritionCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (NutritionCategoryList == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
