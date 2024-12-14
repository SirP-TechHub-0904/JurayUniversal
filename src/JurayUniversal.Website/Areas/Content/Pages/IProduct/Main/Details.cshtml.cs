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

namespace JurayUniversal.Website.Areas.Content.Pages.IProduct.Main
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

        public Product Product { get; set; }

        [BindProperty]
        public ProductSample ProductSample { get; set; }

        [BindProperty]
        public ProductFeature ProductFeature { get; set; }

       

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

            Product = await _context.Products
                .Include(x => x.ProductFeatures)
                .Include(x => x.ProductSamples)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
       
        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnPostSample()
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
                        ProductSample.ImageUrl = xresult.Url;
                        ProductSample.ImageKey = xresult.Key;
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
                        ProductSample.VideoUrl = xresult.Url;
                        ProductSample.VideoKey = xresult.Key;
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

            _context.ProductSamples.Add(ProductSample);
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = ProductSample.ProductId });
        }

        public async Task<IActionResult> OnPostSampleUpdate()
        {


            _context.Attach(ProductSample).State = EntityState.Modified;
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = ProductSample.ProductId });
        }

        public async Task<IActionResult> OnPostSampleDelete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductSample = await _context.ProductSamples.FindAsync(id);

            if (ProductSample != null)
            {
                try
                {
                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var vidresult = await _storageService.DeleteObjectAsync(cred, await _storageService.BucketName(), ProductSample.VideoKey);
                    var imgresult = await _storageService.DeleteObjectAsync(cred, await _storageService.BucketName(), ProductSample.ImageKey);
                }
                catch (Exception c) { }

                _context.ProductSamples.Remove(ProductSample);
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";
            }

            return RedirectToPage("./Details", new { id = ProductSample.ProductId });
        }


        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnPostFeatures()
        {
            _context.ProductFeatures.Add(ProductFeature);
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = ProductFeature.ProductId });
        }

        public async Task<IActionResult> OnPostFeaturesUpdate()
        {


            _context.Attach(ProductFeature).State = EntityState.Modified;
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = ProductFeature.ProductId });
        }

        public async Task<IActionResult> OnPostFeaturesDelete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductFeature = await _context.ProductFeatures.FindAsync(id);

            if (ProductFeature != null)
            {
                _context.ProductFeatures.Remove(ProductFeature);
                await _context.SaveChangesAsync(); 
                TempData["success"] = "Successful";
            }

            return RedirectToPage("./Details", new { id = ProductFeature.ProductId });
        }
    }
}
