using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Admin.Pages.XProjectPage.XT
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<XProjectTask> XProjectTask { get;set; }

        public async Task OnGetAsync()
        {
            XProjectTask = await _context.XProjectTasks
                .Include(x => x.ApprovedBy)
                .Include(x => x.TestedBy)
                .Include(x => x.User).ToListAsync();
        }
    }
}
