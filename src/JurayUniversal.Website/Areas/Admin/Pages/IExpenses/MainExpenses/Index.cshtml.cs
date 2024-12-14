using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Areas.Admin.Pages.IExpenses.MainExpenses
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,CFO,CEO")]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<CompayExpenses> CompayExpenses { get;set; }

        public async Task OnGetAsync()
        {
            CompayExpenses = await _context.CompayExpenses
                .Include(c => c.ExpensesCategory)
                .Include(c => c.User).ToListAsync();
        }
    }
}
