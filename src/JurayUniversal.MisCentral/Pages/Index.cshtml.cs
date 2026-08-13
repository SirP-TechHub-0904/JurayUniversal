using System.Threading.Tasks;
using JurayUniversal.MisCentral.Models;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITenantAnalyticsService _analyticsService;
        private readonly IMasterSuperAdminAuthService _authService;

        public IndexModel(
            ITenantAnalyticsService analyticsService,
            IMasterSuperAdminAuthService authService)
        {
            _analyticsService = analyticsService;
            _authService = authService;
        }

        public DashboardAnalyticsSummary Summary { get; set; } = new DashboardAnalyticsSummary();

        public async Task OnGetAsync()
        {
            Summary = await _analyticsService.GetDashboardSummaryAsync();
        }

        public string GetConfigUrl(string domainName)
        {
            return _authService.GetTargetConfigUrl(domainName);
        }

        public string GetMisUrl(string domainName)
        {
            return _authService.GetTargetMisUrl(domainName);
        }
    }
}
