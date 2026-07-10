using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProposals.Pages.Platform
{
    [Authorize]
    public class SubmitModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IWebHostEnvironment _env;

        public SubmitModel(DashboardDbContext context, UserManager<Profile> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [BindProperty]
        public MisProposal Proposal { get; set; }

        [BindProperty]
        public List<IFormFile> UploadedFiles { get; set; }

        public void OnGet()
        {
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

            var userId = _userManager.GetUserId(User);
            
            Proposal.UserId = userId;
            Proposal.CreatedAt = DateTime.UtcNow;
            Proposal.DateSubmitted = DateTime.UtcNow;
            Proposal.Status = JurayUniversal.Domain.Enum.Enum.ProposalStatus.Submitted;

            _context.MisProposals.Add(Proposal);
            await _context.SaveChangesAsync();

            // Handle file uploads if any
            if (UploadedFiles != null && UploadedFiles.Count > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "mis_proposals");
                Directory.CreateDirectory(uploadFolder);

                foreach (var file in UploadedFiles)
                {
                    if (file.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        var proposalFile = new MisProposalFile
                        {
                            MisProposalId = Proposal.Id,
                            Title = file.FileName,
                            Url = "/uploads/mis_proposals/" + uniqueFileName,
                            Key = uniqueFileName
                        };

                        _context.MisProposalFiles.Add(proposalFile);
                    }
                }
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Your proposal has been successfully submitted for review.";
            return RedirectToPage("./Index");
        }
    }
}
