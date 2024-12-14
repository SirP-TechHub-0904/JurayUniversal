using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution.Questions
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NutritionCategoryList NutritionCategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            

            NutritionCategoryList = await _context.NutritionCategoryLists
                .Include(n => n.NutritionCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (NutritionCategoryList == null)
            {
                return NotFound();
            }
             return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
           

            _context.Attach(NutritionCategoryList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NutritionCategoryListExists(NutritionCategoryList.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/NutriValution/Category/Details", new { id = NutritionCategoryList.NutritionCategoryId });
        }

        private bool NutritionCategoryListExists(long id)
        {
            return _context.NutritionCategoryLists.Any(e => e.Id == id);
        }
    }
}
