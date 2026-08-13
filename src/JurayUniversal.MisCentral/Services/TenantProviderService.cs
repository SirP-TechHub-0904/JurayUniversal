using System;
using System.Collections.Generic;
using System.Linq;
using JurayUniversal.MisCentral.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace JurayUniversal.MisCentral.Services
{
    public interface ITenantProviderService
    {
        List<TenantMetrics> GetTenantsFromConfiguration();
        string ExtractCatalogName(string connectionString);
    }

    public class TenantProviderService : ITenantProviderService
    {
        private readonly IConfiguration _configuration;

        public TenantProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<TenantMetrics> GetTenantsFromConfiguration()
        {
            var rawTenants = new List<TenantConfigEntry>();
            _configuration.GetSection("TenantConfiguration:Tenants").Bind(rawTenants);

            // Group by distinct database catalog / connection string
            var groupedTenants = new Dictionary<string, TenantMetrics>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in rawTenants)
            {
                if (string.IsNullOrWhiteSpace(item.ConnectionString))
                    continue;

                string catalog = ExtractCatalogName(item.ConnectionString);
                string domain = string.IsNullOrWhiteSpace(item.Name) ? catalog : item.Name.Trim();

                if (!groupedTenants.TryGetValue(catalog, out var tenantMetrics))
                {
                    tenantMetrics = new TenantMetrics
                    {
                        CatalogName = catalog,
                        PrimaryDomain = domain,
                        ConnectionString = item.ConnectionString.Trim(),
                        Domains = new List<string>()
                    };
                    groupedTenants[catalog] = tenantMetrics;
                }

                if (!tenantMetrics.Domains.Contains(domain, StringComparer.OrdinalIgnoreCase))
                {
                    tenantMetrics.Domains.Add(domain);
                }
            }

            return groupedTenants.Values.ToList();
        }

        public string ExtractCatalogName(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return "Unknown_DB";

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (!string.IsNullOrWhiteSpace(builder.InitialCatalog))
                    return builder.InitialCatalog;
            }
            catch
            {
                // Fallback string parsing
            }

            try
            {
                var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var kv = part.Split('=', 2);
                    if (kv.Length == 2)
                    {
                        var key = kv[0].Trim();
                        if (key.Equals("Initial Catalog", StringComparison.OrdinalIgnoreCase) ||
                            key.Equals("Database", StringComparison.OrdinalIgnoreCase))
                        {
                            return kv[1].Trim();
                        }
                    }
                }
            }
            catch
            {
                // Ignored
            }

            return "Custom_DB";
        }
    }
}
