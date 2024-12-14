
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
    public class FooterViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public FooterViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string key)
        {
            
            var Pages = await _context.WebPages.Where(x => x.Publish == true && x.ShowInFooter == true && x.SecurityPage == false && x.EnableDirectUrl == false).OrderBy(x=>x.SortOrder).ToListAsync();
            ViewBag.pages = Pages;
            var SecPages = await _context.WebPages.Where(x => x.Publish == true && x.ShowInFooter == true && x.SecurityPage == true).OrderBy(x => x.SortOrder).ToListAsync();
            ViewBag.SecPages = SecPages;
             var linkExternal = await _context.WebPages.Where(x => x.Publish == true && x.ShowInFooter == true && x.EnableDirectUrl == true).ToListAsync();
            ViewBag.linkExternal = linkExternal;
            var setting = await _context.Settings.FirstOrDefaultAsync();
            ViewBag.setting = setting;

            var supersetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.supersetting = supersetting;
            return View();
        }
    }
}
