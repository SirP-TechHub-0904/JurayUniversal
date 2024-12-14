using JurayUniversal.Application.Dtos;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
    public class DashboardDataViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public DashboardDataViewComponent(DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(long data, string color)
        {
            var datalist = await _context.CompanyProgramCategories
                .Include(x=>x.CompanyPrograms)
                .FirstOrDefaultAsync(x=>x.ShowInDashboard == true && x.Id == data);

            ViewBag.Color = color;
            return View(datalist);
        }
    }
}
