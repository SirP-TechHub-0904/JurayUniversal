using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Pages
{
    public class ProfileModel : PageModel
    {
          private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
        private readonly UserManager<Profile> _userManager;

        public ProfileModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, UserManager<Profile> userManager)
        {
            _context = context;
            _settingsService = settingsService;
            _userManager = userManager;
        }
        public ProfilePortfolioSetting ProfilePortfolioSetting { get; set; }
        public async Task<IActionResult> OnGetAsync(string data, string key, string query)
        {
            var httpContext = HttpContext;
            VerificationWebDto setting = await _settingsService.ProfileData(httpContext);
             if(setting.PortfolioPath == "///")
            {
                return NotFound();
            }
            ProfilePortfolioSetting = setting.ProfilePortfolioSetting;

            return Page();
        }
   
    }
}
