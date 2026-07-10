using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisManagement.Pages.Plans
{
    [Authorize(Roles = "mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisNewsletterPlan> MisNewsletterPlans { get; set; } = new List<MisNewsletterPlan>();

        public async Task<IActionResult> OnGetAsync()
        {
            MisNewsletterPlans = await _context.MisNewsletterPlans.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(long id)
        {
            var plan = await _context.MisNewsletterPlans.FindAsync(id);
            if (plan == null) return NotFound();

            plan.IsActive = !plan.IsActive;
            await _context.SaveChangesAsync();
            
            TempData["success"] = "Plan status updated successfully.";
            return RedirectToPage();
        }
    }
}
