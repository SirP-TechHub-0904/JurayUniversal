using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Admin.Pages.CM
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<CompanyProgram> CompanyProgram { get;set; }
        public CompanyProgramCategory Category { get; set; }

        public async Task OnGetAsync(long id)
        {
            CompanyProgram = await _context.CompanyPrograms
                .Include(c => c.CompanyProgramCategory)
                .Where(x=>x.CompanyProgramCategoryId == id)
                .ToListAsync();

            Category = await _context.CompanyProgramCategories.FirstOrDefaultAsync(x=>x.Id == id);
        }
    }
}
