
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Pages.Shared.ViewComponents
{
    public class PageSectionViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public PageSectionViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string? position, string? pageset, long? id, long? secid, int? takex)
        {
            if (takex == 3)
            {
                ViewBag.take3 = takex;
            }
            else
            {
                ViewBag.take3 = 300;
            }
            var setting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.themedata = setting.TemplateLayoutKey;
            ViewBag.pageset = pageset;
            var page = _context.PageSections.Include(x => x.PageSectionLists).Where(x=>x.Disable == false).AsQueryable();
            if (!String.IsNullOrEmpty(position) && !String.IsNullOrEmpty(pageset))
            {
                if (position.ToLower() == "top" && pageset.ToLower() == "home")
                {
                    page = page.Where(x => x.ShowInHome == true && x.HomeSortFrom == HomeSortFrom.Top).OrderBy(x => x.HomePageSortOrder).AsQueryable();
                    return View(await page.ToListAsync());
                }
            }
            if (!String.IsNullOrEmpty(position))
            {
                if (position.ToLower() == "fixed")
                {
                    page = page.Where(x => x.FixedAfterFooter == true).OrderBy(x => x.HomePageSortOrder).AsQueryable();
                    return View(await page.ToListAsync());
                }
            }
            if (!String.IsNullOrEmpty(position) && !String.IsNullOrEmpty(pageset))
            {
                if (position.ToLower() == "bottom" && pageset.ToLower() == "home")
                {
                    page = page.Where(x => x.ShowInHome == true && x.HomeSortFrom == HomeSortFrom.Bottom).OrderBy(x => x.HomePageSortOrder).AsQueryable();
                    return View(await page.ToListAsync());
                }
            }

            if (String.IsNullOrEmpty(position) && id != null)
            {
                if (id > 0)
                {
                    page = page.Where(x => x.Disable == false && x.WebPageId == id).OrderBy(x => x.PageSortOrder).AsQueryable();
                    return View(await page.ToListAsync());
                }
            }
            if (String.IsNullOrEmpty(position) && secid != null)
            {
                if (secid > 0)
                {
                    page = page.Where(x => x.Disable == false && x.Id == secid).OrderBy(x => x.PageSortOrder).AsQueryable();
                    return View(await page.ToListAsync());
                }
            }
            if (!String.IsNullOrEmpty(position))
            {
                if (position.ToLower() == "footer")
                {
                    page = page.Where(x => x.FixedAfterFooter == true).OrderBy(x => x.HomePageSortOrder).AsQueryable();
                    return View(await page.ToListAsync());
                }
            }
            
            
            return View(await page.Where(x => x.WebPageId == 000).ToListAsync());
        }
    }
}
