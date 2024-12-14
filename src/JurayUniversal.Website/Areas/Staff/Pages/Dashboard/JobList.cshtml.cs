using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
     [Microsoft.AspNetCore.Authorization.Authorize]

    public class JobListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public JobListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IList<Job> Job { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var superrole = await _userManager.IsInRoleAsync(user, "mSuperAdmin");
            var adminrole = await _userManager.IsInRoleAsync(user, "Admin");
            var staffrole = await _userManager.IsInRoleAsync(user, "Manager");

            if (superrole.Equals(true) || adminrole.Equals(true) || staffrole.Equals(true))
            {
                Job = await _context.Jobs
                    .Include(p => p.Profile).ToListAsync();
            }
            else
            {
                Job = await _context.Jobs
                    .Include(p => p.Profile).Where(x => x.ProfileId == user.Id).ToListAsync();
            }

        }

    }
}
