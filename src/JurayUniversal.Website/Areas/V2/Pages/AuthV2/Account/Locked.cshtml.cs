using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace JurayUniversal.Website.Areas.V2.Pages.AuthV2.Account
{
    [Authorize]
    public class LockedModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
         private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public LockedModel(SignInManager<Profile> signInManager,
            
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
             
            _context = context;
        }

        [BindProperty]
        public SuperSetting SuperSetting { get; set; }
        [BindProperty]
        public Setting Setting { get; set; }

        [BindProperty]
        public string Name { get; set; }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Setting = await _context.Settings.FirstOrDefaultAsync();

            if (SuperSetting == null)
            {
                return RedirectToPage("/AuthVadmin/Readonly", new { area = "V2" });
            }

            if (SuperSetting.ActivateLogin == false && SuperSetting.ActivateLoginInMenu == false)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            Name = user.FirstName;

            // Clear the existing external cookie to ensure a clean login process
           
            return Page();
        }
    }
}
