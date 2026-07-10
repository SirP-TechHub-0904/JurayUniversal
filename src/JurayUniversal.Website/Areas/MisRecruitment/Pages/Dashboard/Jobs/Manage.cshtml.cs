using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard.Jobs
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
        public MisJobPosting MisJobPosting { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id.HasValue)
            {
                MisJobPosting = await _context.MisJobPostings.FirstOrDefaultAsync(m => m.Id == id.Value);
                if (MisJobPosting == null)
                {
                    return NotFound();
                }
            }
            else
            {
                MisJobPosting = new MisJobPosting { Status = Domain.Enum.Enum.JobStatus.Draft };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (MisJobPosting.Id == 0)
            {
                MisJobPosting.CreatedAt = System.DateTime.UtcNow;
                _context.MisJobPostings.Add(MisJobPosting);
                TempData["success"] = "Job Posting successfully created.";
            }
            else
            {
                var entity = await _context.MisJobPostings.FindAsync(MisJobPosting.Id);
                if (entity == null) return NotFound();

                entity.Title = MisJobPosting.Title;
                entity.Description = MisJobPosting.Description;
                entity.AmountOrSalary = MisJobPosting.AmountOrSalary;
                entity.RequiredSkills = MisJobPosting.RequiredSkills;
                entity.Benefits = MisJobPosting.Benefits;
                entity.JobType = MisJobPosting.JobType;
                entity.Status = MisJobPosting.Status;
                entity.ClosingDate = MisJobPosting.ClosingDate;

                _context.Attach(entity).State = EntityState.Modified;
                TempData["success"] = "Job Posting successfully updated.";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
