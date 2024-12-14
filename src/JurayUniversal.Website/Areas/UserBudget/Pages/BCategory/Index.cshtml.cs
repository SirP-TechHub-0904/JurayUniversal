using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BCategory
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<BudgetSubCategory> BudgetSubCategory { get;set; }

        public async Task OnGetAsync()
        {
            BudgetSubCategory = await _context.BudgetSubCategories
                .Include(b => b.BudgetMainCategory).ToListAsync();
        }
    }
}
