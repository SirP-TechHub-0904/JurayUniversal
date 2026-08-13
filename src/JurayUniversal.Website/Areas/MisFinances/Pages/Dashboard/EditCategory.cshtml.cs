using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class EditCategoryModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditCategoryModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisFinanceCategory Category { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Category = await _context.MisFinanceCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Category.Transactions");
            if (string.IsNullOrWhiteSpace(Category.Title))
            {
                ModelState.AddModelError("Category.Title", "Category Title is required.");
                return Page();
            }

            var categoryToUpdate = await _context.MisFinanceCategories.FindAsync(Category.Id);
            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            categoryToUpdate.Title = Category.Title;
            categoryToUpdate.Description = Category.Description;
            categoryToUpdate.IsCompulsory = Category.IsCompulsory;
            categoryToUpdate.AllowGuestPayment = Category.AllowGuestPayment;
            categoryToUpdate.Frequency = Category.Frequency;
            categoryToUpdate.FixedAmount = Category.FixedAmount;
            categoryToUpdate.IsActive = Category.IsActive;

            await _context.SaveChangesAsync();

            StatusMessage = $"Payment category \"{Category.Title}\" updated successfully.";
            return RedirectToPage("./Categories");
        }
    }
}
