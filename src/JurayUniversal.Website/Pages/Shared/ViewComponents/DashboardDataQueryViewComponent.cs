using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
    public class DashboardDataQueryViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public DashboardDataQueryViewComponent(DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(long data)
        {

            if(data == 0)
            {
                ViewBag.dataX = "USER";
                return View();
            }
            else if (data == 00)
            {
                ViewBag.dataX = "CARRER";
                return View();
            }
            else if(data == 000)
            {
                ViewBag.dataX = "CONTACT US";
                return View();
            }
            else
            { 

                var datalist = await _context.CompanyProgramCategories
             .FirstOrDefaultAsync(c => c.Id == data);
                ViewBag.datalist = datalist;
                return View();
            }

               
        }
    }
}
