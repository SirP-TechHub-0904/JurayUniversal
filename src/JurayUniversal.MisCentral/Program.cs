using System.IO;
using JurayUniversal.MisCentral.Data;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// Register Central Control Hub Services
builder.Services.AddSingleton<ITenantProviderService, TenantProviderService>();
builder.Services.AddSingleton<IDynamicDbContextFactory, DynamicDbContextFactory>();
builder.Services.AddScoped<ITenantAnalyticsService, TenantAnalyticsService>();
builder.Services.AddSingleton<IMasterSuperAdminAuthService, MasterSuperAdminAuthService>();

var app = builder.Build();

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
