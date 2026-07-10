using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class TransactionDetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public TransactionDetailsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisFinanceTransaction Transaction { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Transaction = await _context.MisFinanceTransactions
                .Include(t => t.UserProfile)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Transaction == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
