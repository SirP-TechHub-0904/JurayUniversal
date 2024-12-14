using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    [Authorize]
     public class NewBudgetListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public NewBudgetListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
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
             BudgetSubCategory = await _context.BudgetSubCategories
                
                .FirstOrDefaultAsync(m => m.Id == id);

            if (BudgetSubCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        [BindProperty]
        public BudgetList BudgetList { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
           
            _context.BudgetLists.Add(BudgetList);
            await _context.SaveChangesAsync();

            var maincat = await _context.BudgetSubCategories.FirstOrDefaultAsync(x => x.Id == BudgetList.BudgetSubCategoryId);
            TempData["success"] = "success";
            return RedirectToPage("./Details", new { id = maincat.BudgetMainCategoryId });
        }
    }
}
