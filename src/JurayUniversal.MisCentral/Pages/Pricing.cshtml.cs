using System.Collections.Generic;
using System.Threading.Tasks;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Pages
{
    public class PricingModel : PageModel
    {
        private readonly IPricingPlanService _planService;

        public PricingModel(IPricingPlanService planService)
        {
            _planService = planService;
        }

        public List<PricingPlanItem> Plans { get; set; } = new();

        public async Task OnGetAsync()
        {
            Plans = await _planService.GetAllPlansAsync(onlyActive: true);
        }
    }
}
