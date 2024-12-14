using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Management.Pages
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public SuperSetting SuperSetting { get; set; }
        public DataConfig DataConfig { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            DataConfig = await _context.DataConfigs.FirstOrDefaultAsync();


            var usersadmin = await _userManager.Users.Where(x => x.Email == "universal@platform.io" || x.Email == "universal@j.io" || x.Email == "dashboard@platform.io" || x.Email == "admin@platform.io").ToListAsync();
            foreach (var user in usersadmin)
            {
                var xuser = await _userManager.FindByIdAsync(user.Id);
                user.UserStatus = Domain.Enum.Enum.UserStatus.Active;
                await _userManager.UpdateAsync(xuser);
            }
            return Page();
        }
    }
}
