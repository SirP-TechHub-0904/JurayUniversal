
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
    public class BreadcrumOthersViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public BreadcrumOthersViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string data)
        { 
             var Pages = await _context.PageCategories.Include(x=>x.WebPages).Where(x => x.Publish == true).OrderBy(x=>x.MenuSortOrder).ToListAsync();
            ViewBag.pages = Pages;

            var singlePages = await _context.WebPages.Where(x => x.Publish == true && x.ShowInMainMenu == true).ToListAsync();
            ViewBag.singlePages = singlePages;

            var topPages = await _context.WebPages.Where(x => x.Publish == true && x.ShowInMainTop == true).ToListAsync();
            ViewBag.topPages = topPages;

            var linkExternal = await _context.WebPages.Where(x => x.Publish == true && x.ShowInMainMenu == true && x.EnableDirectUrl == true).ToListAsync();
            ViewBag.linkExternal = linkExternal;
            var productpage = await _context.ProductCategories.Include(x=>x.Product).Where(x => x.Publish == true).ToListAsync();
            ViewBag.productpage = productpage;
            var setting = await _context.Settings.FirstOrDefaultAsync();
            ViewBag.setting = setting;

            var supersetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.supersetting = supersetting;
            ViewBag.datataitle = data;
            var blogcategory = await _context.BlogCategories.Where(x => x.Publish == true).ToListAsync();
            ViewBag.blogcategory = blogcategory;
            return View();
        }
    }
}
