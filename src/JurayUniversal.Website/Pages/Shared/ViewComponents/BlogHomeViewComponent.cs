
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
    public class BlogHomeViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public BlogHomeViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var blog = await _context.Blogs.Where(x => x.Publish == true).OrderByDescending(x=>x.Date).ThenBy(x=>x.SortOrder).Take(10).ToListAsync();
            var setting = await _context.Settings.FirstOrDefaultAsync();
            var suppersetting = await _context.SuperSettings.FirstOrDefaultAsync();
            ViewBag.sxt = setting;
            ViewBag.suppersetting = suppersetting;
            return View(blog);
        }
    }
    
}
