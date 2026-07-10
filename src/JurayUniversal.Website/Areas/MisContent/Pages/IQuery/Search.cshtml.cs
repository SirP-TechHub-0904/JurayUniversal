using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace JurayUniversal.Website.Areas.MisContent.Pages.IQuery
{
    [Authorize]
    public class SearchModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public List<SearchResultDto> Results { get; set; } = new();
        public int TotalMatches { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling((double)TotalMatches / PageSize);
        public SearchModel(DashboardDbContext context)
        {
            _context = context;
        }

        public void OnGet(string query, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(query))
                return;

            Results = new List<SearchResultDto>();
            SearchContent(query);
            SearchBlogs(query);
            SearchBlogCategories(query);
            SearchProducts(query);

            TotalMatches = Results.Count;
            CurrentPage = page < 1 ? 1 : page;

            Results = Results
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        #region Search Groups
        private string HighlightText(string text, string keyword)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(keyword))
                return text;

            return System.Text.RegularExpressions.Regex.Replace(
                text,
                $"({System.Text.RegularExpressions.Regex.Escape(keyword)})",
                "<span class='highlight'>$1</span>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }

        private void SearchContent(string query)
        {
            // =============================
            // LOAD EVERYTHING ONCE
            // =============================
            var allWebPages = _context.WebPages
                .Include(wp => wp.PageSections)
                    .ThenInclude(ps => ps.PageSectionLists)
                .ToList();

            var allSections = _context.PageSections
                .Include(ps => ps.PageSectionLists)
                .ToList();

            var allLists = _context.PageSectionLists.ToList();

            var processedWebPages = new HashSet<long>();
            var processedSections = new HashSet<long>();
            var processedLists = new HashSet<long>();

            // =============================
            // 1. CATEGORY → FULL TREE
            // =============================
            var categories = _context.PageCategories
                .Include(c => c.WebPages)
                .ToList();

            foreach (var cat in categories)
            {
                SearchObject(cat, query, $"PageCategory: {cat.Title}", Results);

                foreach (var wp in cat.WebPages ?? new List<WebPage>())
                {
                    var fullWp = allWebPages.FirstOrDefault(x => x.Id == wp.Id);
                    if (fullWp == null) continue;

                    processedWebPages.Add(fullWp.Id);

                    SearchObject(fullWp, query, $"PageCategory: {cat.Title} > WebPage: {fullWp.Title}", Results);

                    foreach (var sec in fullWp.PageSections ?? new List<PageSection>())
                    {
                        processedSections.Add(sec.Id);

                        SearchObject(sec, query, $"PageCategory: {cat.Title} > WebPage: {fullWp.Title} > Section: {sec.Title}", Results);

                        foreach (var list in sec.PageSectionLists ?? new List<PageSectionList>())
                        {
                            processedLists.Add(list.Id);

                            SearchObject(list, query,
                                $"PageCategory: {cat.Title} > WebPage: {fullWp.Title} > Section: {sec.Title} > List: {list.Title}",
                                Results);
                        }
                    }
                }
            }

            //
            // =============================
            // 2. WEBPAGES WITHOUT CATEGORY
            // =============================
            foreach (var wp in allWebPages)
            {
                if (processedWebPages.Contains(wp.Id))
                    continue;

                SearchObject(wp, query, $"WebPage (No Category): {wp.Title}", Results);

                foreach (var sec in wp.PageSections ?? new List<PageSection>())
                {
                    processedSections.Add(sec.Id);

                    SearchObject(sec, query, $"WebPage: {wp.Title} > Section: {sec.Title}", Results);

                    foreach (var list in sec.PageSectionLists ?? new List<PageSectionList>())
                    {
                        processedLists.Add(list.Id);

                        SearchObject(list, query,
                            $"WebPage: {wp.Title} > Section: {sec.Title} > List: {list.Title}",
                            Results);
                    }
                }
            }

            //
            // =============================
            // 3. SECTIONS WITHOUT WEBPAGE
            // =============================
            foreach (var sec in allSections)
            {
                if (processedSections.Contains(sec.Id))
                    continue;

                SearchObject(sec, query, $"PageSection (No WebPage): {sec.Title}", Results);

                foreach (var list in sec.PageSectionLists ?? new List<PageSectionList>())
                {
                    processedLists.Add(list.Id);

                    SearchObject(list, query,
                        $"PageSection: {sec.Title} > List: {list.Title}",
                        Results);
                }
            }

            //
            // =============================
            // 4. LISTS WITHOUT SECTION
            // =============================
            foreach (var list in allLists)
            {
                if (processedLists.Contains(list.Id))
                    continue;

                SearchObject(list, query, $"PageSectionList (Orphan): {list.Title}", Results);
            }

            var faqs = _context.FAQs.ToList();
            foreach (var faq in faqs)
                SearchObject(faq, query, $"FAQ: {faq.Title}", Results);

            var sliders = _context.Sliders.ToList();
            foreach (var slider in sliders)
                SearchObject(slider, query, $"Slider: {slider.Title}", Results);

            var testimonies = _context.Testimonies.ToList();
            foreach (var testimony in testimonies)
                SearchObject(testimony, query, $"Testimony: {testimony.Title}", Results);

            var postModals = _context.PostModal.ToList();
            foreach (var modal in postModals)
                SearchObject(modal, query, $"PostModal: {modal.Title}", Results);

        }

        private void SearchBlogs(string query)
        {
            var blogs = _context.Blogs.ToList();

            foreach (var blog in blogs)
            {
                SearchObject(blog, query, $"Blog: {blog.Title}", Results);
            }
        }
        private void SearchBlogCategories(string query)
        {
            var blogCategories = _context.BlogCategories.ToList();

            foreach (var category in blogCategories)
            {
                SearchObject(category, query, $"BlogCategory: {category.Title}", Results);
            }
        }

        private void SearchProducts(string query)
        {
            var productCategories = _context.ProductCategories
                .Include(pc => pc.Product)
                    .ThenInclude(p => p.ProductFeatures)
                .Include(pc => pc.Product)
                    .ThenInclude(p => p.ProductSamples)
                .ToList();

            foreach (var category in productCategories)
            {
                SearchObject(category, query, $"ProductCategory: {category.Title}", Results);

                foreach (var product in category.Product ?? new List<Product>())
                {
                    SearchObject(product, query, $"ProductCategory: {category.Title} > Product: {product.Title}", Results);

                    foreach (var feature in product.ProductFeatures ?? new List<ProductFeature>())
                    {
                        SearchObject(feature, query, $"ProductCategory: {category.Title} > Product: {product.Title} > ProductFeature: {feature.Title}", Results);
                    }

                    foreach (var sample in product.ProductSamples ?? new List<ProductSample>())
                    {
                        SearchObject(sample, query, $"ProductCategory: {category.Title} > Product: {product.Title} > ProductSample: {sample.Title}", Results);
                    }
                }
            }
        }

        #endregion

        #region Recursive WebPage Search

        private void SearchWebPageRecursive(WebPage wp, string query, string path, List<SearchResultDto> results)
        {
            SearchObject(wp, query, path, results);

            foreach (var section in wp.PageSections ?? new List<PageSection>())
            {
                SearchObject(section, query, $"{path} > PageSection: {section.Title}", results);

                foreach (var list in section.PageSectionLists ?? new List<PageSectionList>())
                {
                    SearchObject(list, query, $"{path} > PageSection: {section.Title} > ListItem: {list.Title}", results);
                }
            }


        }

        #endregion

        #region Reflection Search

        private void SearchObject(object obj, string keyword, string path, List<SearchResultDto> results)
        {
            if (obj == null || string.IsNullOrWhiteSpace(keyword))
                return;

            var normalizedKeyword = keyword.Trim().ToLowerInvariant();

            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.PropertyType != typeof(string))
                    continue;

                var rawValue = prop.GetValue(obj) as string;
                if (string.IsNullOrWhiteSpace(rawValue))
                    continue;

                var normalizedValue = rawValue.ToLowerInvariant();

                if (normalizedValue.Contains(normalizedKeyword))
                {
                    var pathUrl = GetEditPath(obj);

                    string titleValue = "";
                    try
                    {
                        var allProps = obj.GetType().GetProperties();
                        var titleProp = allProps.FirstOrDefault(p => p.Name == "Title") ?? allProps.FirstOrDefault(p => p.Name == "Name");
                        if (titleProp != null)
                        {
                            titleValue = titleProp.GetValue(obj)?.ToString() ?? "";
                        }
                    }
                    catch
                    {
                    }

                    results.Add(new SearchResultDto
                    {
                        Path = pathUrl,
                        PathText = path,
                        EntityType = obj.GetType().Name,
                        Title = titleValue,
                        Field = prop.Name,
                        Value = HighlightText(rawValue, keyword),
                        MatchedText = keyword
                    });
                }
            }
        }

        private string GetEditPath(object obj)
        {
            return obj switch
            {
                PageCategory x => $"/MisContent/IPages/Category/edit?id={x.Id}",
                WebPage x => $"/MisContent/IPages/Main/edit?id={x.Id}",
                PageSection x => $"/MisContent/IPages/Section/edit?id={x.Id}",
                PageSectionList x => $"/MisContent/IPages/List/edit?id={x.Id}",

                BlogCategory x => $"/MisContent/IBlogs/Category/Edit?id={x.Id}",
                Blog x => $"/MisContent/IBlogs/Post/Edit?id={x.Id}",


                FAQ x => $"/MisContent/IFaq/edit?id={x.Id}",
                Slider x => $"/MisContent/ISliders/edit?id={x.Id}",
                Testimony x => $"/MisContent/ITestimony/edit?id={x.Id}",
                PostModal x => $"/MisContent/IModal/edit?id={x.Id}",


                ProductCategory x => $"/MisContent/IProduct/Category/edit?id={x.Id}",
                Product x => $"/MisContent/IProduct/Main/edit?id={x.Id}",
                ProductFeature x => $"/MisContent/IProduct/IFeatures/edit?id={x.Id}",
                ProductSample x => $"/MisContent/IProduct/ISample/edit?id={x.Id}",
 
                
                ProjectCategory x => $"/MisContent/IContinue/Project/Category/edit?id={x.Id}",
                 

                _ => "#"
            };
        }

        #endregion
    }

    public class SearchResultDto
    {
        public string Path { get; set; } = "#";
        public string PathText { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string MatchedText { get; set; } = string.Empty;
    }
}
