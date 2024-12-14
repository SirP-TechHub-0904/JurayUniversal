using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
      [Authorize]
    public class BudgetModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public BudgetModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
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
            var getuser = await _userManager.FindByIdAsync(uid);
            if(getuser != null) { 
            BudgetMainCategory = await _context.BudgetMainCategories
                .Include(b => b.User)
                .Include(x => x.SubCategories).ThenInclude(x => x.BudgetList)
                .FirstOrDefaultAsync(m => m.Id == id && m.AccessEmails.Contains(getuser.Email) || m.UserId == uid);

            if (BudgetMainCategory == null)
            {
                return NotFound();
            }
            }
            return Page();
        }
    }
}
