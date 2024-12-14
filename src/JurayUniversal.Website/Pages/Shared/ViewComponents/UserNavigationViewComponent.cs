using Amazon.S3;
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
    public class UserNavigationViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;
         
        public UserNavigationViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

 
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var xsetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.xsetting = xsetting;
            var setting = await _context.Settings.FirstOrDefaultAsync();

            var dataitem = await _context.DashboardSettings.FirstOrDefaultAsync();
            if(dataitem  != null) { 
            ViewBag.dataitem = dataitem.UserCustomDashboard;

            }
            var dashmenu = await _context.CompanyProgramCategories.Where(x => x.ShowInMenu == true && x.Publish == true).ToListAsync();
            ViewBag.Cmp = dashmenu;

            var userAccountCategory = await _context.UserCategories.Where(x => x.ShowInMenu == true && x.Publish == true).ToListAsync();
            ViewBag.userAccountCategory = userAccountCategory;

            return View(dataitem);
        }
    }
}
