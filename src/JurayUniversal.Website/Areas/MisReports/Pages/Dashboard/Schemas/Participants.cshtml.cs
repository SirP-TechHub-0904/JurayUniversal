using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard.Schemas
{
    [Authorize(Policy = "MisReportsEditor")]
    public class ParticipantsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ParticipantsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisReportSchema Schema { get; set; }
        public IList<MisReportParticipant> Participants { get; set; }

        [BindProperty]
        public string NewParticipantId { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Schema = await _context.MisReportSchemas.FindAsync(id);
            if (Schema == null) return NotFound();

            Participants = await _context.MisReportParticipants
                .Include(p => p.UserProfile)
                .Where(p => p.MisReportSchemaId == id)
                .ToListAsync();

            var users = await _userManager.Users.OrderBy(u => u.FirstName).ToListAsync();
            ViewData["UserProfileId"] = new SelectList(users, "Id", "Email");

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync(long id)
        {
            if (string.IsNullOrEmpty(NewParticipantId))
            {
                TempData["error"] = "Please select a user to add.";
                return RedirectToPage(new { id = id });
            }

            var existing = await _context.MisReportParticipants
                .FirstOrDefaultAsync(p => p.MisReportSchemaId == id && p.UserProfileId == NewParticipantId);

            if (existing == null)
            {
                var participant = new MisReportParticipant
                {
                    MisReportSchemaId = id,
                    UserProfileId = NewParticipantId
                };
                _context.MisReportParticipants.Add(participant);
                await _context.SaveChangesAsync();
                TempData["success"] = "User added as a required participant.";
            }
            else
            {
                TempData["warning"] = "User is already a participant in this schema.";
            }

            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostRemoveAsync(long id, long participantId)
        {
            var participant = await _context.MisReportParticipants
                .FirstOrDefaultAsync(p => p.Id == participantId && p.MisReportSchemaId == id);

            if (participant != null)
            {
                _context.MisReportParticipants.Remove(participant);
                await _context.SaveChangesAsync();
                TempData["success"] = "Participant removed successfully.";
            }

            return RedirectToPage(new { id = id });
        }
    }
}
