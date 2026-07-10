using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisManagement.Pages.Plans
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
        public MisNewsletterPlan MisNewsletterPlan { get; set; }

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

            MisNewsletterPlan.IsActive = true;
            _context.MisNewsletterPlans.Add(MisNewsletterPlan);
            await _context.SaveChangesAsync();

            TempData["success"] = "Newsletter Plan created successfully.";
            return RedirectToPage("./Index");
        }
    }
}
