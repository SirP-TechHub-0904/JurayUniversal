using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Participants
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class RemitaDetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public RemitaDetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public ParticipantList ParticipantList { get; set; }
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ParticipantList = await _context.ParticipantLists
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ParticipantList == null)
            {
                return NotFound();
            }
            Course = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == ParticipantList.CourseId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ParticipantList = await _context.ParticipantLists.FindAsync(id);

            if (ParticipantList != null)
            {
                ParticipantList.RemitaApproved = true;
                _context.ParticipantLists.Update(ParticipantList);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./RemitaDetails");
        }

    }
}
