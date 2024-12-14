using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{

    [Authorize]
    public class ListDetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ListDetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public BudgetList BudgetList { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BudgetList = await _context.BudgetLists
                .Include(b => b.BudgetSubCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (BudgetList == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
