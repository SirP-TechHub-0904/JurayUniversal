using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.SettingPage.IAccomodation
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Accomodation> Accomodation { get; set; }

        public int AccomodationListCount { get; set; }
        public int TakenAccomodation { get; set; }
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

        public async Task<IActionResult> OnPostAsync()
        {

            var TakenAccomodation = await _context.Accomodations.Include(x => x.ParticipantList)
                .Where(x => x.ParticipantListId != null)
                .ToListAsync();

            foreach (var item in TakenAccomodation)
            {
                var user = await _userManager.FindByIdAsync(item.ParticipantList.UserId);
                if (user != null)
                {
                    item.ParticipantListId = null;
                    _context.Attach(item).State = EntityState.Modified;
                    ///
                    user.LastAccomodation = item.Name;
                    user.HasAccomodation = false;
                    await _userManager.UpdateAsync(user);
                }
            }


            await _context.SaveChangesAsync();
            TempData["success"] = "Reset Successful";
            return RedirectToPage("./Index");
        }
    }
}
