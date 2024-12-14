using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
     public class XTaskViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public XTaskViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
             var xtaskdata = await _context.XProjectTasks
                .Include(x=>x.User)
                .Include(x=>x.TestedBy)
                .Include(x=>x.ApprovedBy).Where(x=>x.XProjectSectionId == id).ToListAsync();
            return View(xtaskdata);
        }
    }

}
