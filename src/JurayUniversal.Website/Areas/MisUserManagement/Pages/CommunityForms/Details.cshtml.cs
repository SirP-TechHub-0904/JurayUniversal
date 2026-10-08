using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.CommunityForms
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public DetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisCommunityFormSubmission Submission { get; set; } = null!;

        [BindProperty]
        public string? AdminNotes { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var item = await _context.MisCommunityFormSubmissions.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            Submission = item;
            AdminNotes = item.AdminNotes;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(long id, string status)
        {
            var item = await _context.MisCommunityFormSubmissions.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Status = status;
            item.AdminNotes = AdminNotes;
            item.ReviewedAtUtc = DateTime.UtcNow;
            item.ReviewedByUserId = User.Identity?.Name;

            await _context.SaveChangesAsync();
            TempData["success"] = $"Application status updated to '{status}'.";
            return RedirectToPage(new { id });
        }
    }
}
