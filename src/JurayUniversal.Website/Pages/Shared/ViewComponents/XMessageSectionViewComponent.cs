using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
      public class XMessageSectionViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public XMessageSectionViewComponent(
            DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var xtaskdata = await _context.XProjectMessages
               .Include(x => x.Sender)
               .Include(x => x.Receiver)
               .Include(x => x.XProject).Where(x => x.XProjectId == id).ToListAsync();
            ViewBag.Id = id;

            string userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.userid = userId;

            return View(xtaskdata);
        }
    }
}
