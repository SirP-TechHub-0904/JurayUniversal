using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.SitePages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public string BaseUrl { get; set; }
        public List<SitePageItem> PageLinks { get; set; } = new();

        public class SitePageItem
        {
            public string Title { get; set; }
            public string Route { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string TargetAudience { get; set; }
            public string IconClass { get; set; }
        }

        public async Task OnGetAsync()
        {
            var setting = await _context.SuperSettings.FirstOrDefaultAsync();
            var domain = setting?.CompanyWebsiteLink?.TrimEnd('/');
            BaseUrl = !string.IsNullOrEmpty(domain) ? domain : $"{Request.Scheme}://{Request.Host}";

            PageLinks = new List<SitePageItem>
            {
                // Payment Portal
                new SitePageItem { Title = "Payment Portal Directory", Route = "/pay", Description = "Public portal listing all active guest payment categories.", Category = "Payment", TargetAudience = "Public & Members", IconClass = "bi-credit-card-2-front-fill" },
                new SitePageItem { Title = "Payment Checkout Form", Route = "/pay/{category_key}", Description = "Direct payment form for a specific payment category (replace {category_key} with category key).", Category = "Payment", TargetAudience = "Public & Members", IconClass = "bi-cash-stack" },
                new SitePageItem { Title = "Payment Receipt Verification", Route = "/pay/callback?ref={transaction_ref}", Description = "Payment confirmation receipt & verification page.", Category = "Payment", TargetAudience = "Public & Members", IconClass = "bi-receipt" },

                // Public Pages
                new SitePageItem { Title = "Community Profiling Form (misCommunityForm)", Route = "/CommunityForm", Description = "Public clan & community membership profiling questionnaire and application form.", Category = "Public Information", TargetAudience = "Public & Members", IconClass = "bi-ui-checks-grid" },
                new SitePageItem { Title = "Website Homepage", Route = "/", Description = "Main organization website landing page.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-house-door-fill" },
                new SitePageItem { Title = "Contact Us", Route = "/Contact", Description = "Organization contact information and message inquiry form.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-envelope-paper-fill" },
                new SitePageItem { Title = "Careers & Jobs", Route = "/Career", Description = "Job openings and career opportunities page.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-briefcase-fill" },
                new SitePageItem { Title = "Online Member Registration", Route = "/Registration", Description = "Member online application signup form.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-person-plus-fill" },
                new SitePageItem { Title = "Registration Status Check", Route = "/RegStatus", Description = "Check membership registration progress.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-patch-check-fill" },
                new SitePageItem { Title = "Knowledge Base & Help", Route = "/KnowledgeBase", Description = "Help articles, documentation, and user guides.", Category = "Public Information", TargetAudience = "Public & Members", IconClass = "bi-book-fill" },
                new SitePageItem { Title = "Newsletter Subscribe", Route = "/Subscribe", Description = "Email newsletter subscription page.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-newspaper" },
                new SitePageItem { Title = "Site Map", Route = "/Sitemap", Description = "System sitemap and navigation index.", Category = "Public Information", TargetAudience = "Public", IconClass = "bi-diagram-3-fill" },

                // Member Pages
                new SitePageItem { Title = "Member Payments History", Route = "/Payments", Description = "Personal transaction history for logged-in members.", Category = "Member Accounts", TargetAudience = "Logged-in Members", IconClass = "bi-wallet2" },
                new SitePageItem { Title = "User Profile", Route = "/Profile", Description = "Member account profile and personal details.", Category = "Member Accounts", TargetAudience = "Logged-in Members", IconClass = "bi-person-badge-fill" }
            };
        }
    }
}
