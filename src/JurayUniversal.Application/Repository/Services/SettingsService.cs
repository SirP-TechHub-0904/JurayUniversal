using Amazon.Runtime.Internal;
using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.DomainServices;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IDomainRepository _domainRepository;
        DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        public SettingsService(IConfiguration configuration, DashboardDbContext context, UserManager<Profile> userManager, IDomainRepository domainRepository, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _domainRepository = domainRepository;
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
        }

        public async Task<bool> GetMaxUserCount()
        {
            // Retrieve the value from the database or any other data source
            // Example: Assuming the value is stored in a column named 'MaxUserCount' of a table named 'Settings'
            //var xmaxUserCount = await _context.SuperSettings.FirstOrDefaultAsync();
            var maxUserCount = await _context.SuperSettings.FirstOrDefaultAsync();
            var UserCount = maxUserCount.MaximumUsers;
            var excludedRoles = new[] { "mSuperAdmin", "SubAdmin", "finance" };

            var excludedUserCount = 0;
            foreach (var role in excludedRoles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                excludedUserCount += usersInRole.Count;
            }

            var totalUserCount = _userManager.Users.Count() - excludedUserCount;


            if (totalUserCount <= UserCount)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<VerificationWebDto> ValidateWeb(HttpContext httpContext)
        {
            VerificationWebDto response = new VerificationWebDto();
            var setting = await _context.Settings.FirstOrDefaultAsync();
            //if (setting == null)
            //{
            //    response.SettingFound = false;
            //    response.Path = "/AuthVadmin/Readonly";
            //    response.Area = "V2";
            //    return response;
            //}
            var superSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            if (superSetting == null)
            {
                response.SettingFound = false;
                response.Path = "/AuthVadmin/Readonly";
                response.Area = "V2";
                return response;
            }
            else
            {
                response.SuperSetting = superSetting;
                response.Setting = setting;
                response.SettingFound = true;
            }
            if (superSetting.ActivateProfilePortfolio == true)
            {
                bool verifyportfoliosetting = MainServices.VerifyPortfolio(superSetting.VerifyTokenFolio);
                if (verifyportfoliosetting == true)
                {
                    string host = httpContext.Request.Host.ToString();
                    string fullUri = httpContext.Request.GetEncodedUrl();
                    var check = await _context.ProfilePortfolioSetting
                        .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host || !x.UseUpgradeWebLink && x.DefaultWebLink == host);
                    if (check != null)
                    {
                        //string targetUrl = $"{httpContext.Request.Scheme}://{host}/Request";

                        //if (fullUri.StartsWith(targetUrl, StringComparison.OrdinalIgnoreCase))
                        //{ 
                        //    response.Path = "/AuthVadmin/Readonly";
                        //    response.Area = "V2";
                        //    return response;
                        //}
                        //else
                        //{

                        response.Portfolio = true;
                        response.PortfolioPath = "./Profile";
                        return response;
                        //}
                    }
                }
            }
            response.SuperSetting = superSetting;
            response.Setting = setting;
            return response;
        }

        public async Task<VerificationWebDto> ContainPortfolioValidateWeb(HttpContext httpContext)
        {
            VerificationWebDto response = new VerificationWebDto();
            var setting = await _context.Settings.FirstOrDefaultAsync();
            var superSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            if (superSetting == null)
            {
                response.SettingFound = false;
                response.Path = "/AuthVadmin/Readonly";
                response.Area = "V2";
                return response;
            }
            else
            {
                response.SuperSetting = superSetting;
                response.Setting = setting;
                response.SettingFound = true;
            }
            if (superSetting.ActivateProfilePortfolio == true)
            {
                bool verifyportfoliosetting = MainServices.VerifyPortfolio(superSetting.VerifyTokenFolio);
                if (verifyportfoliosetting == true)
                {
                    string host = httpContext.Request.Host.ToString();
                    string fullUri = httpContext.Request.GetEncodedUrl();
                    var check = await _context.ProfilePortfolioSetting
                        .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host || !x.UseUpgradeWebLink && x.DefaultWebLink == host);
                    if (check != null)
                    {
                        //string targetUrl = $"{httpContext.Request.Scheme}://{host}/Request";

                        //if (fullUri.StartsWith(targetUrl, StringComparison.OrdinalIgnoreCase))
                        //{ 
                        //    response.Path = "/AuthVadmin/Readonly";
                        //    response.Area = "V2";
                        //    return response;
                        //}
                        //else
                        //{

                        response.Portfolio = true;
                        response.PortfolioPath = "./Profile";
                        return response;
                        //}
                    }
                }
            }
            response.SuperSetting = superSetting;
            response.Setting = setting;
            return response;
        }

        public async Task<bool> CreateAccount(PortfolioAccountSetupDto data, HttpContext httpContext)
        {
            var user = new Profile
            {
                FirstName = data.FirstName,
                MiddleName = data.MiddleName,
                LastName = data.LastName,
                Email = data.Email,
                UserName = data.Email,
                PhoneNumber = data.Phone,
                PortfolioProfile = true
            };
            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                ProfilePortfolioSetting newdata = new ProfilePortfolioSetting();
                newdata.ProfileId = user.Id;
                newdata.DefaultWebLink = data.ProfileDomain;
                newdata.RegDate = DateTime.UtcNow.AddHours(1);
                newdata.TimeFrame = 60;
                newdata.Token = Guid.NewGuid().ToString();
                _context.ProfilePortfolioSetting.Add(newdata);
                await _context.SaveChangesAsync();

                string host = httpContext.Request.Host.ToString();
                ///
                DomainDataDto newdomin = new DomainDataDto();
                newdomin.Date = DateTime.Now;
                newdomin.Domain = data.ProfileDomain;
                newdomin.BaseCompanyUrl = host;

                bool dataresult = await _domainRepository.AddDomain(newdomin);
                if (dataresult != true)
                {
                    return false;
                }
                //create link... 
                string activationToken = newdata.Token;
                string area = "IPortfolio";
                string page = "/Account/Index";

                // Create an instance of IUrlHelper using IUrlHelperFactory.
                var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

                // Generate the activation link URL.
                string activationUrl = urlHelper.Page(page, new { area = area, token = activationToken });
                string flink = "http://" + newdata.DefaultWebLink + activationUrl;


                return true;
            }
            return false;

        }

        public async Task<VerificationWebDto> ProfileData(HttpContext httpContext)
        {
            VerificationWebDto response = new VerificationWebDto();
            var setting = await _context.Settings.FirstOrDefaultAsync();
            var superSetting = await _context.SuperSettings.FirstOrDefaultAsync();

            string host = httpContext.Request.Host.ToString();
            string fullUri = httpContext.Request.GetEncodedUrl();
            var check = await _context.ProfilePortfolioSetting.Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host || !x.UseUpgradeWebLink && x.DefaultWebLink == host);
            if (check == null)
            {

                response.PortfolioPath = "///";
                return response;
                //}
            }

            response.ProfilePortfolioSetting = check;

            return response;
        }

        public async Task<PortfolioDto> GetPortfolioAccount(string accesstoken, HttpContext httpContext)
        {
            PortfolioDto response = new PortfolioDto();
            string host = httpContext.Request.Host.ToString();

            var check = await _context.ProfilePortfolioSetting.Include(x => x.Profile)
                       .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host || !x.UseUpgradeWebLink && x.DefaultWebLink == host);
            if (check != null)
            {
                response.result = true;
                response.ProfilePortfolioSetting = check;
                response.Profile = check.Profile;

                if (!String.IsNullOrEmpty(accesstoken))
                {
                    var verify = await _context.ProfilePortfolioSetting
                        .FirstOrDefaultAsync(x => x.Token == accesstoken && x.FirstTime == false);
                    if (verify != null)
                    {
                        response.FirstTime = true;
                    }
                    else
                    {
                        response.FirstTime = false;
                    }
                }
                else
                {
                    response.FirstTime = false;
                }
                if (response.FirstTime == false)
                {
                    if (httpContext.User.Identity.IsAuthenticated)
                    {
                        response.IsAuthenticated = true;
                    }
                }
            }
            else
            {
                response.result = false;
            }

            return response;
        }

        public async Task<PortfolioNameAndImageDto> GetPortfolioNameAndPassport(HttpContext httpContext)
        {
            PortfolioNameAndImageDto response = new PortfolioNameAndImageDto();
            string host = httpContext.Request.Host.ToString();

            var check = await _context.ProfilePortfolioSetting.Include(x => x.Profile)
                       .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host || !x.UseUpgradeWebLink && x.DefaultWebLink == host);
            if (check != null)
            {
                string textdescription = MainServices.RemoveHtmlAndCss(check.Profile.Biography);
                response.Description = textdescription;
                response.Fullname = check.Profile.GetFullName();
                response.Image = check.Profile.PassportFilePathUrl;

                if (check.UseUpgradeWebLink == true)
                {
                    response.Website = "http://" + check.UpgradeWebLink;
                }
                else
                {
                    response.Website = "http://" + check.DefaultWebLink;
                }
                var contactus = _context.PortfolioContactUses.Where(x => x.ProfileId == check.ProfileId).OrderByDescending(x => x.Date).AsQueryable();
                response.PortfolioContactUs = await contactus.Take(4).ToListAsync();
                response.ContactMeCount = await contactus.CountAsync();

            }
            return response;
        }

        public async Task<bool> AddDemo(HttpContext httpContext)
        {
            string host = httpContext.Request.Host.ToString();
            var check = await _context.ProfilePortfolioSetting.Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.UseUpgradeWebLink && x.UpgradeWebLink == host || !x.UseUpgradeWebLink && x.DefaultWebLink == host);
            if (check == null)
            {
                var user = await _userManager.FindByIdAsync(check.ProfileId);
                user.Address = "37, Karo, FCT, Abuja";
                user.Biography = "Hello, I’m a john, web-developer based on Abuja. I have rich experience in web site design & building and customization. Also I am good at mobile app development";
                await _userManager.UpdateAsync(user);

                AdditionalProfile addpro1 = new AdditionalProfile
                {
                    ProfileId = user.Id,
                    Title = "Website",
                    Description = host
                };
                _context.AdditionalProfiles.Add(addpro1);

                // Create and add the second AdditionalProfile
                AdditionalProfile addpro2 = new AdditionalProfile
                {
                    ProfileId = user.Id,
                    Title = "Country",
                    Description = "Nigeria"
                };
                _context.AdditionalProfiles.Add(addpro2);

                //
                ProfileCategory c1 = new ProfileCategory();
                c1.ProfileId = user.Id;
                c1.Title = "What I do";

                _context.ProfileCategories.Add(c1);
                //
                ProfileCategory c2 = new ProfileCategory();

                c2.ProfileId = user.Id;
                c2.Title = "Technical Skills";

                _context.ProfileCategories.Add(c2);
                //
                ProfileCategory c3 = new ProfileCategory();

                c3.ProfileId = user.Id;
                c3.Title = "Professional Skills";

                _context.ProfileCategories.Add(c3);
                //
                ProfileCategory c4 = new ProfileCategory();

                c4.ProfileId = user.Id;
                c4.Title = "Education";

                _context.ProfileCategories.Add(c4);
                //
                ProfileCategory c5 = new ProfileCategory();

                c5.ProfileId = user.Id;
                c5.Title = "Work Experience";

                _context.ProfileCategories.Add(c5);

                 //
                ProfileCategory c6 = new ProfileCategory();

                c6.ProfileId = user.Id;
                c6.Title = "Portfolio";

                _context.ProfileCategories.Add(c6);
                await _context.SaveChangesAsync();
                /////
                ///
                ProfileCategoryList l1 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c1.Id,
                    Title = "UI Design",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.",

                };
                _context.ProfileCategoryLists.Add(l1);
                ProfileCategoryList l2 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c1.Id,
                    Title = "Web Development",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.",

                };
                _context.ProfileCategoryLists.Add(l2);

                ProfileCategoryList l3 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c1.Id,
                    Title = "App Development",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.",

                };
                _context.ProfileCategoryLists.Add(l3);
                //
                ProfileCategoryList l4 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c2.Id,
                    Title = "Javascript",
                    Description = "70",

                };
                _context.ProfileCategoryLists.Add(l4);
                //
                ProfileCategoryList l5 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c2.Id,
                    Title = "Java",
                    Description = "40",

                };
                _context.ProfileCategoryLists.Add(l5);
                //
                ProfileCategoryList l6 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c2.Id,
                    Title = "Python",
                    Description = "40",

                };
                _context.ProfileCategoryLists.Add(l6);
                //
                ProfileCategoryList l7 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c2.Id,
                    Title = "PHP",
                    Description = "40",

                };
                _context.ProfileCategoryLists.Add(l7);
                //
                ProfileCategoryList l8 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c3.Id,
                    Title = "Communication",
                    Description = "40",

                };
                _context.ProfileCategoryLists.Add(l8);
                //
                ProfileCategoryList l9 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c3.Id,
                    Title = "Team Work",
                    Description = "90",

                };
                _context.ProfileCategoryLists.Add(l9);
                //
                ProfileCategoryList l10 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c3.Id,
                    Title = "Project Management",
                    Description = "30",

                };
                _context.ProfileCategoryLists.Add(l10);
                //
                ProfileCategoryList l11 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c3.Id,
                    Title = "Creativity",
                    Description = "40",

                };
                _context.ProfileCategoryLists.Add(l11);

                //
                ProfileCategoryList l12 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c4.Id,
                    Title = "Art & Multimedia",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Oxford University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l12);
                 //
                ProfileCategoryList l13 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c4.Id,
                    Title = "Art & Multimedia",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Britain University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l13);
                   //
                ProfileCategoryList l14 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c4.Id,
                    Title = "Art & Multimedia",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Britain University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l14);

                   //
                ProfileCategoryList l15 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c5.Id,
                    Title = "UI/UX Designer",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Oxford University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l15);
                 //
                ProfileCategoryList l16 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c5.Id,
                    Title = "Art & Multimedia",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Britain University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l16);
                   //
                ProfileCategoryList l17 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c5.Id,
                    Title = "Art & Multimedia",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Britain University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l17);

                //
                ProfileCategoryList l18 = new ProfileCategoryList
                {
                    ProfileId = user.Id,
                    ProfileCategoryId = c4.Id,
                    Title = "Art & Multimedia",
                    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                    Employer = "Britain University",                   
                    StartDate = "2005",
                    EndDate = "2008",
                    Currently = false
                };
                _context.ProfileCategoryLists.Add(l17);

                ////
                //ProfileCategoryList l17 = new ProfileCategoryList
                //{
                //    ProfileId = user.Id,
                //    ProfileCategoryId = c4.Id,
                //    Title = "Art & Multimedia",
                //    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                //    Employer = "Britain University",                   
                //    StartDate = "2005",
                //    EndDate = "2008",
                //    Currently = false
                //};
                //_context.ProfileCategoryLists.Add(l17);

                ////
                //ProfileCategoryList l17 = new ProfileCategoryList
                //{
                //    ProfileId = user.Id,
                //    ProfileCategoryId = c4.Id,
                //    Title = "Art & Multimedia",
                //    Description = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum",
                //    Employer = "Britain University",                   
                //    StartDate = "2005",
                //    EndDate = "2008",
                //    Currently = false
                //};
                _context.ProfileCategoryLists.Add(l17);
            }

            return false;
        }

        public async Task<bool> UseHttpsWWW()
        {
            var settings = await _context.DataConfigs.FirstOrDefaultAsync();
            if(settings != null)
            {
                if(settings.RedirectTohttpswww == true)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UseHttps()
        {
            var settings = await _context.DataConfigs.FirstOrDefaultAsync();
            if (settings != null)
            {
                if (settings.RedirectTohttps == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
