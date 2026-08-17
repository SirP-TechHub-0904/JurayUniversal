using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.FileReconciliation
{
    [Authorize(Roles = "mSuperAdmin,Editor")]
    public class MoveFileModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;

        public MoveFileModel(DashboardDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public string Entity { get; set; }
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Property { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Key { get; set; }

        public string OldBucket { get; set; }
        public string NewBucket { get; set; }
        public string NewUrl { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string FileType { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Entity) || Id == 0 || string.IsNullOrEmpty(Property) || string.IsNullOrEmpty(Key))
            {
                Message = "Invalid parameters provided for file migration.";
                IsSuccess = false;
                return Page();
            }

            // Get buckets
            OldBucket = "juray2023";
            NewBucket = _context.SuperSettings.FirstOrDefault()?.BucketName ?? "juray2023";

            // Credentials
            var cred = new AwsCredentials()
            {
                AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                SecretKey = _config["AwsConfiguration:AWSSecretKey"]
            };

            // Move image in S3
            try
            {
                NewUrl = await _storageService.MoveImageAndGetPresignedUrlAsync(Key);
                Message = $"File '{Key}' was successfully migrated in S3 and pre-signed for bucket '{NewBucket}'.";
                IsSuccess = true;

                // Update DB entity with new Key/Url
                bool updated = await UpdateEntityFile(Entity, Id, Property, NewUrl, Key);
                if (!updated)
                {
                    Message += " (Note: DB record could not be updated directly; please verify property mapping.)";
                }
                else
                {
                    Message += " Database entity record has been updated with the new URL.";
                }

                // Determine file type for preview
                FileType = GetFileType(Key);
            }
            catch (Exception ex)
            {
                Message = $"Error moving file in AWS S3: {ex.Message}";
                IsSuccess = false;
            }

            return Page();
        }

        private async Task<bool> UpdateEntityFile(string entity, long id, string property, string newUrl, string key)
        {
            object obj = null;
            bool updated = false;
            switch (entity)
            {
                case "Blog":
                    obj = _context.Blogs.FirstOrDefault(x => x.Id == id);
                    break;
                case "Product":
                    obj = _context.Products.FirstOrDefault(x => x.Id == id);
                    break;
                case "ProductSample":
                    obj = _context.ProductSamples.FirstOrDefault(x => x.Id == id);
                    break;
                case "PageSection":
                    obj = _context.PageSections.FirstOrDefault(x => x.Id == id);
                    break;
                case "PageSectionList":
                    obj = _context.PageSectionLists.FirstOrDefault(x => x.Id == id);
                    break;
                case "WebPage":
                    obj = _context.WebPages.FirstOrDefault(x => x.Id == id);
                    break;
                case "Testimony":
                    obj = _context.Testimonies.FirstOrDefault(x => x.Id == id);
                    break;
                case "CompanyFund":
                    obj = _context.CompanyFunds.FirstOrDefault(x => x.Id == id);
                    break;
                case "CompayExpenses":
                    obj = _context.CompayExpenses.FirstOrDefault(x => x.Id == id);
                    break;
                case "CareerFile":
                    obj = _context.CareerFiles.FirstOrDefault(x => x.Id == id);
                    break;
                case "PortfolioTemplate":
                    obj = _context.PortfolioTemplates.FirstOrDefault(x => x.Id == id);
                    break;
                case "PostModal":
                    obj = _context.PostModal.FirstOrDefault(x => x.Id == id);
                    break;
                case "ProfilePortfolio":
                    obj = _context.ProfilePortfolios.FirstOrDefault(x => x.Id == id);
                    break;
                case "VoteCondidate":
                    obj = _context.VoteCondidates.FirstOrDefault(x => x.Id == id);
                    break;
                case "Setting":
                    obj = _context.Settings.FirstOrDefault(x => x.Id == id);
                    break;
                case "SuperSetting":
                    obj = _context.SuperSettings.FirstOrDefault(x => x.Id == id);
                    break;
                case "ProjectMain":
                    obj = _context.ProjectMains.FirstOrDefault(x => x.Id == id);
                    break;
                case "ProjectFile":
                    obj = _context.ProjectFiles.FirstOrDefault(x => x.Id == id);
                    break;
                case "ProposalFile":
                    obj = _context.ProposalFiles.FirstOrDefault(x => x.Id == id);
                    break;
                case "XProjectFile":
                    obj = _context.XProjectFiles.FirstOrDefault(x => x.Id == id);
                    break;
            }

            if (obj != null)
            {
                var t = obj.GetType();
                var urlProp = t.GetProperty($"{property}Url") ?? t.GetProperty($"{property}url");
                var keyProp = t.GetProperty($"{property}Key") ?? t.GetProperty($"{property}key");
                if (urlProp != null) { urlProp.SetValue(obj, newUrl); updated = true; }
                if (keyProp != null) { keyProp.SetValue(obj, key); updated = true; }
                if (updated) await _context.SaveChangesAsync();
            }
            return updated;
        }

        private string GetFileType(string key)
        {
            var ext = System.IO.Path.GetExtension(key ?? "")?.ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".webp") return "image";
            if (ext == ".mp4" || ext == ".mov" || ext == ".avi" || ext == ".webm") return "video";
            if (ext == ".pdf") return "pdf";
            if (ext == ".doc" || ext == ".docx") return "doc";
            return "file";
        }
    }
}
