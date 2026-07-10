using Amazon.S3;
using JurayUniversal.Domain.Models;
using JurayUniversal.Infrastructure.Extensions;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Persistence.EF.SQL.Extensions;
using JurayUniversal.Website.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Repository.DeviceInformation;
using Serilog;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using JurayUniversal.Application.Repository.Base;
using JurayUniversal.Application.Repository.GeneralRepository.ProjectsCategory;
using JurayUniversal.Application.Repository.GeneralRepository.Users;
using JurayUniversal.Application.Repository.GeneralRepository.Projects;
using JurayUniversal.Application.Repository.Services;
using Microsoft.AspNetCore.Authorization;
using JurayUniversal.Application.Repository.DomainServices;
using System.Web.Mvc;
using JurayUniversal.Application.Services;
using PostmarkEmailService;
using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Application.Repository.MIS.UserManagement;

namespace JurayUniversal.Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning() // Don't log Information to avoid huge files, Warning/Error/Fatal only
                .Enrich.FromLogContext()
                .WriteTo.Map("Tenant", "System", (tenant, wt) => 
                    wt.File(Path.Combine("wwwroot", "logs", tenant.ToString(), "syslog-.txt"), 
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30))
                .CreateLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog();

                // Add services to the container.

            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services
                .AddTenantSupport(builder.Configuration)
                .AddEntityFrameworkSqlServer<DashboardDbContext>();

            builder.Services.AddIdentity<Profile, AppRole>()
                        .AddEntityFrameworkStores<DashboardDbContext>()
                        .AddDefaultTokenProviders();

            builder.Services.AddRazorPages().AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(JurayUniversal.Website.Filters.MisFeatureAccessFilter));
            });
            //builder.Services.addser();
            builder.Services.AddTransient<IStorageService, StorageService>();
            builder.Services.AddTransient<INotifyRegisterService, NotifyRegisterService>();
            builder.Services.AddTransient<IDeviceService, DeviceService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddScoped<IRazorRenderService, RazorRenderService>();
            builder.Services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            builder.Services.AddTransient<IProjectCategoryRepositoryAsync, ProjectCategoryRepositoryAsync>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IUserRepositoryAsync, UserRepositoryAsync>();
            builder.Services.AddTransient<IProjectRepositoryAsync, ProjectRepositoryAsync>();
            builder.Services.AddTransient<ISettingsService, SettingsService>();
            builder.Services.AddTransient<IDomainRepository, DomainRepository>();
            builder.Services.AddTransient<IEmailSendService, EmailSendService>();
            builder.Services.AddTransient<ISmsSendService, SmsSendService>();
            builder.Services.AddTransient<IZeptoMailService, ZeptoMailService>();
            builder.Services.AddHttpClient<IPaymentService, PixelPayService>();
            builder.Services.AddHttpClient<PaystackService>();
            builder.Services.AddHttpClient<JurayUniversal.Website.Services.IMisSmartAssistService, JurayUniversal.Website.Services.MisSmartAssistService>();

            // --- MIS Feature Repositories ---
            builder.Services.AddScoped<JurayUniversal.Website.Services.IMisAuthorizationService, JurayUniversal.Website.Services.MisAuthorizationService>();
            builder.Services.AddTransient<IMisUserManagementRepository, MisUserManagementRepository>();
            builder.Services.AddSingleton<IServiceProvider>(provider => provider);
            // Add registration for LoggerLibrary
 
            builder.Services.AddSingleton<LoggerLibrary>(provider =>
            {
                var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
                var wwwRootPath = hostingEnvironment.WebRootPath;
                return new LoggerLibrary(wwwRootPath);
            });

            builder.Services.AddAuthorization(options =>
            {
                // User Management
                options.AddPolicy("MisUserManagementViewer", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisUserManagement", "GlobalViewer")));
                options.AddPolicy("MisUserManagementEditor", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisUserManagement", "GlobalEditor")));
                
                // Groups
                options.AddPolicy("MisGroupsViewer", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisGroups", "GlobalViewer")));
                options.AddPolicy("MisGroupsEditor", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisGroups", "GlobalEditor")));
                
                // Alumni
                options.AddPolicy("MisAlumniViewer", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisAlumni", "GlobalViewer")));
                options.AddPolicy("MisAlumniEditor", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisAlumni", "GlobalEditor")));
                
                // Reports
                options.AddPolicy("MisReportsViewer", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisReports", "GlobalViewer")));
                options.AddPolicy("MisReportsEditor", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisReports", "GlobalEditor")));
                
                // Grants
                options.AddPolicy("MisGrantsViewer", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisGrants", "GlobalViewer")));
                options.AddPolicy("MisGrantsEditor", policy => policy.Requirements.Add(new JurayUniversal.Website.Filters.MisFeatureRequirement("MisGrants", "GlobalEditor")));
            });

            builder.Services.AddTransient<IAuthorizationHandler, JurayUniversal.Website.Filters.MisAuthorizationHandler>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;


            });


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/V2/AuthV2/Account/Login";
                options.LogoutPath = $"/V2/AuthV2/Account/Logout";
                options.AccessDeniedPath = $"/V2/AuthV2/Account/AccessDenied";
            });

            builder.Services.AddAuthentication()
                .AddCookie("ApplicantAuth", options =>
                {
                    options.LoginPath = "/MisRecruitment/Platform/Auth/Login";
                    options.LogoutPath = "/MisRecruitment/Platform/Auth/Logout";
                    options.AccessDeniedPath = "/MisRecruitment/Platform/Auth/Login";
                });
            builder.Services.AddTransient<PostmarkClient>(_ => new PostmarkClient(builder.Configuration.GetSection("PostmarkSettings")["ServerToken"]));

            builder.Services.AddHttpClient();

            // Payment services — each registered by concrete type so Checkout can use both
            builder.Services.AddHttpClient<JurayUniversal.Application.Services.PixelPayService>();
            builder.Services.AddHttpClient<JurayUniversal.Application.Services.PaystackService>();
            
            // Mass Comm SMS Scheduler Background Service
            builder.Services.AddHostedService<JurayUniversal.Application.Services.SmsSchedulerBackgroundService>();
            builder.Services.AddHostedService<JurayUniversal.Application.Services.NewsletterSchedulerBackgroundService>();

            // Email Background Worker for ZeptoMail Retries and Pruning
            builder.Services.AddHostedService<JurayUniversal.Application.Services.EmailBackgroundWorker>();

            var app = builder.Build();

           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                //app.Use(async (context, next) =>
                //{
                //    context.Items["RedirectTohttpswww"] = false;
                //    context.Items["RedirectTohttps"] = false;
                //    await next();
                //});
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                var host = context.Request.Host.Value;
                // Sanitize host for folder creation (Windows doesn't allow colons)
                host = host.Replace(":", "_");
                using (Serilog.Context.LogContext.PushProperty("Tenant", host))
                {
                    await next();
                }
            });

            //app.UseHttpsRedirectMiddleware();
            app.UseStaticFiles();
            app.UseTenantScopeMiddleware();
            //app.UseTokenMiddleware();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); // enables PixelPayWebhookController at POST /api/pixelpay-webhook
            app.MapRazorPages();

            // Seed MisFinances help content on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JurayUniversal.Persistence.EF.SQL.DashboardDbContext>();
            }

            app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}