using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayUniversal.Website.Areas.Content.Pages.IPages.Main
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Editor")]

    public class DetailsModel : PageModel
    {

        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }

        public WebPage WebPage { get; set; }

        [BindProperty]
        public PageSection PageSection { get; set; }



        [BindProperty]
        public IFormFile? imagefile2 { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }


        [BindProperty]
        public IFormFile? videofile { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WebPage = await _context.WebPages
                .Include(x => x.PageSections) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (WebPage == null)
            {
                return NotFound();
            }

            ViewData["TemplateTypeId"] = new SelectList(_context.TemplateTypes, "Id", "Name");
            ViewData["WebPageId"] = new SelectList(_context.WebPages, "Id", "Title");

            return Page();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnPostSection()
        {

            //image
            if (imagefile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefile.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        PageSection.ImageUrl = xresult.Url;
                        PageSection.ImageKey = xresult.Key;
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


            //image 2
            if (imagefile2 != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefile2.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefile2.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        PageSection.SecondImageUrl = xresult.Url;
                        PageSection.SecondImageKey = xresult.Key;
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



            //video
            if (videofile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await videofile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(videofile.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        PageSection.VideoUrl = xresult.Url;
                        PageSection.VideoKey = xresult.Key;
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

            _context.PageSections.Add(PageSection);
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = PageSection.WebPageId });
        }
         

        public async Task<IActionResult> OnPostSectionDelete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PageSection = await _context.PageSections.FindAsync(id);

            if (PageSection != null)
            {
                try
                {
                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var vidresult = await _storageService.DeleteObjectAsync(cred, await _storageService.BucketName(), PageSection.VideoKey);
                    var imgresult = await _storageService.DeleteObjectAsync(cred, await _storageService.BucketName(), PageSection.ImageKey);
                }
                catch (Exception c) { }

                _context.PageSections.Remove(PageSection);
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";
            }

            return RedirectToPage("./Details", new { id = PageSection.WebPageId });
        }

         
    }
}
