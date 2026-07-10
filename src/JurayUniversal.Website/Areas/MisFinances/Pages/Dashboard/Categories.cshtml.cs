using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class CategoriesModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CategoriesModel(DashboardDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<MisFinanceCategory> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.MisFinanceCategories
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync(
            string Title, string Description, bool IsCompulsory, PaymentFrequency Frequency, decimal? FixedAmount)
        {
            var category = new MisFinanceCategory
            {
                Title = Title,
                Description = Description,
                IsCompulsory = IsCompulsory,
                Frequency = Frequency,
                FixedAmount = FixedAmount,
                IsActive = true
            };
            _context.MisFinanceCategories.Add(category);
            await _context.SaveChangesAsync();
            StatusMessage = $"Payment category \"{Title}\" created successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(
            long Id, string Title, string Description, bool IsCompulsory, PaymentFrequency Frequency, decimal? FixedAmount, bool IsActive)
        {
            var category = await _context.MisFinanceCategories.FindAsync(Id);
            if (category == null) return NotFound();

            category.Title = Title;
            category.Description = Description;
            category.IsCompulsory = IsCompulsory;
            category.Frequency = Frequency;
            category.FixedAmount = FixedAmount;
            category.IsActive = IsActive;

            await _context.SaveChangesAsync();
            StatusMessage = $"Payment category \"{Title}\" updated successfully.";
            return RedirectToPage();
        }
    }
}
