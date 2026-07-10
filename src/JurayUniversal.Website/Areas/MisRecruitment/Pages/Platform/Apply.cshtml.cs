using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Platform
{
    [Authorize(AuthenticationSchemes = "ApplicantAuth")]
    public class ApplyModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ApplyModel(DashboardDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public MisJobApplicant Applicant { get; set; }

        public MisJobPosting JobPosting { get; set; }

        [BindProperty]
        public IFormFile ResumeFile { get; set; }

        [BindProperty(SupportsGet = true)]
        public long JobId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            JobPosting = await _context.MisJobPostings.FirstOrDefaultAsync(m => m.Id == JobId && m.Status == Domain.Enum.Enum.JobStatus.Published);

            if (JobPosting == null)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (long.TryParse(userIdString, out long accountId))
            {
                var account = await _context.MisApplicantAccounts.FindAsync(accountId);
                if (account != null)
                {
                    Applicant = new MisJobApplicant
                    {
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        Email = account.Email,
                        Phone = account.Phone
                    };
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            JobPosting = await _context.MisJobPostings.FirstOrDefaultAsync(m => m.Id == JobId && m.Status == Domain.Enum.Enum.JobStatus.Published);
            if (JobPosting == null) return NotFound();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out long accountId))
            {
                return Unauthorized();
            }

            var account = await _context.MisApplicantAccounts.FindAsync(accountId);
            if (account == null) return Unauthorized();

            Applicant.FirstName = account.FirstName;
            Applicant.LastName = account.LastName;
            Applicant.Email = account.Email;
            Applicant.Phone = account.Phone;
            Applicant.MisApplicantAccountId = account.Id;
            Applicant.MisJobPostingId = JobId;
            Applicant.Status = Domain.Enum.Enum.ApplicationStatus.Applied;
            Applicant.CreatedAt = DateTime.UtcNow;

            // Handle file upload
            if (ResumeFile != null && ResumeFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "ats_resumes");
                Directory.CreateDirectory(uploadFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ResumeFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ResumeFile.CopyToAsync(fileStream);
                }
                Applicant.ResumeFileUrl = "/uploads/ats_resumes/" + uniqueFileName;
            }

            var passportFile = Request.Form.Files["PassportFile"];
            if (passportFile != null && passportFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "ats_passports");
                Directory.CreateDirectory(uploadFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + passportFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await passportFile.CopyToAsync(fileStream);
                }
                Applicant.PassportFileUrl = "/uploads/ats_passports/" + uniqueFileName;
            }

            var otherDocs = Request.Form.Files.GetFiles("OtherDocuments");
            foreach (var doc in otherDocs)
            {
                if (doc.Length > 0)
                {
                    string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "ats_docs");
                    Directory.CreateDirectory(uploadFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + doc.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await doc.CopyToAsync(fileStream);
                    }
                    Applicant.Documents.Add(new MisJobApplicantDocument
                    {
                        Title = doc.FileName,
                        FileUrl = "/uploads/ats_docs/" + uniqueFileName,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            _context.MisJobApplicants.Add(Applicant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Track");
        }
    }
}
