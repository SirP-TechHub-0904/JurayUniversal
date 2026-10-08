using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.CommunityForms
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisCommunityFormSubmission> Submissions { get; set; } = new List<MisCommunityFormSubmission>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        public int TotalCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }

        public async Task OnGetAsync()
        {
            TotalCount = await _context.MisCommunityFormSubmissions.CountAsync();
            PendingCount = await _context.MisCommunityFormSubmissions.CountAsync(x => x.Status == "Pending");
            ApprovedCount = await _context.MisCommunityFormSubmissions.CountAsync(x => x.Status == "Approved");
            RejectedCount = await _context.MisCommunityFormSubmissions.CountAsync(x => x.Status == "Rejected");

            var query = _context.MisCommunityFormSubmissions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(StatusFilter))
            {
                query = query.Where(x => x.Status == StatusFilter);
            }

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                var term = SearchString.Trim().ToLower();
                query = query.Where(x =>
                    x.FullName.ToLower().Contains(term) ||
                    (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(term)) ||
                    (x.Email != null && x.Email.ToLower().Contains(term)) ||
                    (x.ClanName != null && x.ClanName.ToLower().Contains(term)) ||
                    (x.FamilyKindredName != null && x.FamilyKindredName.ToLower().Contains(term)) ||
                    (x.ReferenceNumber != null && x.ReferenceNumber.ToLower().Contains(term))
                );
            }

            Submissions = await query
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(long id, string status)
        {
            var item = await _context.MisCommunityFormSubmissions.FindAsync(id);
            if (item != null)
            {
                item.Status = status;
                item.ReviewedAtUtc = System.DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["success"] = $"Submission status updated to {status}.";
            }
            return RedirectToPage(new { SearchString, StatusFilter });
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var item = await _context.MisCommunityFormSubmissions.FindAsync(id);
            if (item != null)
            {
                _context.MisCommunityFormSubmissions.Remove(item);
                await _context.SaveChangesAsync();
                TempData["success"] = "Community form submission deleted successfully.";
            }
            return RedirectToPage(new { SearchString, StatusFilter });
        }
    }
}
