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
        private readonly JurayUniversal.Website.Services.IMisAuthorizationService _misAuth;

        public MisFeatureAccessFilter(DashboardDbContext context, JurayUniversal.Website.Services.IMisAuthorizationService misAuth)
        {
            _context = context;
            _misAuth = misAuth;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path.Value;

            // 1. Bail out immediately if this is NOT an MIS area request
            if (string.IsNullOrEmpty(path) || !path.StartsWith("/Mis", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }
            
            // 2. Bypass filter for the MisManagement area itself and the Suspended page
            if (path.StartsWith("/MisManagement", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/SuspendedFeature", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }

            // Find if the path matches any known platform feature from our predefined list
            var availableFeatures = JurayUniversal.Website.Areas.MisManagement.Pages.IndexModel.AvailableFeatures;
            var knownFeature = availableFeatures.FirstOrDefault(f => 
                path != null && path.StartsWith("/" + f.ModuleFolder, StringComparison.OrdinalIgnoreCase));

            if (knownFeature != null)
            {
                // The user is trying to access a known feature module.
                // Let's check if this feature is actually configured and enabled in the database.
                var features = await _context.MisFeatures.ToListAsync();
                var matchingFeature = features.FirstOrDefault(f => f.ModuleFolder == knownFeature.ModuleFolder);

                if (matchingFeature == null)
                {
                    // Feature is "Setup Pending" (not even in the DB). Block access!
                    context.Result = new RedirectToPageResult("/Dashboard/Index", new { area = "MIS" });
                    return;
                }

                if (matchingFeature.Status == FeatureBillingStatus.Disabled)
                {
                    // If completely disabled, act like it doesn't exist (or redirect to Dashboard)
                    context.Result = new RedirectToPageResult("/Dashboard/Index", new { area = "MIS" });
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

                // Admin roles are only required for the Dashboard side, NOT the User Platform side.
                if (path != null && !path.Contains("/Platform", StringComparison.OrdinalIgnoreCase) && !path.Contains("/User", StringComparison.OrdinalIgnoreCase))
                {
                    // Check Feature Access Authorization
                    bool hasAccess = await _misAuth.HasAccessAsync(context.HttpContext.User, knownFeature.ModuleFolder);
                    if (!hasAccess)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }

                    // If it's a POST request, block modifications if they don't have Edit access (e.g. Readonly users)
                    if (context.HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                    {
                        bool canEdit = await _misAuth.CanEditAsync(context.HttpContext.User, knownFeature.ModuleFolder);
                        if (!canEdit)
                        {
                            context.Result = new ForbidResult();
                            return;
                        }
                    }
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
