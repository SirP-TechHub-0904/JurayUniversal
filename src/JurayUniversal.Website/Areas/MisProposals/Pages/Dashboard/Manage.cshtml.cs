using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace JurayUniversal.Website.Areas.MisProposals.Pages.Dashboard
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ManageModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisProposal MisProposal { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                MisProposal = new MisProposal();
            }
            else
            {
                MisProposal = await _context.MisProposals.FirstOrDefaultAsync(m => m.Id == id.Value);

                if (MisProposal == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ModelState.Remove("MisProposal.UserProfile");
            // ModelState.Remove("MisProposal.ProposalFiles");
            // ModelState.Remove("MisProposal.LinkedGrant");
            // ModelState.Remove("MisProposal.Phases");
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            if (MisProposal.Id == 0)
            {
                // New proposal created by an Admin
                MisProposal.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                MisProposal.CreatedAt = DateTime.UtcNow;
                MisProposal.Status = JurayUniversal.Domain.Enum.Enum.ProposalStatus.Draft;
                
                _context.MisProposals.Add(MisProposal);
            }
            else
            {
                // Update existing proposal
                var entity = await _context.MisProposals.FindAsync(MisProposal.Id);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.Title = MisProposal.Title;
                entity.Description = MisProposal.Description;
                entity.Address = MisProposal.Address;
                entity.Note = MisProposal.Note;
                entity.TypedProposal = MisProposal.TypedProposal;
                entity.ExecutionDuration = MisProposal.ExecutionDuration;
                entity.ProjectStartDate = MisProposal.ProjectStartDate;
                entity.ProjectEndDate = MisProposal.ProjectEndDate;
                // Status mapping should be managed separately or via a different action, not directly here unless specifically requested.
                // Or maybe they can submit it immediately.
            }

            await _context.SaveChangesAsync();

            TempData["success"] = MisProposal.Id == 0 ? "Proposal created successfully." : "Proposal updated successfully.";
            return RedirectToPage("./Index");
        }
    }
}
