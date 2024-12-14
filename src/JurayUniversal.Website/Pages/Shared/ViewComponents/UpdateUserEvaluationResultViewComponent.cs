using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
   public class UpdateUserEvaluationResultViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public UpdateUserEvaluationResultViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id, string userId)
        {

            var data = await _context.UserNutritionEvaluations
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.NutritionCategoryListId == id && x.UserId == userId);
            if (data != null)
            {
                if (!String.IsNullOrEmpty(data.Result))
                {
                    ViewBag.result = data.Result;
                }
                else
                {
                    ViewBag.result = "-------";
                }
            }
            else
            {
                ViewBag.result = "-------";
            }
            return View();
        }
    }
}
