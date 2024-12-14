using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
     public class BudgetListViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public BudgetListViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var bd = await _context.BudgetLists
                .Where(x => x.BudgetSubCategoryId == id).ToListAsync();
            return View(bd);
        }
    }
}
