
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
    public class SliderViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public SliderViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var slider = await _context.Sliders.Where(x => x.Show == true).OrderBy(x=>x.SortOrder).ToListAsync();
            var setting = await _context.Settings.FirstOrDefaultAsync();
            var xxsetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.superx = xxsetting;
            ViewBag.setting = setting;
            var Pages = await _context.PageCategories.Include(x => x.WebPages).Where(x => x.Publish == true).OrderBy(x => x.MenuSortOrder).ToListAsync();
            ViewBag.pages = Pages;

            var singlePages = await _context.WebPages.Where(x => x.Publish == true && x.ShowInMainMenu == true).ToListAsync();
            ViewBag.singlePages = singlePages;

            var topPages = await _context.WebPages.Where(x => x.Publish == true && x.ShowInMainTop == true).ToListAsync();
            ViewBag.topPages = topPages;

            var linkExternal = await _context.WebPages.Where(x => x.Publish == true && x.ShowInMainTop == true && x.EnableDirectUrl == true).ToListAsync();
            ViewBag.linkExternal = topPages;
            var productpage = await _context.ProductCategories.Where(x => x.Publish == true).ToListAsync();
            ViewBag.productpage = productpage;

            var blogcategory = await _context.BlogCategories.Where(x => x.Publish == true).ToListAsync();
            ViewBag.blogcategory = blogcategory;
            return View(slider);
        }
    }
}
