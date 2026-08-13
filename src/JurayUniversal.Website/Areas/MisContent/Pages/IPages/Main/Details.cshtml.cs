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

namespace JurayUniversal.Website.Areas.MisContent.Pages.IPages.Main
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
            ViewData["TargetWebPageId"] = new SelectList(_context.WebPages.OrderBy(x => x.Title), "Id", "Title");

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

        public async Task<IActionResult> OnPostMoveSectionAsync(long sectionId, long targetWebPageId)
        {
            var section = await _context.PageSections.FindAsync(sectionId);
            if (section == null)
            {
                TempData["error"] = "Section not found.";
                return RedirectToPage();
            }

            var currentWebPageId = section.WebPageId;
            section.WebPageId = targetWebPageId;
            _context.Attach(section).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = "Section moved successfully.";
            return RedirectToPage("./Details", new { id = currentWebPageId });
        }

        public async Task<IActionResult> OnPostCopySectionAsync(long sectionId, long targetWebPageId)
        {
            var sourceSection = await _context.PageSections
                .Include(s => s.PageSectionLists)
                .FirstOrDefaultAsync(s => s.Id == sectionId);

            if (sourceSection == null)
            {
                TempData["error"] = "Source section not found.";
                return RedirectToPage();
            }

            var newSection = new PageSection
            {
                WebPageId = targetWebPageId,
                Title = sourceSection.Title != null ? $"{sourceSection.Title} (Copy)" : null,
                MiniTitle = sourceSection.MiniTitle,
                Qoute = sourceSection.Qoute,
                Description = sourceSection.Description,
                FullDescription = sourceSection.FullDescription,
                ButtonText = sourceSection.ButtonText,
                ButtonLink = sourceSection.ButtonLink,
                DirectUrl = sourceSection.DirectUrl,
                ShowInHome = sourceSection.ShowInHome,
                DisableButton = sourceSection.DisableButton,
                Disable = sourceSection.Disable,
                FixedAfterFooter = sourceSection.FixedAfterFooter,
                HomePageSortOrder = sourceSection.HomePageSortOrder,
                HomeSortFrom = sourceSection.HomeSortFrom,
                PageSortOrder = sourceSection.PageSortOrder,
                BlogSectionPosition = sourceSection.BlogSectionPosition,
                ShowSectionInBlog = sourceSection.ShowSectionInBlog,
                TemplateKey = sourceSection.TemplateKey,
                CustomClass = sourceSection.CustomClass,
                AddToRibon = sourceSection.AddToRibon,
                RibonCustomDisplayTitle = sourceSection.RibonCustomDisplayTitle,
                RibonSortOrder = sourceSection.RibonSortOrder,
                ImageUrl = sourceSection.ImageUrl,
                ImageKey = sourceSection.ImageKey,
                SecondImageUrl = sourceSection.SecondImageUrl,
                SecondImageKey = sourceSection.SecondImageKey,
                VideoUrl = sourceSection.VideoUrl,
                VideoKey = sourceSection.VideoKey,
                YoutubeUrlLink = sourceSection.YoutubeUrlLink
            };

            _context.PageSections.Add(newSection);
            await _context.SaveChangesAsync();

            if (sourceSection.PageSectionLists != null && sourceSection.PageSectionLists.Any())
            {
                foreach (var listItem in sourceSection.PageSectionLists)
                {
                    var newList = new PageSectionList
                    {
                        PageSectionId = newSection.Id,
                        Title = listItem.Title,
                        MiniTitle = listItem.MiniTitle,
                        Description = listItem.Description,
                        MoreDescription = listItem.MoreDescription,
                        ButtonText = listItem.ButtonText,
                        ButtonLink = listItem.ButtonLink,
                        DirectUrl = listItem.DirectUrl,
                        Disable = listItem.Disable,
                        SortOrder = listItem.SortOrder,
                        IconText = listItem.IconText,
                        ImageUrl = listItem.ImageUrl,
                        ImageKey = listItem.ImageKey,
                        VideoUrl = listItem.VideoUrl,
                        VideoKey = listItem.VideoKey,
                        AddToRibon = listItem.AddToRibon,
                        RibonCustomDisplayTitle = listItem.RibonCustomDisplayTitle,
                        RibonSortOrder = listItem.RibonSortOrder
                    };
                    _context.PageSectionLists.Add(newList);
                }
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Section copied successfully.";
            return RedirectToPage("./Details", new { id = sourceSection.WebPageId });
        }
    }
}
