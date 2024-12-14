using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.E
{
    [Authorize]
    public class ViewElectionResultModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;

        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        public ViewElectionResultModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public long VotingId { get; set; }
        public Evoting Evoting { get; set; }

        [BindProperty]
        public VoteCategory VoteCategory { get; set; }

        public List<VoteCondidate> VoteCondidates { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
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

            //
            VoteCondidates = await _context.VoteCondidates
                .Include(x => x.UserVote)
                .Include(x => x.VoteCategory)
                .Where(x => x.VoteCategory.EvotingId == Evoting.Id).ToListAsync();

           

            return Page();
        }
    }
}