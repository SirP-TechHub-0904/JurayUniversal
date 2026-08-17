using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public SelectList GroupSelectList { get; set; }
        public SelectList UserCategorySelectList { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Category = await _context.MisFinanceCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            await PopulateDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Category.Transactions");
            ModelState.Remove("Category.MisGroup");
            ModelState.Remove("Category.UserCategory");

            if (string.IsNullOrWhiteSpace(Category.Title))
            {
                ModelState.AddModelError("Category.Title", "Category Title is required.");
                await PopulateDropdownsAsync();
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
            categoryToUpdate.MisGroupId = Category.MisGroupId;
            categoryToUpdate.UserCategoryId = Category.UserCategoryId;

            await _context.SaveChangesAsync();

            StatusMessage = $"Payment category \"{Category.Title}\" updated successfully.";
            return RedirectToPage("./Categories");
        }

        private async Task PopulateDropdownsAsync()
        {
            var groups = await _context.MisGroups
                .Where(g => g.IsActive)
                .OrderBy(g => g.Title)
                .ToListAsync();

            GroupSelectList = new SelectList(groups, "Id", "Title", Category?.MisGroupId);

            var userCategories = await _context.UserCategories
                .OrderBy(c => c.Title)
                .ToListAsync();

            UserCategorySelectList = new SelectList(userCategories, "Id", "Title", Category?.UserCategoryId);
        }
    }
}
