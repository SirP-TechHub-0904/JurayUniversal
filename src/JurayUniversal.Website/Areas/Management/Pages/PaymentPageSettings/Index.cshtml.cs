using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Management.Pages.PaymentPageSettings
{
    [Authorize(Roles = "mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public SuperSetting SuperSetting { get; set; }

        public async Task OnGetAsync()
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
        }
    }
}