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

namespace JurayUniversal.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services
                .AddTenantSupport(builder.Configuration)
                .AddEntityFrameworkSqlServer<DashboardDbContext>();

            builder.Services.AddIdentity<Profile, IdentityRole>()
                        .AddEntityFrameworkStores<DashboardDbContext>()
                        .AddDefaultTokenProviders();

            builder.Services.AddRazorPages();
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
            builder.Services.AddSingleton<IServiceProvider>(provider => provider);
            // Add registration for LoggerLibrary
 
            builder.Services.AddSingleton<LoggerLibrary>(provider =>
            {
                var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
                var wwwRootPath = hostingEnvironment.WebRootPath;
                return new LoggerLibrary(wwwRootPath);
            });

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("MaxUserPolicy", policy =>
            //    {
            //        //policy.Requirements.Add(new MaxUserRequirement(builder.Services.BuildServiceProvider().GetRequiredService<ISettingsService>().GetMaxUserCount().Result));
            //    });
            //});

            //builder.Services.AddTransient<IAuthorizationHandler, MaxUserHandler>();
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
            builder.Services.AddTransient<PostmarkClient>(_ => new PostmarkClient(builder.Configuration.GetSection("PostmarkSettings")["ServerToken"]));

            builder.Services.AddHttpClient();

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
            //app.UseHttpsRedirectMiddleware();
            app.UseStaticFiles();
            app.UseTenantScopeMiddleware();
            //app.UseTokenMiddleware();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}