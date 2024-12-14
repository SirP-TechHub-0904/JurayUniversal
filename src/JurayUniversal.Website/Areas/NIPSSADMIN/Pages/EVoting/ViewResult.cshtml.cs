using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.EVoting
{
    [Authorize]
         public class ViewResultModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ViewResultModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VoteCategory VoteCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VoteCategory = await _context.VoteCategories
                .Include(v => v.Evoting)
                .Include(x=>x.VoteCondidates).ThenInclude(x=>x.UserVote)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (VoteCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

       
    }

}
