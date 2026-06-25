using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Filters
{
    public class MisFeatureAccessFilter : IAsyncPageFilter
    {
        private readonly DashboardDbContext _context;

        public MisFeatureAccessFilter(DashboardDbContext context)
        {
            _context = context;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path.Value;
            
            // Bypass filter for the MisManagement area itself and the Suspended page
            if (path != null && (path.StartsWith("/MisManagement", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/SuspendedFeature", StringComparison.OrdinalIgnoreCase)))
            {
                await next();
                return;
            }

            // Find if the path matches any configured feature
            var features = await _context.MisFeatures.ToListAsync();
            
            var matchingFeature = features.FirstOrDefault(f => 
                path != null && path.StartsWith("/" + f.ModuleFolder, StringComparison.OrdinalIgnoreCase));

            if (matchingFeature != null)
            {
                if (matchingFeature.Status == FeatureBillingStatus.Disabled)
                {
                    // If completely disabled, act like it doesn't exist (or redirect to Dashboard)
                    context.Result = new RedirectToPageResult("/Platform/Index", new { area = "MIS" });
                    return;
                }

                if (matchingFeature.Status == FeatureBillingStatus.Suspended)
                {
                    // Feature is suspended due to unpaid bill -> redirect to suspended page
                    context.Result = new RedirectToPageResult("/SuspendedFeature", new { area = "MisManagement", featureName = matchingFeature.FeatureName });
                    return;
                }

                // If Active, check billing date and grace period to see if we need to auto-suspend
                if (matchingFeature.NextBillingDate.HasValue && matchingFeature.NextBillingDate.Value.AddDays(matchingFeature.GracePeriodDays) < DateTime.UtcNow)
                {
                    // Auto-suspend the feature
                    matchingFeature.Status = FeatureBillingStatus.Suspended;
                    _context.Entry(matchingFeature).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    context.Result = new RedirectToPageResult("/SuspendedFeature", new { area = "MisManagement", featureName = matchingFeature.FeatureName });
                    return;
                }
            }

            await next();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
