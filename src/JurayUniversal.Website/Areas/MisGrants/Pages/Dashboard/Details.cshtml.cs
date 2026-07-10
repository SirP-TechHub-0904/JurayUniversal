using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Dashboard
{
    [Authorize(Policy = "MisGrantsViewer")]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisGrant MisGrant { get; set; }

        [BindProperty]
        public MisGrantMilestone NewMilestone { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisGrant = await _context.MisGrants
                .Include(g => g.MisDonor)
                .Include(g => g.Milestones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (MisGrant == null)
            {
                return NotFound();
            }

            return Page();
        }

        [Authorize(Policy = "MisGrantsEditor")]
        public async Task<IActionResult> OnPostAddMilestoneAsync(long grantId)
        {
            var grant = await _context.MisGrants.FindAsync(grantId);
            if (grant == null) return NotFound();

            NewMilestone.MisGrantId = grantId;
            _context.MisGrantMilestones.Add(NewMilestone);
            await _context.SaveChangesAsync();

            TempData["success"] = "Milestone added successfully.";
            return RedirectToPage(new { id = grantId });
        }

        [Authorize(Policy = "MisGrantsEditor")]
        public async Task<IActionResult> OnPostUpdateMilestoneAsync(long milestoneId, decimal amountReceived, System.DateTime dateReceived, string note)
        {
            var milestone = await _context.MisGrantMilestones.FindAsync(milestoneId);
            if (milestone == null) return NotFound();

            milestone.AmountReceived = amountReceived;
            milestone.ReceivedDate = dateReceived;
            milestone.Note = note;

            await _context.SaveChangesAsync();

            TempData["success"] = "Milestone updated successfully.";
            return RedirectToPage(new { id = milestone.MisGrantId });
        }
    }
}
