using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Staff.Pages.Dashboard
{
    [Authorize]
    public class UserSalaryModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public UserSalaryModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<Salary> SalaryHistory { get; set; } = new List<Salary>();
        public string Title { get; set; } = "Salary History";
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync()
        {
            Title = "Salary History";
 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                SalaryHistory = new List<Salary>();
                return;
            }

            var query = _context.Salarys
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .AsQueryable();

             
            SalaryHistory = await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
    }
}