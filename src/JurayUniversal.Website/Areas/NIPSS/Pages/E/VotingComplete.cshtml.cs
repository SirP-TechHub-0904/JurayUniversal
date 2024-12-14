using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.E
{
    [Authorize]
    public class VotingCompleteModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public VotingCompleteModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public SuperSetting SuperSetting { get; set; }

        public Evoting Evoting { get; set; }
        public List<SelectedVoteDto> SelectedVotes { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Evoting = await _context.Evotings
                .Include(v => v.Course)
                .ThenInclude(v => v.CourseCategory)
                .Include(v => v.VoteCategories)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (Evoting == null)
            {
                return NotFound();
            }
            if (TempData["completevotetoken"] == null)
            {

                TempData["error"] = "Uanble to Validate Voted. Try again";
                return RedirectToPage("./Index", new { id = id });
            }
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            return Page();
        }
    }
}
