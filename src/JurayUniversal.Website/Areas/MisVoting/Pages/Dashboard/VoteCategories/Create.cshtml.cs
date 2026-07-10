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
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisVoteCategory Category { get; set; }

        public async Task<IActionResult> OnGetAsync(long electionId)
        {
            var election = await _context.MisElections.FirstOrDefaultAsync(m => m.Id == electionId);
            if (election == null)
            {
                return NotFound();
            }

            Category = new MisVoteCategory
            {
                MisElectionId = electionId,
                SortOrder = _context.MisVoteCategories.Where(c => c.MisElectionId == electionId).Count() + 1
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            _context.MisVoteCategories.Add(Category);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category added successfully.";
            return RedirectToPage("../Elections/Details", new { id = Category.MisElectionId });
        }
    }
}
