using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JurayUniversal.Website.Pages
{
    public class ProcessingModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
        private readonly UserManager<Profile> _userManager;

        public ProcessingModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, UserManager<Profile> userManager)
        {
            _context = context;
            _settingsService = settingsService;
            _userManager = userManager;
        }
        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }
        public ProfilePortfolioSetting ProfilePortfolioSetting { get; set; }
        public async Task<IActionResult> OnGetAsync(string data, string key, string query)
        {
            var httpContext = HttpContext;
            VerificationWebDto setting = await _settingsService.ValidateWeb(httpContext);
            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }
            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);

            }

            SuperSetting = setting.SuperSetting;
            var user = await _userManager.FindByEmailAsync(data);
            if (user != null)
            {
                ProfilePortfolioSetting = await _context.ProfilePortfolioSetting.Include(x=>x.Profile).FirstOrDefaultAsync(x => x.ProfileId == user.Id);
                if (ProfilePortfolioSetting == null)
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
            return Page();
        }
    }
}
