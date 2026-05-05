using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages
{
    public class PaymentReceiptModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;

        public PaymentReceiptModel(
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context,
            ISettingsService settingsService)
        {
            _context = context;
            _settingsService = settingsService;
        }

        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }
        public Payment Payment { get; set; }

        [BindProperty(SupportsGet = true)]
        public long? Id { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
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

            if (id == null)
            {
                TempData["error"] = "Invalid payment receipt.";
                return RedirectToPage("/Payments");
            }

            Payment = await _context.Payments
                .Include(x => x.PaymentCategory)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Payment == null)
            {
                TempData["error"] = "Payment receipt not found.";
                return RedirectToPage("/Payments");
            }

            return Page();
        }
    }
}