using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public BudgetMainCategory BudgetMainCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string uid = _userManager.GetUserId(HttpContext.User);
            BudgetMainCategory = await _context.BudgetMainCategories
                .Include(b => b.User)
                .Include(x=>x.SubCategories).ThenInclude(x=>x.BudgetList)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == uid);

            if (BudgetMainCategory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
