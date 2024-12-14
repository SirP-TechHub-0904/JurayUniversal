using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
     public class XProjectFileViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public XProjectFileViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var xtaskdata = await _context.XProjectFiles
                .Where(x => x.XProjectId == id).ToListAsync();
            return View(xtaskdata);
        }
    }
}
