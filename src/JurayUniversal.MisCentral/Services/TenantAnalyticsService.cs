using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Enum;
using JurayUniversal.MisCentral.Models;
using Microsoft.EntityFrameworkCore;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.MisCentral.Services
{
    public interface ITenantAnalyticsService
    {
        Task<DashboardAnalyticsSummary> GetDashboardSummaryAsync();
        Task<bool> UpdateFeaturePricingAsync(string connectionString, long featureId, decimal newCost, FeatureBillingDuration billingDuration, int gracePeriodDays);
        Task<int> UpdateGlobalFeaturePricingAsync(string moduleFolder, decimal newCost, FeatureBillingDuration billingDuration, int gracePeriodDays);
    }

    public class TenantAnalyticsService : ITenantAnalyticsService
    {
        private readonly ITenantProviderService _tenantProvider;
        private readonly IDynamicDbContextFactory _dbContextFactory;

        public TenantAnalyticsService(
            ITenantProviderService tenantProvider,
            IDynamicDbContextFactory dbContextFactory)
        {
            _tenantProvider = tenantProvider;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<DashboardAnalyticsSummary> GetDashboardSummaryAsync()
        {
            var tenants = _tenantProvider.GetTenantsFromConfiguration();
            var summary = new DashboardAnalyticsSummary
            {
                TotalWebsitesCount = tenants.Count,
                Tenants = tenants
            };

            foreach (var tenant in tenants)
            {
                try
                {
                    using var context = _dbContextFactory.CreateDbContext(tenant.ConnectionString);
                    
                    // Quick connection test
                    var canConnect = await context.Database.CanConnectAsync();
                    if (!canConnect)
                    {
                        tenant.IsOnline = false;
                        tenant.StatusErrorMessage = "Could not connect to target SQL database server.";
                        continue;
                    }

                    tenant.IsOnline = true;
                    var features = await context.MisFeatures.AsNoTracking().ToListAsync();

                    tenant.TotalFeatures = features.Count;
                    tenant.EnabledFeatures = features.Count(f => f.Status != FeatureBillingStatus.Disabled);
                    tenant.TotalCost = features.Where(f => f.Status != FeatureBillingStatus.Disabled).Sum(f => f.CostPerDuration);

                    var featureMetrics = new List<PortalFeatureMetric>();
                    int urgentCount = 0;

                    foreach (var f in features)
                    {
                        var metric = new PortalFeatureMetric
                        {
                            Id = f.Id,
                            FeatureName = f.FeatureName,
                            ModuleFolder = f.ModuleFolder,
                            CostPerDuration = f.CostPerDuration,
                            BillingDuration = f.BillingDuration,
                            GracePeriodDays = f.GracePeriodDays,
                            Status = f.Status,
                            NextBillingDate = f.NextBillingDate
                        };
                        featureMetrics.Add(metric);

                        if (f.Status != FeatureBillingStatus.Disabled && f.NextBillingDate.HasValue)
                        {
                            var days = metric.DaysUntilExpiration;
                            if (days.HasValue)
                            {
                                if (days <= 0) summary.ExpiredCount++;
                                else if (days <= 7) summary.Expiring7DaysCount++;
                                else if (days <= 30) summary.Expiring30DaysCount++;

                                if (days <= 7) urgentCount++;
                            }
                        }
                    }

                    tenant.Features = featureMetrics;
                    tenant.UrgentExpiringCount = urgentCount;

                    summary.TotalOnlineCount++;
                    summary.TotalFeaturesAvailable += tenant.TotalFeatures;
                    summary.TotalFeaturesEnabled += tenant.EnabledFeatures;
                    summary.TotalCumulativeCost += tenant.TotalCost;
                }
                catch (Exception ex)
                {
                    tenant.IsOnline = false;
                    tenant.StatusErrorMessage = ex.Message;
                }
            }

            return summary;
        }

        public async Task<bool> UpdateFeaturePricingAsync(string connectionString, long featureId, decimal newCost, FeatureBillingDuration billingDuration, int gracePeriodDays)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext(connectionString);
                var feature = await context.MisFeatures.FirstOrDefaultAsync(f => f.Id == featureId);
                if (feature == null) return false;

                feature.CostPerDuration = newCost;
                feature.BillingDuration = billingDuration;
                feature.GracePeriodDays = gracePeriodDays;

                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> UpdateGlobalFeaturePricingAsync(string moduleFolder, decimal newCost, FeatureBillingDuration billingDuration, int gracePeriodDays)
        {
            var tenants = _tenantProvider.GetTenantsFromConfiguration();
            int updatedCount = 0;

            foreach (var tenant in tenants)
            {
                try
                {
                    using var context = _dbContextFactory.CreateDbContext(tenant.ConnectionString);
                    var features = await context.MisFeatures.Where(f => f.ModuleFolder.ToLower() == moduleFolder.ToLower()).ToListAsync();
                    
                    if (features.Any())
                    {
                        foreach (var f in features)
                        {
                            f.CostPerDuration = newCost;
                            f.BillingDuration = billingDuration;
                            f.GracePeriodDays = gracePeriodDays;
                        }
                        await context.SaveChangesAsync();
                        updatedCount += features.Count;
                    }
                }
                catch
                {
                    // Skip failed tenant connection during batch operation
                }
            }

            return updatedCount;
        }
    }
}
