using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.SettingPage.IAccomodation
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public Accomodation Accomodation { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Accomodation = await _context.Accomodations
                .Include(a => a.ParticipantList)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Accomodation == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
