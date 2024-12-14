using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
     public class BudgetPreviewViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public BudgetPreviewViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var bd = await _context.BudgetSubCategories
                .Include(x => x.BudgetList)
                .Where(x => x.BudgetMainCategoryId == id && x.Show == true).ToListAsync();
            ViewBag.mId = id;
            return View(bd);
        }
    }
}
