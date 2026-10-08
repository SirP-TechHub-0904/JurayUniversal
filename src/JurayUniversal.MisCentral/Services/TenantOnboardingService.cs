using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JurayUniversal.Domain.Enum;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JurayUniversal.MisCentral.Services
{
    public class TenantOnboardingRequest
    {
        public string OrganizationName { get; set; } = string.Empty;
        public string TenantKey { get; set; } = string.Empty;
        public string AdminFirstName { get; set; } = string.Empty;
        public string AdminLastName { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? CustomDomain { get; set; }
        public string? Sector { get; set; }
        public List<string> SelectedFeatureKeys { get; set; } = new List<string>();
    }

    public class TenantOnboardingResult
    {
        public bool Success { get; set; }
        public string TenantKey { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public string PrimaryDomain { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? ErrorDetail { get; set; }
    }

    public interface ITenantOnboardingService
    {
        Task<TenantOnboardingResult> OnboardNewTenantAsync(TenantOnboardingRequest request, CancellationToken cancellationToken = default);
        Task<bool> IsTenantKeyAvailableAsync(string tenantKey, CancellationToken cancellationToken = default);
        Task<List<MasterTenantCatalog>> GetAllTenantsAsync(CancellationToken cancellationToken = default);
        Task<MasterTenantCatalog?> GetTenantByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateTenantStatusAsync(int id, bool isActive, CancellationToken cancellationToken = default);
    }

    public class TenantOnboardingService : ITenantOnboardingService
    {
        private readonly ApplicationDbContext _centralDbContext;
        private readonly IDynamicDbContextFactory _dbContextFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TenantOnboardingService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TenantOnboardingService(
            ApplicationDbContext centralDbContext,
            IDynamicDbContextFactory dbContextFactory,
            IConfiguration configuration,
            ILogger<TenantOnboardingService> logger,
            IServiceProvider serviceProvider)
        {
            _centralDbContext = centralDbContext;
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> IsTenantKeyAvailableAsync(string tenantKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tenantKey)) return false;
            var cleanKey = tenantKey.Trim().ToLowerInvariant();

            if (ReservedWorkspaceNames.IsReserved(cleanKey))
            {
                return false;
            }

            return !await _centralDbContext.MasterTenantCatalogs
                .AnyAsync(t => t.TenantKey == cleanKey, cancellationToken);
        }

        public async Task<List<MasterTenantCatalog>> GetAllTenantsAsync(CancellationToken cancellationToken = default)
        {
            return await _centralDbContext.MasterTenantCatalogs
                .Include(t => t.FeatureSubscriptions)
                .OrderByDescending(t => t.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<MasterTenantCatalog?> GetTenantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _centralDbContext.MasterTenantCatalogs
                .Include(t => t.FeatureSubscriptions)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateTenantStatusAsync(int id, bool isActive, CancellationToken cancellationToken = default)
        {
            var tenant = await _centralDbContext.MasterTenantCatalogs.FindAsync(new object[] { id }, cancellationToken);
            if (tenant == null) return false;

            tenant.IsActive = isActive;
            tenant.UpdatedAtUtc = DateTime.UtcNow;
            await _centralDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<TenantOnboardingResult> OnboardNewTenantAsync(TenantOnboardingRequest request, CancellationToken cancellationToken = default)
        {
            var cleanKey = request.TenantKey.Trim().ToLowerInvariant().Replace(" ", "-");

            var baseDomain = _configuration["TenantSettings:BaseDomain"]
                          ?? _configuration["TenantSettings__BaseDomain"]
                          ?? "jurayuniversal.com";

            var primaryDomain = $"{cleanKey}.{baseDomain}";

            // 1. Validate Not Reserved
            if (ReservedWorkspaceNames.IsReserved(cleanKey))
            {
                return new TenantOnboardingResult
                {
                    Success = false,
                    Message = $"Workspace key '{cleanKey}' is reserved by the system."
                };
            }

            // 2. Validate Uniqueness
            bool keyTaken = await _centralDbContext.MasterTenantCatalogs
                .AnyAsync(t => t.TenantKey == cleanKey || t.PrimaryDomain == primaryDomain, cancellationToken);

            if (keyTaken)
            {
                return new TenantOnboardingResult
                {
                    Success = false,
                    Message = $"Workspace key '{cleanKey}' is already registered."
                };
            }

            // 3. Resolve Master Connection and Build Dedicated Tenant Connection
            var masterConnStr = _centralDbContext.Database.GetConnectionString()
                ?? _configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

            var serverBuilder = new SqlConnectionStringBuilder(masterConnStr);
            string serverHost = serverBuilder.DataSource;

            string dbName = TenantSecurityGenerator.GenerateDatabaseName(cleanKey);

            var tenantConnBuilder = new SqlConnectionStringBuilder
            {
                DataSource = serverHost,
                InitialCatalog = dbName,
                TrustServerCertificate = true,
                PersistSecurityInfo = true
            };

            if (serverBuilder.IntegratedSecurity)
            {
                tenantConnBuilder.IntegratedSecurity = true;
            }
            else
            {
                tenantConnBuilder.UserID = serverBuilder.UserID;
                tenantConnBuilder.Password = serverBuilder.Password;
            }

            string dedicatedTenantConnStr = tenantConnBuilder.ConnectionString;

            try
            {
                _logger.LogInformation("================================================================");
                _logger.LogInformation("PROVISIONING ISOLATED TENANT: [{TenantKey}] - {OrgName}", cleanKey, request.OrganizationName);
                _logger.LogInformation("Database Name: {DatabaseName}", dbName);
                _logger.LogInformation("Primary Domain: {PrimaryDomain}", primaryDomain);

                // Step A: Create database via SQL command on master DB
                await CreateDatabaseAsync(masterConnStr, dbName, cancellationToken);

                // Step B: Apply EF Core Migrations using DashboardDbContext
                await ApplyTenantMigrationsAsync(dedicatedTenantConnStr, cancellationToken);

                // Step C: Seed Initial Admin User & Base Settings
                await SeedInitialTenantDataAsync(dedicatedTenantConnStr, request, cancellationToken);

                // Step D: Register in Master Catalog
                var catalogEntry = new MasterTenantCatalog
                {
                    TenantKey = cleanKey,
                    OrganizationName = request.OrganizationName,
                    PrimaryDomain = primaryDomain,
                    WebsiteUrl = $"https://{primaryDomain}",
                    CustomDomain = string.IsNullOrWhiteSpace(request.CustomDomain) ? null : request.CustomDomain.Trim().ToLowerInvariant(),
                    DatabaseName = dbName,
                    ConnectionString = dedicatedTenantConnStr,
                    AdminEmail = request.AdminEmail.Trim().ToLowerInvariant(),
                    ContactPhone = request.PhoneNumber,
                    Sector = request.Sector,
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow
                };

                _centralDbContext.MasterTenantCatalogs.Add(catalogEntry);
                await _centralDbContext.SaveChangesAsync(cancellationToken);

                // Step E: Grant Feature Subscriptions
                var availableFeatures = await _centralDbContext.MasterFeatureCatalogs.ToListAsync(cancellationToken);
                var selectedKeys = request.SelectedFeatureKeys ?? new List<string>();

                foreach (var feat in availableFeatures)
                {
                    bool isSelected = feat.IsCompulsory || selectedKeys.Contains(feat.FeatureKey, StringComparer.OrdinalIgnoreCase);
                    if (isSelected)
                    {
                        _centralDbContext.MasterTenantFeatureSubscriptions.Add(new MasterTenantFeatureSubscription
                        {
                            MasterTenantCatalogId = catalogEntry.Id,
                            FeatureKey = feat.FeatureKey,
                            FeatureName = feat.Name,
                            IsEnabled = true,
                            EnabledAtUtc = DateTime.UtcNow,
                            GrantedBy = "Central SuperAdmin"
                        });
                    }
                }

                await _centralDbContext.SaveChangesAsync(cancellationToken);

                return new TenantOnboardingResult
                {
                    Success = true,
                    TenantKey = cleanKey,
                    OrganizationName = request.OrganizationName,
                    PrimaryDomain = primaryDomain,
                    DatabaseName = dbName,
                    Message = $"Workspace '{request.OrganizationName}' provisioned successfully!"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to provision tenant workspace [{TenantKey}]", cleanKey);
                return new TenantOnboardingResult
                {
                    Success = false,
                    TenantKey = cleanKey,
                    OrganizationName = request.OrganizationName,
                    Message = $"Provisioning failed: {ex.Message}",
                    ErrorDetail = ex.ToString()
                };
            }
        }

        private async Task CreateDatabaseAsync(string masterConnStr, string dbName, CancellationToken cancellationToken)
        {
            var masterBuilder = new SqlConnectionStringBuilder(masterConnStr)
            {
                InitialCatalog = "master"
            };

            await using var connection = new SqlConnection(masterBuilder.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            var checkSql = "SELECT COUNT(*) FROM sys.databases WHERE name = @dbName";
            await using (var cmd = new SqlCommand(checkSql, connection))
            {
                cmd.Parameters.AddWithValue("@dbName", dbName);
                int count = (int)(await cmd.ExecuteScalarAsync(cancellationToken) ?? 0);
                if (count == 0)
                {
                    var createSql = $"CREATE DATABASE [{dbName}]";
                    await using var createCmd = new SqlCommand(createSql, connection);
                    await createCmd.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        private async Task ApplyTenantMigrationsAsync(string tenantConnStr, CancellationToken cancellationToken)
        {
            var tenantDbContext = _dbContextFactory.CreateDbContext(tenantConnStr);
            await tenantDbContext.Database.MigrateAsync(cancellationToken);
        }

        private async Task SeedInitialTenantDataAsync(string tenantConnStr, TenantOnboardingRequest request, CancellationToken cancellationToken)
        {
            var tenantDbContext = _dbContextFactory.CreateDbContext(tenantConnStr);

            // 1. Seed Roles
            var roles = new[] { "mSuperAdmin", "Admin", "Administrator", "Editor", "User", "Member" };
            foreach (var roleName in roles)
            {
                if (!await tenantDbContext.Roles.AnyAsync(r => r.Name == roleName, cancellationToken))
                {
                    tenantDbContext.Roles.Add(new AppRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = roleName,
                        NormalizedName = roleName.ToUpperInvariant(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }
            }
            await tenantDbContext.SaveChangesAsync(cancellationToken);

            // 2. Seed Admin User Profile
            var email = request.AdminEmail.Trim().ToLowerInvariant();
            if (!await tenantDbContext.Users.AnyAsync(u => u.Email == email, cancellationToken))
            {
                var hasher = new PasswordHasher<Profile>();
                var adminUser = new Profile
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = email,
                    NormalizedUserName = email.ToUpperInvariant(),
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    EmailConfirmed = true,
                    FirstName = request.AdminFirstName.Trim(),
                    LastName = request.AdminLastName.Trim(),
                    PhoneNumber = request.PhoneNumber,
                    PhoneNumberConfirmed = !string.IsNullOrEmpty(request.PhoneNumber),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Date = DateTime.UtcNow,
                    UserStatus = JurayUniversal.Domain.Enum.Enum.UserStatus.Active
                };

                adminUser.PasswordHash = hasher.HashPassword(adminUser, request.AdminPassword);
                tenantDbContext.Users.Add(adminUser);
                await tenantDbContext.SaveChangesAsync(cancellationToken);

                // Assign Admin & mSuperAdmin roles
                var adminRole = await tenantDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);
                var superAdminRole = await tenantDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "mSuperAdmin", cancellationToken);

                if (adminRole != null)
                {
                    tenantDbContext.UserRoles.Add(new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = adminRole.Id });
                }
                if (superAdminRole != null)
                {
                    tenantDbContext.UserRoles.Add(new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = superAdminRole.Id });
                }
                await tenantDbContext.SaveChangesAsync(cancellationToken);
            }

            // 3. Seed Default SuperSetting and Setting
            if (!await tenantDbContext.SuperSettings.AnyAsync(cancellationToken))
            {
                tenantDbContext.SuperSettings.Add(new SuperSetting
                {
                    CompanyName = request.OrganizationName,
                    CompanyNameAbbreviation = request.TenantKey.ToUpperInvariant(),
                    WebsiteTitle = request.OrganizationName,
                    DashboardTitle = $"{request.OrganizationName} Management System",
                    ActivateLogin = true,
                    ActivateLoginInMenu = true,
                    MaximumUsers = 1000,
                    TemplateLayoutKey = "Layout1111",
                    SliderTemplateKey = "Slider1111",
                    BlogTemplateKey = "Blog1111"
                });
            }

            if (!await tenantDbContext.Settings.AnyAsync(cancellationToken))
            {
                tenantDbContext.Settings.Add(new Setting
                {
                    AddressOne = request.OrganizationName
                });
            }

            if (!await tenantDbContext.MisSettings.AnyAsync(cancellationToken))
            {
                tenantDbContext.MisSettings.Add(new MisSetting
                {
                    CategoryAlias = "Category",
                    AlumniAlias = "Alumni"
                });
            }

            // 4. Seed Active Modules in MisFeatures
            var featureKeys = request.SelectedFeatureKeys ?? new List<string>();
            foreach (var key in featureKeys)
            {
                if (!await tenantDbContext.MisFeatures.AnyAsync(f => f.ModuleFolder == key, cancellationToken))
                {
                    tenantDbContext.MisFeatures.Add(new MisFeature
                    {
                        FeatureName = key,
                        ModuleFolder = key,
                        Status = JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Active,
                        BillingDuration = JurayUniversal.Domain.Enum.Enum.FeatureBillingDuration.Monthly,
                        CostPerDuration = 0
                    });
                }
            }

            await tenantDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
