using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.Management.Pages.V3
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]

    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }
        [BindProperty]
        public IFormFile? appStoreImage { get; set; }
        [BindProperty]
        public IFormFile? playStoreImage { get; set; }
        [BindProperty]
        public IFormFile? reloaderimage { get; set; }
        [BindProperty]
        public IFormFile? imagelogin { get; set; }

        [BindProperty]
        public IFormFile? imagefilewhitecompany { get; set; }
        [BindProperty]
        public SuperSetting SuperSetting { get; set; }
        [BindProperty]
        public IFormFile? imagefilecompany { get; set; }
        [BindProperty]
        public IFormFile? imagefilecompanyicon { get; set; }

        [BindProperty]
        public IFormFile? imagefilereloadicon { get; set; }
        [BindProperty]
        public IFormFile? imageport1 { get; set; }
        [BindProperty]
        public IFormFile? imageport2 { get; set; }
         [BindProperty]
        public IFormFile? imageport3 { get; set; }

        [BindProperty]
        public IFormFile? encrytionupload { get; set; }

        [BindProperty]
        public DataConfig DataConfig { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SuperSetting = await _context.SuperSettings.AsNoTracking()
                    .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (SuperSetting == null)
            {
                return NotFound();
            }
                          DataConfig = await _context.DataConfigs.FirstOrDefaultAsync();
            ViewData["WebPageId"] = new SelectList(_context.WebPages, "Id", "Title");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
           
            if (imagelogin != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagelogin.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagelogin.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.LoginBackgroundImageKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.LoginBackgroundImageUrl = xresult.Url;
                        SuperSetting.LoginBackgroundImageKey = xresult.Key;
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.CompanyLogoKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.CompanyLogoUrl = xresult.Url;
                        SuperSetting.CompanyLogoKey = xresult.Key;
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.CompanyWhiteLogoKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.CompanyWhiteLogoUrl = xresult.Url;
                        SuperSetting.CompanyWhiteLogoKey = xresult.Key;
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.CompanyIconKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.CompanyIconUrl = xresult.Url;
                        SuperSetting.CompanyIconKey = xresult.Key;
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

            if (imageport1 != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imageport1.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imageport1.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.PortfolioImageOneKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.PortfolioImageOneUrl = xresult.Url;
                        SuperSetting.PortfolioImageOneKey = xresult.Key;
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

            if (imageport2 != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imageport2.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imageport2.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.PortfolioImageTwoKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.PortfolioImageTwoUrl = xresult.Url;
                        SuperSetting.PortfolioImageTwoKey = xresult.Key;
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

            if (imageport3 != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imageport3.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imageport3.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.PortfolioBreacrumImageKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.PortfolioBreacrumImageUrl = xresult.Url;
                        SuperSetting.PortfolioBreacrumImageKey = xresult.Key;
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.ReloaderIconKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.ReloaderIconUrl = xresult.Url;
                        SuperSetting.ReloaderIconKey = xresult.Key;
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
            //
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
            if (imagefilereloadicon != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefilereloadicon.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefilereloadicon.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.ReloaderIconKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.ReloaderIconUrl = xresult.Url;
                        SuperSetting.ReloaderIconKey = xresult.Key;
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
            if (encrytionupload != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await encrytionupload.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(encrytionupload.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, SuperSetting.EncryptionKey);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        SuperSetting.EncryptionUrl = xresult.Url;
                        SuperSetting.EncryptionKey = xresult.Key;
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

            _context.Attach(SuperSetting).State = EntityState.Modified;


            //           var dataConfig = await _context.DataConfigs.FirstOrDefaultAsync();

            //dataConfig.DashboardCSS = DataConfig.DashboardCSS;
            //dataConfig.LayoutCSS = DataConfig.LayoutCSS;
            //dataConfig.LoginCSS = DataConfig.LoginCSS;
            //dataConfig.Configuration = DataConfig.Configuration;
            //dataConfig.RedirectTohttpswww = DataConfig.RedirectTohttpswww;
            //dataConfig.RedirectTohttps = DataConfig.RedirectTohttps;
            //dataConfig.LiveConfiguration = DataConfig.LiveConfiguration;

            //            _context.Attach(dataConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuperSettingExists(SuperSetting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SuperSettingExists(long id)
        {
            return _context.SuperSettings.Any(e => e.Id == id);
        }
    }
}
