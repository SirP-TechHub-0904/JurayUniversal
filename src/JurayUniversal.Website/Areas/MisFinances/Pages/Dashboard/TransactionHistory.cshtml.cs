using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Dashboard
{
    [Authorize]
    public class TransactionHistoryModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public TransactionHistoryModel(DashboardDbContext context)
        {
            _context = context;
        }

        public List<Profile> Users { get; set; } = new();
        public IEnumerable<MisFinanceTransaction> Transactions { get; set; } = new List<MisFinanceTransaction>();
        public string? SelectedUserId { get; set; }

        public async Task OnGetAsync(string? userId)
        {
            Users = await _context.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToListAsync();
            SelectedUserId = userId;

            var query = _context.MisFinanceTransactions.AsQueryable();
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.UserId == userId);
            }
            Transactions = await query
                .Include(t => t.UserProfile)
                .Include(t => t.Category)
                .OrderByDescending(t => t.PaidAt)
                .ToListAsync();
        }
    }
}
