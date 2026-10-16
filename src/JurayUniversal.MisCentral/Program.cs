using System.IO;
using JurayUniversal.MisCentral.Data;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

JurayUniversal.MisCentral.DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

// Load shared appsettings.json from JurayUniversal.Website
var sharedAppSettingsPath = Path.Combine(builder.Environment.ContentRootPath, "../JurayUniversal.Website/appsettings.json");
if (File.Exists(sharedAppSettingsPath))
{
    builder.Configuration.AddJsonFile(sharedAppSettingsPath, optional: false, reloadOnChange: true);
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? builder.Configuration.GetSection("TenantConfiguration:ConnectionString").Value
    ?? "Server=localhost;Database=JurayMisCentralDb;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "JurayUniversal.MisCentral.Auth";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/Administrator/Account/Login";
    options.LogoutPath = "/Administrator/Account/Logout";
    options.AccessDeniedPath = "/Administrator/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddRazorPages(options =>
{
    // Secure Master Administrator Area
    options.Conventions.AuthorizeAreaFolder("Administrator", "/");
    options.Conventions.AllowAnonymousToAreaFolder("Administrator", "/Account");
    
    // Public Central Website Pages
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/About");
    options.Conventions.AllowAnonymousToPage("/Features");
    options.Conventions.AllowAnonymousToPage("/Pricing");
    options.Conventions.AllowAnonymousToPage("/Contact");
    options.Conventions.AllowAnonymousToPage("/Login");
    options.Conventions.AllowAnonymousToPage("/Privacy");
    options.Conventions.AllowAnonymousToPage("/Error");
});

// Register Central Control Hub Services
builder.Services.AddSingleton<ITenantProviderService, TenantProviderService>();
builder.Services.AddSingleton<IDynamicDbContextFactory, DynamicDbContextFactory>();
builder.Services.AddScoped<ITenantAnalyticsService, TenantAnalyticsService>();
builder.Services.AddSingleton<IMasterSuperAdminAuthService, MasterSuperAdminAuthService>();
builder.Services.AddScoped<ITenantOnboardingService, TenantOnboardingService>();
builder.Services.AddScoped<ITenantSyncService, TenantSyncService>();
builder.Services.AddSingleton<IProspectiveLeadService, ProspectiveLeadService>();
builder.Services.AddSingleton<IPricingPlanService, PricingPlanService>();

var app = builder.Build();

// Seed Default Central Roles & Master Super Admin User
await MasterIdentitySeeder.SeedAsync(app.Services);

// Check for CLI sync flag
if (args.Contains("--sync-tenants"))
{
    Console.WriteLine("==================================================");
    Console.WriteLine("  STARTING TENANT CATALOG SYNC & MIGRATIONS RUN  ");
    Console.WriteLine("==================================================");

    using (var scope = app.Services.CreateScope())
    {
        var syncService = scope.ServiceProvider.GetRequiredService<ITenantSyncService>();
        var summary = await syncService.SyncAllDatabasesToMasterCatalogAsync();

        Console.WriteLine($"\nSync Finished:");
        Console.WriteLine($"  - Total Databases Scanned : {summary.TotalDatabasesFound}");
        Console.WriteLine($"  - Newly Registered Catalogs: {summary.NewlyRegisteredCatalogs}");
        Console.WriteLine($"  - Updated Existing Catalogs: {summary.UpdatedCatalogs}");
        Console.WriteLine($"  - Migrations Succeeded     : {summary.SuccessfulMigrations}");
        Console.WriteLine($"  - Migrations Failed        : {summary.FailedMigrations}");
        Console.WriteLine("--------------------------------------------------");
        foreach (var r in summary.Results)
        {
            Console.WriteLine($"  [{r.DatabaseName}] -> Key: {r.TenantKey}, Org: {r.OrganizationName}, Migrated: {r.MigrationSucceeded} ({r.StatusMessage})");
        }
        Console.WriteLine("==================================================");
    }
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
