using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using JurayUniversal.Application.Services.AWS;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.MisContent.Pages.IPages.Section
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
        [BindProperty]
        public PageSectionList PageSectionList { get; set; }

       
        public PageSection PageSection { get; set; }



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

            PageSection = await _context.PageSections
                .Include(x => x.PageSectionLists)
                .Include(x => x.WebPage)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (PageSection == null)
            {
                return NotFound();
            }
            ViewData["WebPageId"] = new SelectList(_context.WebPages, "Id", "Title");
            ViewData["TargetPageSections"] = new SelectList(
                await _context.PageSections
                    .Include(p => p.WebPage)
                    .OrderBy(p => p.WebPage.Title)
                    .ThenBy(p => p.Title)
                    .Select(p => new {
                        Id = p.Id,
                        DisplayName = (p.WebPage != null ? p.WebPage.Title + " -> " : "") + (p.Title ?? "Untitled Section")
                    })
                    .ToListAsync(),
                "Id",
                "DisplayName"
            );

            return Page();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnPostList()
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
                        PageSectionList.ImageUrl = xresult.Url;
                        PageSectionList.ImageKey = xresult.Key;
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
                        PageSectionList.VideoUrl = xresult.Url;
                        PageSectionList.VideoKey = xresult.Key;
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

            _context.PageSectionLists.Add(PageSectionList);
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = PageSectionList.PageSectionId });
        }

        public async Task<IActionResult> OnPostMoveListItemAsync(long listId, long targetSectionId)
        {
            var item = await _context.PageSectionLists.FindAsync(listId);
            if (item == null)
            {
                TempData["error"] = "List item not found.";
                return RedirectToPage();
            }

            var currentSectionId = item.PageSectionId;
            item.PageSectionId = targetSectionId;
            _context.Attach(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = "List item moved successfully.";
            return RedirectToPage("./Details", new { id = currentSectionId });
        }

        public async Task<IActionResult> OnPostCopyListItemAsync(long listId, long targetSectionId)
        {
            var sourceItem = await _context.PageSectionLists.FindAsync(listId);
            if (sourceItem == null)
            {
                TempData["error"] = "Source list item not found.";
                return RedirectToPage();
            }

            var newItem = new PageSectionList
            {
                PageSectionId = targetSectionId,
                Title = sourceItem.Title != null ? $"{sourceItem.Title} (Copy)" : null,
                MiniTitle = sourceItem.MiniTitle,
                Description = sourceItem.Description,
                MoreDescription = sourceItem.MoreDescription,
                ButtonText = sourceItem.ButtonText,
                ButtonLink = sourceItem.ButtonLink,
                DirectUrl = sourceItem.DirectUrl,
                Disable = sourceItem.Disable,
                SortOrder = sourceItem.SortOrder,
                IconText = sourceItem.IconText,
                ImageUrl = sourceItem.ImageUrl,
                ImageKey = sourceItem.ImageKey,
                VideoUrl = sourceItem.VideoUrl,
                VideoKey = sourceItem.VideoKey,
                AddToRibon = sourceItem.AddToRibon,
                RibonCustomDisplayTitle = sourceItem.RibonCustomDisplayTitle,
                RibonSortOrder = sourceItem.RibonSortOrder
            };

            _context.PageSectionLists.Add(newItem);
            await _context.SaveChangesAsync();

            TempData["success"] = "List item copied successfully.";
            return RedirectToPage("./Details", new { id = sourceItem.PageSectionId });
        }
    }
}
