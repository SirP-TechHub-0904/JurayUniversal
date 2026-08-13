using System.Threading.Tasks;
using JurayUniversal.Domain.Enum;
using JurayUniversal.MisCentral.Models;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.MisCentral.Pages
{
    public class FeaturePricingModel : PageModel
    {
        private readonly ITenantAnalyticsService _analyticsService;

        public FeaturePricingModel(ITenantAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public DashboardAnalyticsSummary Summary { get; set; } = new DashboardAnalyticsSummary();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            Summary = await _analyticsService.GetDashboardSummaryAsync();
        }

        public async Task<IActionResult> OnPostUpdateInstanceFeatureAsync(
            string connectionString, 
            long featureId, 
            decimal costPerDuration, 
            FeatureBillingDuration billingDuration, 
            int gracePeriodDays)
        {
            var success = await _analyticsService.UpdateFeaturePricingAsync(connectionString, featureId, costPerDuration, billingDuration, gracePeriodDays);
            if (success)
            {
                StatusMessage = "Feature pricing updated successfully for selected instance!";
            }
            else
            {
                StatusMessage = "Error updating feature pricing.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateGlobalFeatureAsync(
            string moduleFolder, 
            decimal costPerDuration, 
            FeatureBillingDuration billingDuration, 
            int gracePeriodDays)
        {
            int updated = await _analyticsService.UpdateGlobalFeaturePricingAsync(moduleFolder, costPerDuration, billingDuration, gracePeriodDays);
            StatusMessage = $"Successfully updated pricing for feature module '{moduleFolder}' across {updated} instance(s)!";

            return RedirectToPage();
        }
    }
}
