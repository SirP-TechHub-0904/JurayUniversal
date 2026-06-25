using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisManagement.Pages
{
    [Authorize(Roles = "mSuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisFeature MisFeature { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null) return NotFound();

            var feature = await _context.MisFeatures.FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null) return NotFound();

            MisFeature = feature;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var featureToUpdate = await _context.MisFeatures.FindAsync(MisFeature.Id);
            if (featureToUpdate == null) return NotFound();

            featureToUpdate.FeatureName = MisFeature.FeatureName;
            featureToUpdate.ModuleFolder = MisFeature.ModuleFolder;
            featureToUpdate.Description = MisFeature.Description;
            featureToUpdate.BillingDuration = MisFeature.BillingDuration;
            featureToUpdate.CostPerDuration = MisFeature.CostPerDuration;
            featureToUpdate.GracePeriodDays = MisFeature.GracePeriodDays;

            _context.Entry(featureToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = "Feature configuration updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
