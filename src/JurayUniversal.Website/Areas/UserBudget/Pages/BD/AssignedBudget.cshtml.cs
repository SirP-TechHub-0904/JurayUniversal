using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    [Authorize]
    public class AssignedBudgetModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public AssignedBudgetModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<BudgetMainCategory> BudgetMainCategory { get; set; }
        //public IList<BudgetMainCategory> BudgetMainCategoryList { get;set; }

        public async Task OnGetAsync()
        {
            string uid = _userManager.GetUserId(HttpContext.User);
            
            var user = await _userManager.FindByIdAsync(uid);
            BudgetMainCategory = await _context.BudgetMainCategories
                .Include(b => b.User)
                .Where(x => x.AccessEmails.ToLower().Contains(user.Email.ToLower())).ToListAsync();

        }
    }
}
