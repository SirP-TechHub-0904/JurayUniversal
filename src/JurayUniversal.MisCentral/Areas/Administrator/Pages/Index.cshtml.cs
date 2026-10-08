using System.Collections.Generic;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.MisCentral.Data;
using JurayUniversal.MisCentral.Models;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantAnalyticsService _analyticsService;

        public IndexModel(ApplicationDbContext context, ITenantAnalyticsService analyticsService)
        {
            _context = context;
            _analyticsService = analyticsService;
        }

        public DashboardAnalyticsSummary Summary { get; set; } = new DashboardAnalyticsSummary();
        public List<MasterTenantCatalog> Catalogs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Summary = await _analyticsService.GetDashboardSummaryAsync();
            Catalogs = await _context.MasterTenantCatalogs
                .Include(t => t.FeatureSubscriptions)
                .OrderByDescending(t => t.CreatedAtUtc)
                .ToListAsync();

            return Page();
        }
    }
}
