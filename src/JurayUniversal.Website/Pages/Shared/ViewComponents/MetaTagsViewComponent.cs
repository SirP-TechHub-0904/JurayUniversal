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
    public class MetaTagsViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;
         
        public MetaTagsViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        

        public async Task<IViewComponentResult> InvokeAsync()
        {

            //var slider = await _context.Sliders.Where(x => x.Show == true).OrderBy(x=>x.SortOrder).ToListAsync();
            return View();
        }
    }
}
