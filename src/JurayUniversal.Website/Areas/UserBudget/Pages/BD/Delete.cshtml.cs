using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    public class DeleteModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DeleteModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BudgetMainCategory BudgetMainCategory { get; set; }

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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BudgetMainCategory = await _context.BudgetMainCategories.FindAsync(id);

            if (BudgetMainCategory != null)
            {
                _context.BudgetMainCategories.Remove(BudgetMainCategory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
