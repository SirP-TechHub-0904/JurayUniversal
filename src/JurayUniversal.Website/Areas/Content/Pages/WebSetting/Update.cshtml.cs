using JurayUniversal.Application.Dtos.AwsDtos;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Content.Pages.WebSetting
{
     [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin,Editor")]

    public class UpdateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public UpdateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }
        [BindProperty]
        public IFormFile? appStoreImage { get; set; }
        [BindProperty]
        public IFormFile? playStoreImage { get; set; }
        [BindProperty]
        public IFormFile? reloaderimage { get; set; }
        [BindProperty]
        public IFormFile? imagefilewhitecompany { get; set; }

        [BindProperty]
        public IFormFile? imagefilecompany { get; set; }
        [BindProperty]
        public IFormFile? imagefilecompanyicon { get; set; }
        [BindProperty]
        public IFormFile? imagetitlebg { get; set; }
        [BindProperty]
        public Setting Setting { get; set; }
        [BindProperty]
        public SuperSetting SuperSetting { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
             
            Setting = await _context.Settings.FirstOrDefaultAsync();

            if (Setting == null)
            {
                return NotFound();
            }
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();

            if (SuperSetting == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var updatesetting = await _context.Settings.FirstOrDefaultAsync();
            var superupdate = await _context.SuperSettings.FirstOrDefaultAsync();
            //image
            if (imagefilecompany != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefilecompany.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefilecompany.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, superupdate.CompanyLogoKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        superupdate.CompanyLogoUrl = xresult.Url;
                        superupdate.CompanyIconKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }

            if (imagefilewhitecompany != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefilewhitecompany.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefilewhitecompany.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, superupdate.CompanyWhiteLogoKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        superupdate.CompanyWhiteLogoUrl = xresult.Url;
                        superupdate.CompanyWhiteLogoKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }

            if (imagefilecompanyicon != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefilecompanyicon.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefilecompanyicon.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, superupdate.CompanyIconKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        superupdate.CompanyIconUrl = xresult.Url;
                        superupdate.CompanyIconKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }
            if (imagetitlebg != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagetitlebg.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagetitlebg.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, updatesetting.DefaultTitleBackgroundKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        updatesetting.DefaultTitleBackgroundUrl = xresult.Url;
                        updatesetting.DefaultTitleBackgroundKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }
            ////
            ///
            if (reloaderimage != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await reloaderimage.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(reloaderimage.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, superupdate.ReloaderIconKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        superupdate.ReloaderIconUrl = xresult.Url;
                        superupdate.ReloaderIconKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }
            if (appStoreImage != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await appStoreImage.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(appStoreImage.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.AppStoreImageKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.AppStoreImageUrl = xresult.Url;
                        SuperSetting.AppStoreImageKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }

            if (playStoreImage != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await playStoreImage.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(playStoreImage.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.PlayStoreImageKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.PlayStoreImageUrl = xresult.Url;
                        SuperSetting.PlayStoreImageKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }

            ///
            superupdate.CompanyName = SuperSetting.CompanyName;
            superupdate.CompanyDescription = SuperSetting.CompanyDescription;
            superupdate.CompanyWebsiteLink = SuperSetting.CompanyWebsiteLink;
            superupdate.WebsiteTitle = SuperSetting.WebsiteTitle;
            superupdate.ProductTitle = SuperSetting.ProductTitle;
            superupdate.ProductTitleHome = SuperSetting.ProductTitleHome;
            superupdate.SpecialMenuButtonActivate = SuperSetting.SpecialMenuButtonActivate;
            superupdate.SpecialMenuButtonLink = SuperSetting.SpecialMenuButtonLink;
            superupdate.SpecialMenuButtonText = SuperSetting.SpecialMenuButtonText;


            superupdate.ActivateWorkWithUsFooter = SuperSetting.ActivateWorkWithUsFooter;
            superupdate.ActivateDownloadOurAppFooter = SuperSetting.ActivateDownloadOurAppFooter;
            superupdate.TitleDownloadOurAppFooter = SuperSetting.TitleDownloadOurAppFooter;
            superupdate.DescriptionDownloadOurAppFooter = SuperSetting.DescriptionDownloadOurAppFooter;
            superupdate.AppStoreLink = SuperSetting.AppStoreLink;
            superupdate.PlayStoreLink = SuperSetting.PlayStoreLink;
            superupdate.ActivateSubscribeFormOnFooter = SuperSetting.ActivateSubscribeFormOnFooter; 
 

            updatesetting.Qoute = Setting.Qoute;
            updatesetting.TopNote = Setting.TopNote;
            updatesetting.FacebookPage = Setting.FacebookPage;
            updatesetting.InstagramPage = Setting.InstagramPage;
            updatesetting.TwitterPage = Setting.TwitterPage;
            updatesetting.TiktokPage = Setting.TiktokPage;
            updatesetting.YoutubeChannel = Setting.YoutubeChannel;
            updatesetting.WorkingHour = Setting.WorkingHour;
            updatesetting.AddressOne = Setting.AddressOne;
            updatesetting.AddressTwo = Setting.AddressTwo;
            updatesetting.EmailOne = Setting.EmailOne;
            updatesetting.ShowEmailOneInTop = Setting.ShowEmailOneInTop;
            updatesetting.ShowEmailOneInFooter = Setting.ShowEmailOneInFooter;
            updatesetting.EmailTwo = Setting.EmailTwo;
            updatesetting.ShowEmailTwoInTop = Setting.ShowEmailTwoInTop;
            updatesetting.ShowEmailTwoInFooter = Setting.ShowEmailTwoInFooter;
            updatesetting.EmailThree = Setting.EmailThree;
            updatesetting.ShowEmailThreeInTop = Setting.ShowEmailThreeInTop;
            updatesetting.ShowEmailThreeInFooter = Setting.ShowEmailThreeInFooter;
            updatesetting.PhoneOne = Setting.PhoneOne;
            updatesetting.ShowPhoneOneInTop = Setting.ShowPhoneOneInTop;
            updatesetting.ShowPhoneOneInFooter = Setting.ShowPhoneOneInFooter;
            updatesetting.PhoneTwo = Setting.PhoneTwo;
            updatesetting.ShowPhoneTwoInTop = Setting.ShowPhoneTwoInTop;
            updatesetting.ShowPhoneTwoInFooter = Setting.ShowPhoneTwoInFooter;
            updatesetting.PhoneThree = Setting.PhoneThree;
            updatesetting.ShowPhoneThreeInTop = Setting.ShowPhoneThreeInTop;
            updatesetting.ShowPhoneThreeInFooter = Setting.ShowPhoneThreeInFooter;
            updatesetting.GoogleMap = Setting.GoogleMap;
            updatesetting.ShowContactUsMenu = Setting.ShowContactUsMenu;
            updatesetting.ShowContactUsFooter = Setting.ShowContactUsFooter;
            updatesetting.ShowAddressOneInTop = Setting.ShowAddressOneInTop;
            updatesetting.AddBlogToFooter = Setting.AddBlogToFooter;
            updatesetting.AddBlogToHome = Setting.AddBlogToHome;
            updatesetting.AddBlogToMenu = Setting.AddBlogToMenu;
            updatesetting.BlogDisplayTitle = Setting.BlogDisplayTitle;

               updatesetting.AddCareerToFooter = Setting.AddCareerToFooter; 
            updatesetting.AddCareerToMenu = Setting.AddCareerToMenu;
            updatesetting.CareerDisplayTitle = Setting.CareerDisplayTitle;


            updatesetting.AddFaqToHome = Setting.AddFaqToHome;
            updatesetting.AddFaqToFooter = Setting.AddFaqToFooter;
            updatesetting.AddTestimonyToHome = Setting.AddTestimonyToHome;
            updatesetting.AddTestimonyToFooter = Setting.AddTestimonyToFooter;
            updatesetting.DisableMainTopMenu = Setting.DisableMainTopMenu;
            updatesetting.ShowProductsInMenu = Setting.ShowProductsInMenu;
            updatesetting.ShowProductsInFooter = Setting.ShowProductsInFooter;
            updatesetting.ActivateProductsInHome = Setting.ActivateProductsInHome;
            updatesetting.ShowTwoProducts = Setting.ShowTwoProducts;
            updatesetting.ShowThreeProducts = Setting.ShowThreeProducts;
            updatesetting.ShowSixProducts = Setting.ShowSixProducts; 
            updatesetting.EnableBreakingNewsRibon = Setting.EnableBreakingNewsRibon; 
            updatesetting.RibonPosition = Setting.RibonPosition;
            updatesetting.BreakingNewsRibonTitle = Setting.BreakingNewsRibonTitle;
            try
            {
                _context.Attach(updatesetting).State = EntityState.Modified; 
                await _context.SaveChangesAsync();
            }
            catch(Exception g) { }
            try
            {
                _context.Attach(superupdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception g) { }
               
                TempData["success"] = "Successful";
           

            return RedirectToPage("./Index");
        }

         
    }
}
