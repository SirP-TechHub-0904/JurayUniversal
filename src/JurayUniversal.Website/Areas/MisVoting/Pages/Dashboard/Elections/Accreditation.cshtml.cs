using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.Elections
{
    [Authorize]
    public class AccreditationModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public AccreditationModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisElection Election { get; set; }
        
        public class VoterDto
        {
            public string UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public bool IsAccredited { get; set; }
        }

        public List<VoterDto> Voters { get; set; } = new List<VoterDto>();

        [BindProperty]
        public List<string> SelectedUserIds { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Election = await _context.MisElections
                .Include(x => x.MisElectionVoters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Election == null || Election.ElectionType != Domain.Enum.Enum.MisElectionType.Closed)
            {
                return NotFound();
            }

            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                bool isAccredited = Election.MisElectionVoters.Any(x => x.UserId == user.Id && x.IsAccredited);
                Voters.Add(new VoterDto
                {
                    UserId = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    Email = user.Email,
                    IsAccredited = isAccredited
                });
            }

            Voters = Voters.OrderBy(x => x.FullName).ToList();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            Election = await _context.MisElections
                .Include(x => x.MisElectionVoters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Election == null)
            {
                return NotFound();
            }

            // Remove existing unvoted accreditations
            var existingAccredited = Election.MisElectionVoters.Where(x => !x.HasVoted).ToList();
            _context.MisElectionVoters.RemoveRange(existingAccredited);

            if (SelectedUserIds != null)
            {
                foreach (var userId in SelectedUserIds)
                {
                    // If they already voted, we don't recreate them.
                    if (!Election.MisElectionVoters.Any(x => x.UserId == userId && x.HasVoted))
                    {
                        _context.MisElectionVoters.Add(new MisElectionVoter
                        {
                            MisElectionId = id,
                            UserId = userId,
                            IsAccredited = true,
                            HasVoted = false
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Accreditation checklist updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
