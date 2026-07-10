using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.VoteCategories
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisVoteCategory Category { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Category = await _context.MisVoteCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            var categoryToUpdate = await _context.MisVoteCategories.FirstOrDefaultAsync(c => c.Id == Category.Id);
            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            categoryToUpdate.Title = Category.Title;
            categoryToUpdate.Description = Category.Description;
            categoryToUpdate.SortOrder = Category.SortOrder;

            await _context.SaveChangesAsync();
            TempData["success"] = "Category updated successfully.";

            return RedirectToPage("../Elections/Details", new { id = categoryToUpdate.MisElectionId });
        }
    }
}
