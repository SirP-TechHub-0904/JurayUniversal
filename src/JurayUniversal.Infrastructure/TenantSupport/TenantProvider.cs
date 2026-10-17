using Microsoft.Extensions.Options;

namespace JurayUniversal.Infrastructure.TenantSupport;

public class TenantProvider : ITenantProvider
{
    public const string DefaultSchemaName = "dbo";
    private readonly string _defaultConnectionString;
    private readonly Dictionary<string, string> _connectionStringPerTenant;

    public string? CurrentTenant => TenantContext.CurrentTenant;

    //public string DbSchemaName => CurrentTenant ?? DefaultSchemaName;

    public string ConnectionString
    {
        get
        {
            if (CurrentTenant != null)
            {
                if (_connectionStringPerTenant.TryGetValue(CurrentTenant, out var connectionString))
                {
                    return connectionString;
                }

                // Fallback: check without port if CurrentTenant includes port (e.g., [REDACTED_SERVER_IP]:5077 -> [REDACTED_SERVER_IP])
                var hostWithoutPort = CurrentTenant.Split(':')[0];
                if (_connectionStringPerTenant.TryGetValue(hostWithoutPort, out var fallbackConnectionString))
                {
                    return fallbackConnectionString;
                }
            }

            return _defaultConnectionString;
        }
    }

    public TenantProvider(IOptions<TenantConfigurationOptions> tenantConfigurationOptions)
    {
        _defaultConnectionString = tenantConfigurationOptions.Value.ConnectionString;

        _connectionStringPerTenant = tenantConfigurationOptions.Value.Tenants
            .Where(t => !string.IsNullOrWhiteSpace(t.ConnectionString))
            .ToDictionary(ks => ks.Name, vs => vs.ConnectionString!);
    }
    
    public IDisposable BeginScope(string tenant)
    {
        return TenantContext.BeginScope(tenant);
    }

    public override string? ToString()
    {
        return CurrentTenant;
    }
}
