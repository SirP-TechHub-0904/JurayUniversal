using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages
{
    public class PaymentsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;

        public PaymentsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService)
        {
            _context = context;
            _settingsService = settingsService;
        }

        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }

        public IList<PaymentCategory> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Setting = await _context.Settings.FirstOrDefaultAsync();

            var httpContext = HttpContext;
            var setting = await _settingsService.ValidateWeb(httpContext);

            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }

            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);
            }

            SuperSetting = setting.SuperSetting;

            if (Year == null)
            {
                Year = DateTime.UtcNow.Year;
            }

            Categories = await _context.PaymentCategories
                .Where(x => x.Year == Year)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Page();
        }
    }
}