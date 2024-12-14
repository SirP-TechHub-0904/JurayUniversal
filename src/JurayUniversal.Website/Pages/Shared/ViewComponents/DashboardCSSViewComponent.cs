
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Pages.Shared.ViewComponents
{
    public class DashboardCSSViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public DashboardCSSViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
             
            var supersetting = await _context.DataConfigs.FirstOrDefaultAsync();
            ViewBag.Dash = supersetting.DashboardCSS;
            return View();
        }
    }
}
