using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
    public class BudgetDollarSumViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public BudgetDollarSumViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            decimal sumx = 0;

            var bd = await _context.BudgetSubCategories.Where(x => x.BudgetMainCategoryId == id && x.Show == true).ToListAsync();
            foreach (var data in bd)
            {
                var list = await _context.BudgetLists.Where(x => x.BudgetSubCategoryId == data.Id && x.Show == true).SumAsync(x => x.AmountInDollar);
                sumx += list;
            };

            ViewBag.sumdollar = sumx;

            return View();
        }
    }
}
