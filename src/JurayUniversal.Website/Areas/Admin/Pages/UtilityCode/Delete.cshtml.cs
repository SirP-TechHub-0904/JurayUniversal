using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Areas.Admin.Pages.UtilityCode
{
    public class DeleteModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DeleteModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IList<Training> Training { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            
           var xTraining = await _context.Trainings.ToListAsync();
            foreach(var x in xTraining)
            {
                _context.Trainings.Remove(x);
            }

           var yTraining = await _context.TrainingAttendances.ToListAsync();
            foreach (var x in yTraining)
            {
                _context.TrainingAttendances.Remove(x);
            }
            await _context.SaveChangesAsync();
            return Page();
        }

     }
}
