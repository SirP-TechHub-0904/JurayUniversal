using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.EmailLogs
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisNewsletterEmailLog> Logs { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync(int? page)
        {
            var query = _context.MisNewsletterEmailLogs.Include(l => l.Campaign).AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(x => x.EmailAddress.Contains(SearchTerm) || x.Subject.Contains(SearchTerm));
            }

            int pageSize = 50;
            PageNumber = (page ?? 1);
            
            int totalCount = await query.CountAsync();
            TotalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

            Logs = await query
                .OrderByDescending(x => x.SentDatetime)
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var log = await _context.MisNewsletterEmailLogs.FindAsync(id);
            if (log != null)
            {
                _context.MisNewsletterEmailLogs.Remove(log);
                await _context.SaveChangesAsync();
                TempData["success"] = "Email log deleted successfully.";
            }
            return RedirectToPage("./Index", new { page = 1, SearchTerm = SearchTerm });
        }

        public async Task<IActionResult> OnPostResendAsync(long id)
        {
            var log = await _context.MisNewsletterEmailLogs.FindAsync(id);
            if (log != null)
            {
                log.Status = JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Pending;
                log.RetryCount = 0; // Reset retry count
                await _context.SaveChangesAsync();
                TempData["success"] = "Email queued for resending.";
            }
            return RedirectToPage("./Index", new { page = 1, SearchTerm = SearchTerm });
        }
    }
}
