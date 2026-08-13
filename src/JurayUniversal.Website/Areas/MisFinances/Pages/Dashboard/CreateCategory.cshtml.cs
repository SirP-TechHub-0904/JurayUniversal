using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
            Category = new MisFinanceCategory
            {
                IsCompulsory = true,
                Frequency = PaymentFrequency.OneTime,
                IsActive = true
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Category.Transactions");
            if (string.IsNullOrWhiteSpace(Category.Title))
            {
                ModelState.AddModelError("Category.Title", "Category Title is required.");
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
    }
}
