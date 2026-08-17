using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;
using Microsoft.Extensions.Configuration;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.FileReconciliation
{
    public class FileEntryDto
    {
        public string EntityName { get; set; }
        public long EntityId { get; set; }
        public string PropertyType { get; set; } // "Image", "Video", "Document", "Receipt", etc.
        public string Url { get; set; }
        public string Key { get; set; }
        public string BucketName { get; set; }
        public string PropertyName { get; set; } // For updating
        public string CategoryGroup { get; set; } // "Web Content", "Media", "Documents", "Settings"
    }

    [Authorize(Roles = "mSuperAdmin,Editor")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;

        public IndexModel(DashboardDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }

        public List<FileEntryDto> FileEntries { get; set; } = new();

        public int TotalFiles => FileEntries.Count;
        public int TotalImages => FileEntries.Count(f => f.PropertyType.ToLower().Contains("image") || f.PropertyType.ToLower().Contains("logo") || f.PropertyType.ToLower().Contains("icon") || f.PropertyType.ToLower().Contains("background") || f.PropertyType.ToLower().Contains("passport") || f.PropertyType.ToLower().Contains("sample"));
        public int TotalVideos => FileEntries.Count(f => f.PropertyType.ToLower().Contains("video"));
        public int TotalDocuments => FileEntries.Count(f => f.PropertyType.ToLower().Contains("cv") || f.PropertyType.ToLower().Contains("certificate") || f.PropertyType.ToLower().Contains("application") || f.PropertyType.ToLower().Contains("receipt") || f.PropertyType.ToLower().Contains("file") || f.PropertyType.ToLower().Contains("signature"));

        public string CurrentBucket { get; set; }

        public void OnGet()
        {
            CurrentBucket = GetCurrentBucket();
            LoadAllFiles(CurrentBucket);
        }

        private void LoadAllFiles(string bucket)
        {
            // Blogs
            var blogs = _context.Blogs.ToList();
            FileEntries.AddRange(
                blogs.Select(x => new FileEntryDto
                {
                    EntityName = "Blog",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                blogs.Select(x => new FileEntryDto
                {
                    EntityName = "Blog",
                    EntityId = x.Id,
                    PropertyType = "Video",
                    Url = x.VideoUrl,
                    Key = x.VideoKey,
                    BucketName = bucket,
                    PropertyName = "Video",
                    CategoryGroup = "Media"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // Products
            var products = _context.Products.ToList();
            FileEntries.AddRange(
                products.Select(x => new FileEntryDto
                {
                    EntityName = "Product",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                products.Select(x => new FileEntryDto
                {
                    EntityName = "Product",
                    EntityId = x.Id,
                    PropertyType = "Video",
                    Url = x.VideoUrl,
                    Key = x.VideoKey,
                    BucketName = bucket,
                    PropertyName = "Video",
                    CategoryGroup = "Media"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                products.Select(x => new FileEntryDto
                {
                    EntityName = "Product",
                    EntityId = x.Id,
                    PropertyType = "TitleBackground",
                    Url = x.TitleBackgroundUrl,
                    Key = x.TitleBackgroundKey,
                    BucketName = bucket,
                    PropertyName = "TitleBackground",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // ProductSamples
            var productSamples = _context.ProductSamples.ToList();
            FileEntries.AddRange(
                productSamples.Select(x => new FileEntryDto
                {
                    EntityName = "ProductSample",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                productSamples.Select(x => new FileEntryDto
                {
                    EntityName = "ProductSample",
                    EntityId = x.Id,
                    PropertyType = "Video",
                    Url = x.VideoUrl,
                    Key = x.VideoKey,
                    BucketName = bucket,
                    PropertyName = "Video",
                    CategoryGroup = "Media"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // PageSections
            var pageSections = _context.PageSections.ToList();
            FileEntries.AddRange(
                pageSections.Select(x => new FileEntryDto
                {
                    EntityName = "PageSection",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                pageSections.Select(x => new FileEntryDto
                {
                    EntityName = "PageSection",
                    EntityId = x.Id,
                    PropertyType = "SecondImage",
                    Url = x.SecondImageUrl,
                    Key = x.SecondImageKey,
                    BucketName = bucket,
                    PropertyName = "SecondImage",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                pageSections.Select(x => new FileEntryDto
                {
                    EntityName = "PageSection",
                    EntityId = x.Id,
                    PropertyType = "Video",
                    Url = x.VideoUrl,
                    Key = x.VideoKey,
                    BucketName = bucket,
                    PropertyName = "Video",
                    CategoryGroup = "Media"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // PageSectionLists
            var pageSectionLists = _context.PageSectionLists.ToList();
            FileEntries.AddRange(
                pageSectionLists.Select(x => new FileEntryDto
                {
                    EntityName = "PageSectionList",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                pageSectionLists.Select(x => new FileEntryDto
                {
                    EntityName = "PageSectionList",
                    EntityId = x.Id,
                    PropertyType = "Video",
                    Url = x.VideoUrl,
                    Key = x.VideoKey,
                    BucketName = bucket,
                    PropertyName = "Video",
                    CategoryGroup = "Media"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // WebPages
            var webPages = _context.WebPages.ToList();
            FileEntries.AddRange(
                webPages.Select(x => new FileEntryDto
                {
                    EntityName = "WebPage",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // Testimonies
            var testimonies = _context.Testimonies.ToList();
            FileEntries.AddRange(
                testimonies.Select(x => new FileEntryDto
                {
                    EntityName = "Testimony",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // CompanyFunds
            var companyFunds = _context.CompanyFunds.ToList();
            FileEntries.AddRange(
                companyFunds.Select(x => new FileEntryDto
                {
                    EntityName = "CompanyFund",
                    EntityId = x.Id,
                    PropertyType = "Receipt",
                    Url = x.ReceiptUrl,
                    Key = x.ReceiptKey,
                    BucketName = bucket,
                    PropertyName = "Receipt",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // CompayExpenses
            var compayExpenses = _context.CompayExpenses.ToList();
            FileEntries.AddRange(
                compayExpenses.Select(x => new FileEntryDto
                {
                    EntityName = "CompayExpenses",
                    EntityId = x.Id,
                    PropertyType = "ProveImage",
                    Url = x.ProveImageUrl,
                    Key = x.ProveImageKey,
                    BucketName = bucket,
                    PropertyName = "ProveImage",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // CareerFiles
            var careerFiles = _context.CareerFiles.ToList();
            FileEntries.AddRange(
                careerFiles.Select(x => new FileEntryDto
                {
                    EntityName = "CareerFile",
                    EntityId = x.Id,
                    PropertyType = "CV",
                    Url = x.CvUrl,
                    Key = x.CvKey,
                    BucketName = bucket,
                    PropertyName = "Cv",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                careerFiles.Select(x => new FileEntryDto
                {
                    EntityName = "CareerFile",
                    EntityId = x.Id,
                    PropertyType = "Certificate",
                    Url = x.CertificateUrl,
                    Key = x.CertificateKey,
                    BucketName = bucket,
                    PropertyName = "Certificate",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                careerFiles.Select(x => new FileEntryDto
                {
                    EntityName = "CareerFile",
                    EntityId = x.Id,
                    PropertyType = "Application",
                    Url = x.ApplicationUrl,
                    Key = x.ApplicationKey,
                    BucketName = bucket,
                    PropertyName = "Application",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                careerFiles.Select(x => new FileEntryDto
                {
                    EntityName = "CareerFile",
                    EntityId = x.Id,
                    PropertyType = "Passport",
                    Url = x.PassportUrl,
                    Key = x.PassportKey,
                    BucketName = bucket,
                    PropertyName = "Passport",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // PortfolioTemplates
            var portfolioTemplates = _context.PortfolioTemplates.ToList();
            FileEntries.AddRange(
                portfolioTemplates.Select(x => new FileEntryDto
                {
                    EntityName = "PortfolioTemplate",
                    EntityId = x.Id,
                    PropertyType = "Sample",
                    Url = x.SampleUrl,
                    Key = x.SampleKey,
                    BucketName = bucket,
                    PropertyName = "Sample",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // PostModal
            var postModals = _context.PostModal.ToList();
            FileEntries.AddRange(
                postModals.Select(x => new FileEntryDto
                {
                    EntityName = "PostModal",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // ProfilePortfolio
            var profilePortfolios = _context.ProfilePortfolios.ToList();
            FileEntries.AddRange(
                profilePortfolios.Select(x => new FileEntryDto
                {
                    EntityName = "ProfilePortfolio",
                    EntityId = x.Id,
                    PropertyType = "Image",
                    Url = x.ImageUrl,
                    Key = x.ImageKey,
                    BucketName = bucket,
                    PropertyName = "Image",
                    CategoryGroup = "Web Content"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // VoteCondidate
            var voteCondidates = _context.VoteCondidates.ToList();
            FileEntries.AddRange(
                voteCondidates.Select(x => new FileEntryDto
                {
                    EntityName = "VoteCondidate",
                    EntityId = x.Id,
                    PropertyType = "CandidateImage",
                    Url = x.CandidateImageUrl,
                    Key = x.CandidateImageKey,
                    BucketName = bucket,
                    PropertyName = "CandidateImage",
                    CategoryGroup = "Media"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // Settings
            var settings = _context.Settings.ToList();
            FileEntries.AddRange(
                settings.Select(x => new FileEntryDto
                {
                    EntityName = "Setting",
                    EntityId = x.Id,
                    PropertyType = "DefaultTitleBackground",
                    Url = x.DefaultTitleBackgroundUrl,
                    Key = x.DefaultTitleBackgroundKey,
                    BucketName = bucket,
                    PropertyName = "DefaultTitleBackground",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                settings.Select(x => new FileEntryDto
                {
                    EntityName = "Setting",
                    EntityId = x.Id,
                    PropertyType = "JobCardTitleBackground",
                    Url = x.JobCardTitleBackgroundUrl,
                    Key = x.JobCardTitleBackgroundKey,
                    BucketName = bucket,
                    PropertyName = "JobCardTitleBackground",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                settings.Select(x => new FileEntryDto
                {
                    EntityName = "Setting",
                    EntityId = x.Id,
                    PropertyType = "Signature",
                    Url = x.SignatureUrl,
                    Key = x.SignatureKey,
                    BucketName = bucket,
                    PropertyName = "Signature",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // SuperSettings
            var superSettings = _context.SuperSettings.ToList();
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "CompanyLogo",
                    Url = x.CompanyLogoUrl,
                    Key = x.CompanyLogoKey,
                    BucketName = bucket,
                    PropertyName = "CompanyLogo",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "CompanyWhiteLogo",
                    Url = x.CompanyWhiteLogoUrl,
                    Key = x.CompanyWhiteLogoKey,
                    BucketName = bucket,
                    PropertyName = "CompanyWhiteLogo",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "CompanyIcon",
                    Url = x.CompanyIconUrl,
                    Key = x.CompanyIconKey,
                    BucketName = bucket,
                    PropertyName = "CompanyIcon",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "ReloaderIcon",
                    Url = x.ReloaderIconUrl,
                    Key = x.ReloaderIconKey,
                    BucketName = bucket,
                    PropertyName = "ReloaderIcon",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "LoginBackgroundImage",
                    Url = x.LoginBackgroundImageUrl,
                    Key = x.LoginBackgroundImageKey,
                    BucketName = bucket,
                    PropertyName = "LoginBackgroundImage",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "PortfolioImageOne",
                    Url = x.PortfolioImageOneUrl,
                    Key = x.PortfolioImageOneKey,
                    BucketName = bucket,
                    PropertyName = "PortfolioImageOne",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "PortfolioImageTwo",
                    Url = x.PortfolioImageTwoUrl,
                    Key = x.PortfolioImageTwoKey,
                    BucketName = bucket,
                    PropertyName = "PortfolioImageTwo",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "PortfolioBreacrumImage",
                    Url = x.PortfolioBreacrumImageUrl,
                    Key = x.PortfolioBreacrumImageKey,
                    BucketName = bucket,
                    PropertyName = "PortfolioBreacrumImage",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "PlayStoreImage",
                    Url = x.PlayStoreImageUrl,
                    Key = x.PlayStoreImageKey,
                    BucketName = bucket,
                    PropertyName = "PlayStoreImage",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );
            FileEntries.AddRange(
                superSettings.Select(x => new FileEntryDto
                {
                    EntityName = "SuperSetting",
                    EntityId = x.Id,
                    PropertyType = "AppStoreImage",
                    Url = x.AppStoreImageUrl,
                    Key = x.AppStoreImageKey,
                    BucketName = bucket,
                    PropertyName = "AppStoreImage",
                    CategoryGroup = "Settings"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // ProjectMains
            var projectMains = _context.ProjectMains.ToList();
            FileEntries.AddRange(
                projectMains.Select(x => new FileEntryDto
                {
                    EntityName = "ProjectMain",
                    EntityId = x.Id,
                    PropertyType = "Logo",
                    Url = x.LogoUrl,
                    Key = x.LogoKey,
                    BucketName = bucket,
                    PropertyName = "Logo",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // ProjectFiles
            var projectFiles = _context.ProjectFiles.ToList();
            FileEntries.AddRange(
                projectFiles.Select(x => new FileEntryDto
                {
                    EntityName = "ProjectFile",
                    EntityId = x.Id,
                    PropertyType = "File",
                    Url = x.FileUrl,
                    Key = x.FileKey,
                    BucketName = bucket,
                    PropertyName = "File",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // ProposalFiles
            var proposalFiles = _context.ProposalFiles.ToList();
            FileEntries.AddRange(
                proposalFiles.Select(x => new FileEntryDto
                {
                    EntityName = "ProposalFile",
                    EntityId = x.Id,
                    PropertyType = "File",
                    Url = x.Url,
                    Key = x.Key,
                    BucketName = bucket,
                    PropertyName = "File",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // XProjectFiles
            var xProjectFiles = _context.XProjectFiles.ToList();
            FileEntries.AddRange(
                xProjectFiles.Select(x => new FileEntryDto
                {
                    EntityName = "XProjectFile",
                    EntityId = x.Id,
                    PropertyType = "File",
                    Url = x.Url,
                    Key = x.Key,
                    BucketName = bucket,
                    PropertyName = "File",
                    CategoryGroup = "Documents"
                }).Where(e => !string.IsNullOrEmpty(e.Key))
            );

            // Sort by entity name then Id
            FileEntries = FileEntries.OrderBy(e => e.EntityName).ThenBy(e => e.EntityId).ToList();
        }

        public string GetCurrentBucket()
        {
            var setting = _context.SuperSettings.FirstOrDefault();
            return setting?.BucketName ?? "juray2023";
        }
    }
}
