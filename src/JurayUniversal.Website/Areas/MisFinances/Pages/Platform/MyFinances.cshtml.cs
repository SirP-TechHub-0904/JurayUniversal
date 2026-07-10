using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisFinances.Pages.Platform
{
    [Authorize]
    public class MyFinancesModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public MyFinancesModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<MisFinanceCategory> OutstandingDues { get; set; } = new();
        public List<MisFinanceCategory> OptionalCauses { get; set; } = new();
        public List<MisFinanceTransaction> MyTransactions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;

            var allActive = await _context.MisFinanceCategories
                .Where(c => c.IsActive)
                .ToListAsync();

            // Compulsory dues this user has NOT yet paid
            var paidCategoryIds = await _context.MisFinanceTransactions
                .Where(t => t.UserId == user.Id && t.Status == PaymentStatus.Success)
                .Select(t => t.MisFinanceCategoryId)
                .ToListAsync();

            OutstandingDues = allActive
                .Where(c => c.IsCompulsory && !paidCategoryIds.Contains(c.Id))
                .ToList();

            OptionalCauses = allActive
                .Where(c => !c.IsCompulsory)
                .ToList();

            MyTransactions = await _context.MisFinanceTransactions
                .Include(t => t.Category)
                .Where(t => t.UserId == user.Id)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
