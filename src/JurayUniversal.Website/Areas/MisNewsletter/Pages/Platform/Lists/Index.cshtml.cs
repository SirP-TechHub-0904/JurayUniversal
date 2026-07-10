using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Platform.Lists
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<MisNewsletterList> Lists { get; set; } = new List<MisNewsletterList>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Lists = await _context.MisNewsletterLists
                .Include(l => l.Contacts)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var userId = _userManager.GetUserId(User);
            var list = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
            
            if (list == null) return NotFound();

            _context.MisNewsletterLists.Remove(list);
            await _context.SaveChangesAsync();

            TempData["success"] = "Mailing list deleted successfully.";
            return RedirectToPage();
        }
    }
}

