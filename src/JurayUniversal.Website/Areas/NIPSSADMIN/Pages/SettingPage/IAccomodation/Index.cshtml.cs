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

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<Accomodation> Accomodation { get;set; }

        public int AccomodationListCount {  get;set; }
        public int TakenAccomodation { get;set; }
        public async Task OnGetAsync()
        {
            Accomodation = await _context.Accomodations
                .Include(a => a.ParticipantList).ThenInclude(x => x.User)
                .ToListAsync();


            AccomodationListCount = Accomodation.Count();
            TakenAccomodation = Accomodation
                .Where(x => x.ParticipantListId != null)
                .Count();


        }
    }
}
