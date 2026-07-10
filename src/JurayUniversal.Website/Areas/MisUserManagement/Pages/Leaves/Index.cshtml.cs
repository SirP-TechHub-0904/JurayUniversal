using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Website.Filters;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Leaves
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisLeaveRequest> LeaveRequests { get; set; } = new List<MisLeaveRequest>();

        public async Task OnGetAsync()
        {
            LeaveRequests = await _context.MisLeaveRequests
                .Include(x => x.User)
                .OrderByDescending(x => x.DateSubmitted)
                .ToListAsync();
        }
    }
}
