using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.Candidates
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
        public MisCandidate Candidate { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Candidate = await _context.MisCandidates.FirstOrDefaultAsync(m => m.Id == id);

            if (Candidate == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var candidateToUpdate = await _context.MisCandidates.FirstOrDefaultAsync(c => c.Id == Candidate.Id);
            if (candidateToUpdate == null)
            {
                return NotFound();
            }

            if (imagefile != null)
            {
                var xresult = await _storageService.MainUploadFileReturnUrlAsync("", imagefile);
                if (xresult.Message.Contains("200"))
                {
                    candidateToUpdate.CandidateImageUrl = xresult.Url;
                    candidateToUpdate.CandidateImageKey = xresult.Key;
                }
            }

            candidateToUpdate.Title = Candidate.Title;
            candidateToUpdate.ShortProfile = Candidate.ShortProfile;
            candidateToUpdate.Manifesto = Candidate.Manifesto;

            await _context.SaveChangesAsync();

            var category = await _context.MisVoteCategories.FirstOrDefaultAsync(c => c.Id == candidateToUpdate.MisVoteCategoryId);
            
            TempData["success"] = "Candidate updated successfully.";
            return RedirectToPage("../Elections/Details", new { id = category.MisElectionId });
        }
    }
}
