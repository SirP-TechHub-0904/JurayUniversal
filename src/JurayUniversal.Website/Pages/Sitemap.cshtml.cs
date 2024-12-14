using Amazon.Runtime.Internal.Transform;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml.Linq;

namespace JurayUniversal.Website.Pages
{
    public class SitemapModel : PageModel
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly PageLoader _pageLoader; // Assuming this is needed for future use
        private readonly DashboardDbContext _context;
        private readonly Regex _regex = new Regex("[^a-zA-Z0-9]"); // Regex to clean up titles

        public SitemapModel(IUrlHelperFactory urlHelperFactory, PageLoader pageLoader, DashboardDbContext context)
        {
            _urlHelperFactory = urlHelperFactory;
            _pageLoader = pageLoader;
            _context = context;
        }

        public List<string> PageUrls { get; set; } = new List<string>(); // Initialize the list
        public string Templatechoose { get; set; }
        public SuperSetting SuperSetting { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(PageContext);

            // Initialize a dictionary to group sitemap entries by category title
            var groupedEntries = new Dictionary<string, List<SitemapEntry>>();

            // Step 1: Add manual Razor Page URLs first, under "Website Pages"
            var setting = await _context.Settings.FirstOrDefaultAsync();
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Templatechoose = SuperSetting.TemplateLayoutKey;
            var manualUrls = new List<SitemapEntry>();
            // Add default Razor Page URLs
            manualUrls.AddRange(new List<SitemapEntry>
    {
        new SitemapEntry { Title = "Home", Url = urlHelper.Page("/Index") },
        new SitemapEntry { Title = "Contact", Url = urlHelper.Page("/Contact") },
        // Add more Razor Pages as needed
    });
            if (setting.AddCareerToFooter == true || setting.AddCareerToMenu == true)
            {
                string siteurl = urlHelper.Page("/Career");
                manualUrls.Add(new SitemapEntry
                {
                    Title = "Career Page", // Store the title
                    Url = siteurl // Store the URL
                });
            }

            if (setting.AddTestimonyToFooter == true || setting.AddTestimonyToHome == true)
            {
                string siteurl = urlHelper.Page("/Tpg");
                manualUrls.Add(new SitemapEntry
                {
                    Title = "Testimony Pages", // Store the title
                    Url = siteurl // Store the URL
                });
            }

            if (setting.AddBlogToMenu == true || setting.AddBlogToHome == true)
            {
                string siteurl = urlHelper.Page("/Bpg");
                manualUrls.Add(new SitemapEntry
                {
                    Title = "Blog Pages", // Store the title
                    Url = siteurl // Store the URL
                });
            }

            if (setting.ShowProductsInMenu == true || setting.ShowProductsInFooter == true)
            {
                string siteurl = urlHelper.Page("/UHSource");
                manualUrls.Add(new SitemapEntry
                {
                    Title = SuperSetting.ProductTitle, // Store the title
                    Url = siteurl // Store the URL
                });
            }



            // Combine manual URLs under the title "Website Pages" and add them first
            groupedEntries["Website Pages"] = manualUrls;

            // Step 2: Add category pages
            var pages = await _context.PageCategories
                .Include(x => x.WebPages)
                .Where(x => x.Publish)
                .ToListAsync();

            foreach (var category in pages)
            {
                if (!groupedEntries.ContainsKey(category.Title))
                {
                    groupedEntries[category.Title] = new List<SitemapEntry>(); // Initialize the list for this category
                }

                foreach (var item in category.WebPages.Where(x => x.Publish == true))
                {
                    string cleanTitle = _regex.Replace(item.Title, "-");
                    string url = urlHelper.Page("/Source", new { id = item.Id, title = cleanTitle });
                    groupedEntries[category.Title].Add(new SitemapEntry
                    {
                        Title = item.Title, // Store the title
                        Url = url // Store the URL
                    });
                }
            }

            // Pass the grouped entries to the view
            ViewData["GroupedEntries"] = groupedEntries;

            return Page();
        }
    }
    public class SitemapEntry
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

}
