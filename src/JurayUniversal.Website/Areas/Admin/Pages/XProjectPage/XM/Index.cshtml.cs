using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Admin.Pages.XProjectPage.XM
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<XProjectMessage> XProjectMessage { get;set; }

        public async Task OnGetAsync()
        {
            XProjectMessage = await _context.XProjectMessages
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.XProject).ToListAsync();
        }
    }
}
