
using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Dtos.AwsDtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages
{
    public class CareerModel : PageModel
    {

         private readonly ISettingsService _settingsService;
       
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public CareerModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
             _settingsService = settingsService;
        }


        [BindProperty]
        [Required]
        public IFormFile cvfile { get; set; }


        [BindProperty]
        [Required]
        public IFormFile appfile { get; set; }

        [BindProperty]
        [Required]
        public IFormFile certificatefile { get; set; }

        [BindProperty]
        [Required]
        public IFormFile passportfile { get; set; }
        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Setting = await _context.Settings.FirstOrDefaultAsync();
            var httpContext = HttpContext;
            VerificationWebDto setting = await _settingsService.ValidateWeb(httpContext);
            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }
            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);

            }

            SuperSetting = setting.SuperSetting;
            ViewData["CareerPath"] = new SelectList(_context.CareerPaths.OrderBy(x=>x.Title), "Id", "Title");

            return Page();
        }

        [BindProperty]
        public CareerFile CareerFile { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //cv
            if (cvfile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await cvfile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(cvfile.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new S3Object()
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        CareerFile.CvUrl = xresult.Url;
                        CareerFile.CvKey = xresult.Key;
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

            //cert
            if (appfile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await appfile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(appfile.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new S3Object()
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        CareerFile.ApplicationUrl = xresult.Url;
                        CareerFile.ApplicationKey = xresult.Key;
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
            //app
            if (certificatefile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await certificatefile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(certificatefile.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new S3Object()
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        CareerFile.CertificateUrl = xresult.Url;
                        CareerFile.CertificateKey = xresult.Key;
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
            //passport
            if (passportfile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await passportfile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(passportfile.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new S3Object()
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        CareerFile.PassportUrl = xresult.Url;
                        CareerFile.PassportKey = xresult.Key;
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

            _context.CareerFiles.Add(CareerFile);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";


            return RedirectToPage("./Submitted");
        }
    }
}
