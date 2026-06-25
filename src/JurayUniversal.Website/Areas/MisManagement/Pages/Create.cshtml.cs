using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisManagement.Pages
{
    [Authorize(Roles = "mSuperAdmin")]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisFeature MisFeature { get; set; } = new MisFeature();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MisFeature.Status = JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Disabled; // Default to disabled
            
            _context.MisFeatures.Add(MisFeature);
            await _context.SaveChangesAsync();

            TempData["success"] = "Feature pricing configured successfully!";
            return RedirectToPage("./Index");
        }
    }
}
