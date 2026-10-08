using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Plans
{
    public class IndexModel : PageModel
    {
        private readonly IPricingPlanService _planService;

        public IndexModel(IPricingPlanService planService)
        {
            _planService = planService;
        }

        public List<PricingPlanItem> Plans { get; set; } = new();

        [BindProperty]
        public PlanInputModel Input { get; set; } = new();

        public async Task OnGetAsync()
        {
            Plans = await _planService.GetAllPlansAsync();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            var featuresArray = (Input.FeaturesRaw ?? string.Empty)
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.Trim())
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .ToList();

            var plan = new PricingPlanItem
            {
                Id = string.IsNullOrWhiteSpace(Input.Id) ? Guid.NewGuid().ToString("N") : Input.Id,
                Name = Input.Name,
                Subtitle = Input.Subtitle,
                MonthlyPrice = Input.MonthlyPrice,
                Currency = Input.Currency ?? "₦",
                BillingCycle = Input.BillingCycle ?? "month",
                IsPopular = Input.IsPopular,
                SortOrder = Input.SortOrder,
                IsActive = Input.IsActive,
                FeaturesList = featuresArray,
                CtaText = string.IsNullOrWhiteSpace(Input.CtaText) ? "Get Started Now" : Input.CtaText,
                CtaPlanKey = string.IsNullOrWhiteSpace(Input.CtaPlanKey) ? Input.Name : Input.CtaPlanKey
            };

            await _planService.SavePlanAsync(plan);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTogglePopularAsync(string id)
        {
            await _planService.TogglePopularAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(string id)
        {
            await _planService.ToggleActiveAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _planService.DeletePlanAsync(id);
            return RedirectToPage();
        }

        public class PlanInputModel
        {
            public string? Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Subtitle { get; set; } = string.Empty;
            public decimal MonthlyPrice { get; set; }
            public string Currency { get; set; } = "₦";
            public string BillingCycle { get; set; } = "month";
            public bool IsPopular { get; set; }
            public int SortOrder { get; set; } = 1;
            public bool IsActive { get; set; } = true;
            public string FeaturesRaw { get; set; } = string.Empty;
            public string CtaText { get; set; } = "Get Started Now";
            public string CtaPlanKey { get; set; } = "Starter";
        }
    }
}
