using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.PhoneBooks
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisPhoneBook> GlobalPhoneBooks { get; set; } = new List<MisPhoneBook>();

        public async Task<IActionResult> OnGetAsync()
        {
            GlobalPhoneBooks = await _context.MisPhoneBooks
                .Include(p => p.Contacts)
                .Where(p => p.UserId == null)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
