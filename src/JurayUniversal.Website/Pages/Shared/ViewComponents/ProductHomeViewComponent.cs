
using JurayUniversal.Domain.Models;
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
    public class ProductHomeViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public ProductHomeViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }


        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(string key)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            var suppersetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.setting = setting;
            ViewBag.suppersetting = suppersetting;
            

            var page = await _context.Products.Include(x=>x.ProductFeatures).Where(x => x.ShowInHome == true).OrderBy(x => x.SortOrder).ToListAsync();

            page = page.Take(6).ToList();

            return View(page);
        }
    }
}
