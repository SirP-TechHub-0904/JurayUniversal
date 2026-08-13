using System;
using System.Collections.Generic;
using JurayUniversal.Domain.Enum;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.MisCentral.Models
{
    public class TenantConfigEntry
    {
        public string Name { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class PortalFeatureMetric
    {
        public long Id { get; set; }
        public string FeatureName { get; set; } = string.Empty;
        public string ModuleFolder { get; set; } = string.Empty;
        public decimal CostPerDuration { get; set; }
        public FeatureBillingDuration BillingDuration { get; set; }
        public int GracePeriodDays { get; set; }
        public FeatureBillingStatus Status { get; set; }
        public DateTime? NextBillingDate { get; set; }

        public int? DaysUntilExpiration
        {
            get
            {
                if (!NextBillingDate.HasValue) return null;
                return (int)Math.Ceiling((NextBillingDate.Value - DateTime.Now).TotalDays);
            }
        }

        public string ExpirationStatusBadge
        {
            get
            {
                if (Status == FeatureBillingStatus.Disabled) return "bg-secondary";
                if (!NextBillingDate.HasValue) return "bg-info";

                var days = DaysUntilExpiration;
                if (days < 0) return "bg-danger"; // Expired
                if (days <= 7) return "bg-danger bg-opacity-75"; // Urgent (< 7 days)
                if (days <= 30) return "bg-warning text-dark"; // Warning (< 30 days)
                return "bg-success"; // Healthy
            }
        }

        public string ExpirationStatusText
        {
            get
            {
                if (Status == FeatureBillingStatus.Disabled) return "Disabled";
                if (!NextBillingDate.HasValue) return "No Billing Date";

                var days = DaysUntilExpiration;
                if (days < 0) return $"Expired ({Math.Abs(days.Value)} days ago)";
                if (days == 0) return "Expires Today!";
                if (days == 1) return "Expires Tomorrow!";
                if (days <= 30) return $"Expires in {days} days";
                return $"Renews in {days} days";
            }
        }
    }

    public class TenantMetrics
    {
        public string CatalogName { get; set; } = string.Empty;
        public string PrimaryDomain { get; set; } = string.Empty;
        public List<string> Domains { get; set; } = new List<string>();
        public string ConnectionString { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = false;
        public string? StatusErrorMessage { get; set; }

        public int TotalFeatures { get; set; }
        public int EnabledFeatures { get; set; }
        public decimal TotalCost { get; set; }
        public int UrgentExpiringCount { get; set; }

        public List<PortalFeatureMetric> Features { get; set; } = new List<PortalFeatureMetric>();
    }

    public class DashboardAnalyticsSummary
    {
        public int TotalWebsitesCount { get; set; }
        public int TotalOnlineCount { get; set; }
        public int TotalFeaturesAvailable { get; set; }
        public int TotalFeaturesEnabled { get; set; }
        public decimal TotalCumulativeCost { get; set; }
        public int Expiring7DaysCount { get; set; }
        public int Expiring30DaysCount { get; set; }
        public int ExpiredCount { get; set; }

        public List<TenantMetrics> Tenants { get; set; } = new List<TenantMetrics>();
    }
}
