using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace JurayUniversal.Website.Pages
{
    public class PortfolioAccountModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
                private readonly UserManager<Profile> _userManager;

        public PortfolioAccountModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, UserManager<Profile> userManager)
        {
            _context = context;
            _settingsService = settingsService;
            _userManager = userManager;
        }
        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }
        public async Task<IActionResult> OnGetAsync()
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

            return Page();
        }
        [BindProperty]
        public PortfolioAccountSetupDto Input { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
             bool isAvailable = await DomainExistsInDatabase(Input.ProfileDomain);
            if(isAvailable == false)
            {
                TempData["error"] = "Domain already taken";
                return Page();
            }
            bool isEmailAvailable = await EmailExistsInDatabase(Input.Email);
            if(isEmailAvailable == false)
            {
                TempData["error"] = "Email already taken";
                return Page();
            }
            var httpContext = HttpContext;
            var result = await _settingsService.CreateAccount(Input, httpContext);
            if (result == true)
            {
                Guid newGuid = Guid.NewGuid();
                Guid newGuidx = Guid.NewGuid();
                return RedirectToPage("./Processing", new {data = Input.Email, key = newGuid.ToString(), query = newGuidx.ToString() });
            }
            return Page();
        }

        public async Task<IActionResult> OnGetCheckEmailAvailability(string email)
        {
         
            bool isAvailable = await EmailExistsInDatabase(email);

            return new JsonResult(new { isAvailable });
        }

        private async Task<bool> EmailExistsInDatabase(string email)
        {
           var check = await _userManager.FindByEmailAsync(email);
            if(check == null)
            {
                return true;
            }
            return false;
        }

        
        public async Task<IActionResult> OnGetCheckDomainAvailability(string domain)
        {
         
            bool isAvailable = await DomainExistsInDatabase(domain);

            return new JsonResult(new { isAvailable });
        }

        private async Task<bool> DomainExistsInDatabase(string domain)
        {
           var check = await _context.ProfilePortfolioSetting.FirstOrDefaultAsync(x=>x.DefaultWebLink == domain);
            if(check == null)
            {
                return true;
            }
            return false;
        }


    }
}
