
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
    public class ReloaderViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public ReloaderViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string key)
        {

            var supersetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.supersetting = supersetting;
            var DataConfig = await _context.DataConfigs.FirstOrDefaultAsync();

            ViewBag.DataConfigs = DataConfig;
            return View();
        }
    }
}
