using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    [Authorize]
    public class NewCategoryModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public NewCategoryModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BudgetMainCategory = await _context.BudgetMainCategories
               .Include(b => b.User).FirstOrDefaultAsync(m => m.Id == id);

            if (BudgetMainCategory == null)
            {
                return NotFound();
            }

            return Page();
        }

        [BindProperty]
        public BudgetSubCategory BudgetSubCategory { get; set; }
        public BudgetMainCategory BudgetMainCategory { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
           

            _context.BudgetSubCategories.Add(BudgetSubCategory);
            await _context.SaveChangesAsync();
            TempData["success"] = "success";
            return RedirectToPage("./Details", new { id = BudgetSubCategory.Id });
        }
    }
}
