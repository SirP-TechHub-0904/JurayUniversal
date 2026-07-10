using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.Elections
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IStorageService _storageService;

        public EditModel(DashboardDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        [BindProperty]
        public MisElection Election { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Election = await _context.MisElections.FirstOrDefaultAsync(m => m.Id == id);

            if (Election == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var electionToUpdate = await _context.MisElections
                .Include(e => e.MisVoteCategories)
                .ThenInclude(c => c.MisCandidates)
                .FirstOrDefaultAsync(e => e.Id == Election.Id);

            if (electionToUpdate == null)
            {
                return NotFound();
            }

            // Validate Active Status manually
            if (Election.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Active && electionToUpdate.VotingStatus != Domain.Enum.Enum.MisVotingStatus.Active)
            {
                bool hasCandidates = electionToUpdate.MisVoteCategories != null && 
                                     electionToUpdate.MisVoteCategories.Any(c => c.MisCandidates != null && c.MisCandidates.Any());
                
                if (!hasCandidates)
                {
                    TempData["error"] = "Cannot set status to Active: You must add at least one candidate before activating the election.";
                    return Page();
                }
            }

            // Upload banner if provided
            if (imagefile != null)
            {
                var xresult = await _storageService.MainUploadFileReturnUrlAsync("", imagefile);
                if (xresult.Message.Contains("200"))
                {
                    electionToUpdate.ElectionBannerUrl = xresult.Url;
                }
            }

            // Update properties
            electionToUpdate.Title = Election.Title;
            electionToUpdate.Instructions = Election.Instructions;
            electionToUpdate.ElectionType = Election.ElectionType;
            electionToUpdate.VotingStatus = Election.VotingStatus;
            electionToUpdate.StartDate = Election.StartDate;
            electionToUpdate.EndDate = Election.EndDate;
            electionToUpdate.EnableCandidateImages = Election.EnableCandidateImages;
            // ShortCode should not be updated from Edit page to prevent accidentally breaking links

            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Election updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectionExists(Election.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = Election.Id });
        }

        private bool ElectionExists(long id)
        {
            return _context.MisElections.Any(e => e.Id == id);
        }
    }
}
