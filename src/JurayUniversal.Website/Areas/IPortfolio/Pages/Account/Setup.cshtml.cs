using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.IPortfolio.Pages.Account
{
    public class SetupModel : PageModel
    {
        private readonly IConfiguration _configuration;
        DashboardDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly ISettingsService _setting;
        private readonly UserManager<Profile> _userManager;
        public SetupModel(IConfiguration configuration, DashboardDbContext context, UserManager<Profile> userManager, ILogger<IndexModel> logger, ISettingsService setting)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _setting = setting;
        }
        public Profile Profile { get; set; }
        public IList<PortfolioTemplate> PortfolioTemplates { get; set; }
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
                PortfolioTemplates = await _context.PortfolioTemplates.ToListAsync();
                Profile = result.Profile;
                return Page();
            }

            else
            {
                return NotFound();
            }


        }
         
        //[BindProperty]
        //public long Idx { get; set; }
        public async Task<IActionResult> OnPostAsync(long Idx)
        {

            var context = HttpContext;
            var host = context.Request.Host;
            var check = await _context.ProfilePortfolioSetting
                        .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host.ToString() || !x.UseUpgradeWebLink && x.DefaultWebLink == host.ToString());
            if (check != null)
            {
                check.PortfolioTemplateId = Idx;
                _context.Attach(check).State = EntityState.Modified;

            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
