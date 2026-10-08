using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using JurayUniversal.MisCentral.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JurayUniversal.MisCentral.Services
{
    public class TenantSyncResultItem
    {
        public string DatabaseName { get; set; } = string.Empty;
        public string TenantKey { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public string PrimaryDomain { get; set; } = string.Empty;
        public List<string> Domains { get; set; } = new();
        public bool IsCatalogRegistered { get; set; }
        public bool MigrationSucceeded { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }

    public class TenantSyncSummary
    {
        public int TotalDatabasesFound { get; set; }
        public int NewlyRegisteredCatalogs { get; set; }
        public int UpdatedCatalogs { get; set; }
        public int SuccessfulMigrations { get; set; }
        public int FailedMigrations { get; set; }
        public List<TenantSyncResultItem> Results { get; set; } = new();
    }

    public interface ITenantSyncService
    {
        Task<TenantSyncSummary> SyncAllDatabasesToMasterCatalogAsync(CancellationToken cancellationToken = default);
    }

    public class TenantSyncService : ITenantSyncService
    {
        private readonly ApplicationDbContext _centralDbContext;
        private readonly IDynamicDbContextFactory _dbContextFactory;
        private readonly IConfiguration _configuration;
        private readonly ITenantProviderService _tenantProviderService;
        private readonly ILogger<TenantSyncService> _logger;

        public TenantSyncService(
            ApplicationDbContext centralDbContext,
            IDynamicDbContextFactory dbContextFactory,
            IConfiguration configuration,
            ITenantProviderService tenantProviderService,
            ILogger<TenantSyncService> logger)
        {
            _centralDbContext = centralDbContext;
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
            _tenantProviderService = tenantProviderService;
            _logger = logger;
        }

        public async Task<TenantSyncSummary> SyncAllDatabasesToMasterCatalogAsync(CancellationToken cancellationToken = default)
        {
            var summary = new TenantSyncSummary();

            // 1. Get raw tenant list from Configuration
            var tenantMetrics = _tenantProviderService.GetTenantsFromConfiguration();
            summary.TotalDatabasesFound = tenantMetrics.Count;

            // Ensure master feature catalogs are loaded
            var masterFeatures = await _centralDbContext.MasterFeatureCatalogs.ToListAsync(cancellationToken);

            foreach (var metric in tenantMetrics)
            {
                var resultItem = new TenantSyncResultItem
                {
                    DatabaseName = metric.CatalogName,
                    PrimaryDomain = metric.PrimaryDomain,
                    Domains = metric.Domains
                };

                try
                {
                    // Generate a clean tenant key from catalog or primary domain
                    string cleanKey = metric.PrimaryDomain
                        .Replace("https://", "")
                        .Replace("http://", "")
                        .Replace("www.", "")
                        .Split('.')[0]
                        .ToLowerInvariant();

                    if (string.IsNullOrWhiteSpace(cleanKey) || cleanKey.Length < 2)
                    {
                        cleanKey = metric.CatalogName.ToLowerInvariant().Replace("db_", "").Replace("jurayuniversal_", "");
                    }

                    resultItem.TenantKey = cleanKey;

                    // Check if catalog record already exists by ConnectionString or DatabaseName or TenantKey
                    var existingCatalog = await _centralDbContext.MasterTenantCatalogs
                        .Include(t => t.FeatureSubscriptions)
                        .FirstOrDefaultAsync(t => 
                            t.DatabaseName == metric.CatalogName || 
                            t.ConnectionString == metric.ConnectionString ||
                            t.TenantKey == cleanKey, cancellationToken);

                    string orgName = metric.PrimaryDomain;
                    string? adminEmail = null;
                    string? phone = null;
                    string? customDomain = metric.Domains.Count > 1 ? metric.Domains.FirstOrDefault(d => !d.Equals(metric.PrimaryDomain, StringComparison.OrdinalIgnoreCase)) : null;

                    // Connect to tenant DB to read SuperSetting, Admin User, and run migrations
                    bool dbReachable = false;
                    try
                    {
                        var tenantDbContext = _dbContextFactory.CreateDbContext(metric.ConnectionString);
                        
                        // Run EF Core migrations
                        await tenantDbContext.Database.MigrateAsync(cancellationToken);
                        resultItem.MigrationSucceeded = true;
                        summary.SuccessfulMigrations++;
                        dbReachable = true;

                        // Query SuperSetting if available
                        var superSetting = await tenantDbContext.SuperSettings.FirstOrDefaultAsync(cancellationToken);
                        if (superSetting != null && !string.IsNullOrWhiteSpace(superSetting.CompanyName))
                        {
                            orgName = superSetting.CompanyName;
                        }

                        // Query Admin user if available
                        var adminUser = await tenantDbContext.Users.FirstOrDefaultAsync(cancellationToken);
                        if (adminUser != null)
                        {
                            adminEmail = adminUser.Email;
                            phone = adminUser.PhoneNumber;
                        }
                    }
                    catch (Exception exDb)
                    {
                        _logger.LogWarning(exDb, "Could not run migrations or extract info from database [{DbName}]", metric.CatalogName);
                        resultItem.MigrationSucceeded = false;
                        resultItem.StatusMessage = $"DB Migration Warning: {exDb.Message}";
                        summary.FailedMigrations++;
                    }

                    resultItem.OrganizationName = orgName;

                    if (existingCatalog == null)
                    {
                        // Create MasterTenantCatalog entry
                        var newCatalog = new MasterTenantCatalog
                        {
                            TenantKey = cleanKey,
                            OrganizationName = orgName,
                            PrimaryDomain = metric.PrimaryDomain,
                            CustomDomain = customDomain,
                            DatabaseName = metric.CatalogName,
                            ConnectionString = metric.ConnectionString,
                            AdminEmail = adminEmail,
                            ContactPhone = phone,
                            IsActive = true,
                            CreatedAtUtc = DateTime.UtcNow
                        };

                        // Add default module subscriptions
                        foreach (var feature in masterFeatures)
                        {
                            newCatalog.FeatureSubscriptions.Add(new MasterTenantFeatureSubscription
                            {
                                FeatureKey = feature.FeatureKey,
                                FeatureName = feature.Name,
                                IsEnabled = true,
                                EnabledAtUtc = DateTime.UtcNow,
                                GrantedBy = "System Sync"
                            });
                        }

                        _centralDbContext.MasterTenantCatalogs.Add(newCatalog);
                        summary.NewlyRegisteredCatalogs++;
                        resultItem.IsCatalogRegistered = true;
                    }
                    else
                    {
                        // Update existing catalog
                        existingCatalog.DatabaseName = metric.CatalogName;
                        existingCatalog.ConnectionString = metric.ConnectionString;
                        if (dbReachable && !string.IsNullOrWhiteSpace(orgName) && orgName != metric.PrimaryDomain)
                        {
                            existingCatalog.OrganizationName = orgName;
                        }
                        if (!string.IsNullOrWhiteSpace(adminEmail))
                        {
                            existingCatalog.AdminEmail = adminEmail;
                        }
                        if (!string.IsNullOrWhiteSpace(phone))
                        {
                            existingCatalog.ContactPhone = phone;
                        }
                        if (!string.IsNullOrWhiteSpace(customDomain))
                        {
                            existingCatalog.CustomDomain = customDomain;
                        }
                        existingCatalog.UpdatedAtUtc = DateTime.UtcNow;

                        // Ensure missing feature subscriptions are added
                        foreach (var feature in masterFeatures)
                        {
                            if (!existingCatalog.FeatureSubscriptions.Any(s => s.FeatureKey.Equals(feature.FeatureKey, StringComparison.OrdinalIgnoreCase)))
                            {
                                existingCatalog.FeatureSubscriptions.Add(new MasterTenantFeatureSubscription
                                {
                                    MasterTenantCatalogId = existingCatalog.Id,
                                    FeatureKey = feature.FeatureKey,
                                    FeatureName = feature.Name,
                                    IsEnabled = true,
                                    EnabledAtUtc = DateTime.UtcNow,
                                    GrantedBy = "System Sync"
                                });
                            }
                        }

                        summary.UpdatedCatalogs++;
                        resultItem.IsCatalogRegistered = true;
                    }

                    if (string.IsNullOrEmpty(resultItem.StatusMessage))
                    {
                        resultItem.StatusMessage = "Successfully synced and migrated.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing database [{DbName}]", metric.CatalogName);
                    resultItem.StatusMessage = $"Error: {ex.Message}";
                }

                summary.Results.Add(resultItem);
            }

            await _centralDbContext.SaveChangesAsync(cancellationToken);
            return summary;
        }
    }
}
