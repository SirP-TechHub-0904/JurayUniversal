using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.IPortfolio.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        DashboardDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly ISettingsService _setting;
        private readonly UserManager<Profile> _userManager;
        public IndexModel(IConfiguration configuration, DashboardDbContext context, UserManager<Profile> userManager, ILogger<IndexModel> logger, ISettingsService setting)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _setting = setting;
        }
       public Profile Profile { get; set; }
        public async Task<IActionResult> OnGetAsync(string? token)
        {
            var httpContext = HttpContext;
            PortfolioDto result = await _setting.GetPortfolioAccount(token, httpContext);
            if (result == null)
            {
                return NotFound();
            }
            if (result.result == false)
            {
                return NotFound();
            }
            if (result.FirstTime == true)
            {
                return RedirectToPage("./Setup", new {token = token});
            }
            if (result.IsAuthenticated == true)
            {
                return RedirectToPage("./Dashboard");
            }
            else
            {
                return Page();
            }


        }
    }
}
