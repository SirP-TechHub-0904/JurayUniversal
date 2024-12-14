using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
      [Authorize]
     public class UpdateBudgetListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UpdateBudgetListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public BudgetSubCategory BudgetSubCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            BudgetList = await _context.BudgetLists

               .FirstOrDefaultAsync(m => m.Id == id);

            if (BudgetList == null)
            {
                return NotFound();
            }

            BudgetSubCategory = await _context.BudgetSubCategories

              .FirstOrDefaultAsync(m => m.Id == BudgetList.BudgetSubCategoryId);

            return Page();
        }

        [BindProperty]
        public BudgetList BudgetList { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(BudgetList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    throw;
                
            }

            var maincat = await _context.BudgetSubCategories.FirstOrDefaultAsync(x=>x.Id == BudgetList.BudgetSubCategoryId);
            TempData["success"] = "success";
            return RedirectToPage("./Details", new { id = maincat.BudgetMainCategoryId });
        }
    }
}
