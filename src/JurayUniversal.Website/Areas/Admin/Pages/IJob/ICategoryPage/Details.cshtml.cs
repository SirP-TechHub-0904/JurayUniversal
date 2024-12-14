using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Areas.Admin.Pages.IJob.ICategoryPage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public JobCategory JobCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobCategory = await _context.JobCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (JobCategory == null)
            {
                return NotFound();
            }

            PlanList = await _context.Plans.Where(x => x.JobCategoryId == JobCategory.Id).ToListAsync();
            return Page();
        }

        [BindProperty]
        public Plan Plan { get; set; }
        public List<Plan> PlanList { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            
            _context.Plans.Add(Plan);
            await _context.SaveChangesAsync();
            TempData["success"] = "successful";
            return RedirectToPage("./Details", new {id = Plan.JobCategoryId});
        }
    }
}
