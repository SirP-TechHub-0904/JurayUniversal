using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisManagement.Pages.Plans
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
        public MisNewsletterPlan MisNewsletterPlan { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisNewsletterPlan = await _context.MisNewsletterPlans.FirstOrDefaultAsync(m => m.Id == id);

            if (MisNewsletterPlan == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MisNewsletterPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.MisNewsletterPlans.AnyAsync(e => e.Id == MisNewsletterPlan.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["success"] = "Newsletter Plan updated successfully.";
            return RedirectToPage("./Index");
        }
    }
}
