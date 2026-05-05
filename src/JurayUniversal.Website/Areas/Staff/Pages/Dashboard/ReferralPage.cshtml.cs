using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class ReferralPageModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ReferralPageModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Profile Profile { get; set; }
        public string SiteUrl { get; set; }
        public async Task OnGetAsync()
        {
            Profile = await _userManager.GetUserAsync(User);
            var superSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            if(superSetting != null)
            {
                SiteUrl = CleanDomain(superSetting.CompanyWebsiteLink);
            }
        }
        private string CleanDomain(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "";

            url = url.Trim().ToLower();

            // Remove protocol
            url = url.Replace("http://", "")
                     .Replace("https://", "");

            // Remove trailing slash
            url = url.TrimEnd('/');

            return url;
        }
    }

}
