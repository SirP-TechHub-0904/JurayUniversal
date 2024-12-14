using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<XProject> XProject { get; set; }
        public string UserId { get; set; }
        public async Task OnGetAsync()
        {

            string uid = _userManager.GetUserId(HttpContext.User);
            //var user = await _userManager.FindByIdAsync(userId);
            UserId = uid;
            XProject = await _context.XProjects
                .Include(x=>x.Sections).ThenInclude(x=>x.Tasks)
                .ToListAsync();
        }
    }
}
