using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class CreateCategoryModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateCategoryModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisFinanceCategory Category { get; set; } = new MisFinanceCategory();

        public SelectList GroupSelectList { get; set; }
        public SelectList UserCategorySelectList { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            Category = new MisFinanceCategory
            {
                IsCompulsory = true,
                Frequency = PaymentFrequency.OneTime,
                IsActive = true
            };

            await PopulateDropdownsAsync();
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

            Category.CreatedAt = DateTime.UtcNow;
            if (string.IsNullOrEmpty(Category.Key))
            {
                Category.Key = "CAT-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            }

            _context.MisFinanceCategories.Add(Category);
            await _context.SaveChangesAsync();

            StatusMessage = $"Payment category \"{Category.Title}\" created successfully.";
            return RedirectToPage("./Categories");
        }

        private async Task PopulateDropdownsAsync()
        {
            var groups = await _context.MisGroups
                .Where(g => g.IsActive)
                .OrderBy(g => g.Title)
                .ToListAsync();

            GroupSelectList = new SelectList(groups, "Id", "Title", Category.MisGroupId);

            var userCategories = await _context.UserCategories
                .OrderBy(c => c.Title)
                .ToListAsync();

            UserCategorySelectList = new SelectList(userCategories, "Id", "Title", Category.UserCategoryId);
        }
    }
}
