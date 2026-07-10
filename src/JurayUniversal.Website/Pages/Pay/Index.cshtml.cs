using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Pages.Pay
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public List<MisFinanceCategory> Categories { get; set; } = new();
        public string OrgName { get; set; } = "Payment Portal";

        public async Task OnGetAsync()
        {
            var setting = await _context.SuperSettings.FirstOrDefaultAsync();
            if (setting != null)
                OrgName = setting.CompanyName ?? OrgName;

            Categories = await _context.MisFinanceCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.IsCompulsory ? 0 : 1)
                .ThenBy(c => c.Title)
                .ToListAsync();
        }
    }
}
