using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
        private readonly LoggerLibrary _logger;
        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, LoggerLibrary logger)
        {
            _context = context;
            _settingsService = settingsService;
            _logger = logger;
        }
        public string Templatechoose { get; set; }
        public SuperSetting SuperSetting { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var DataConfig = await _context.DataConfigs.FirstOrDefaultAsync();
            if(DataConfig == null)
            {
                return RedirectToPage("/V3/Create", new { area = "Management" });
            }

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
            // Log TempData value
            //_logger.Log($"the is a logged file");

            Templatechoose = setting.SuperSetting.TemplateLayoutKey;
            SuperSetting = setting.SuperSetting;
            return Page();
        }
    }
}