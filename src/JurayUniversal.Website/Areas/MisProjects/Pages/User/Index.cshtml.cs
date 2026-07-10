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

namespace JurayUniversal.Website.Areas.MisProjects.Pages.User
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

        public IList<MisProjectUser> MyProjects { get; set; } = new List<MisProjectUser>();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            
            MyProjects = await _context.MisProjectUsers
                .Include(p => p.MisProject)
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .OrderByDescending(p => p.DateAdded)
                .ToListAsync();
        }
    }
}
