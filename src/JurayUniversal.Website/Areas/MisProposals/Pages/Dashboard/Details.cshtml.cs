using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProposals.Pages.Dashboard
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisProposal Proposal { get; set; }

        [BindProperty]
        public MisProposalPhase NewPhase { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Proposal = await _context.MisProposals
                .Include(p => p.UserProfile)
                .Include(p => p.ProposalFiles)
                .Include(p => p.Phases)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Proposal == null)
            {
                return NotFound();
            }

            // If it's the first time an admin views it, mark it as Under Review automatically
            if (Proposal.Status == JurayUniversal.Domain.Enum.Enum.ProposalStatus.Submitted)
            {
                Proposal.Status = JurayUniversal.Domain.Enum.Enum.ProposalStatus.UnderReview;
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(long id)
        {
            var proposal = await _context.MisProposals.FindAsync(id);
            if (proposal != null)
            {
                proposal.Status = JurayUniversal.Domain.Enum.Enum.ProposalStatus.Approved;
                proposal.DateApproved = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                TempData["success"] = "Proposal has been approved!";
            }
            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostRejectAsync(long id)
        {
            var proposal = await _context.MisProposals.FindAsync(id);
            if (proposal != null)
            {
                proposal.Status = JurayUniversal.Domain.Enum.Enum.ProposalStatus.Rejected;
                await _context.SaveChangesAsync();
                
                TempData["error"] = "Proposal has been rejected.";
            }
            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostAddPhaseAsync(long proposalId)
        {
            var proposal = await _context.MisProposals.FindAsync(proposalId);
            if (proposal == null) return NotFound();

            NewPhase.MisProposalId = proposalId;
            NewPhase.CreatedAt = DateTime.UtcNow;
            
            _context.MisProposalPhases.Add(NewPhase);
            await _context.SaveChangesAsync();

            TempData["success"] = "Execution phase added successfully.";
            return RedirectToPage(new { id = proposalId });
        }

        public async Task<IActionResult> OnPostUpdatePhaseAsync(long phaseId, JurayUniversal.Domain.Enum.Enum.PhaseStatus status, string reportSummary)
        {
            var phase = await _context.MisProposalPhases.FindAsync(phaseId);
            if (phase == null) return NotFound();

            phase.Status = status;
            phase.ReportSummary = reportSummary;

            await _context.SaveChangesAsync();

            TempData["success"] = "Phase updated successfully.";
            return RedirectToPage(new { id = phase.MisProposalId });
        }
    }
}
