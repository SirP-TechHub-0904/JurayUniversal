using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]

    public class RemoveUserModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public RemoveUserModel(SignInManager<Profile> signInManager,
             UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public Profile UserDatas { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);

            if (UserDatas == null)
            {
                return NotFound();
            }

            return Page();
        }

        [BindProperty]
        public Accomodation Accomodation { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                //var user = await _userManager.FindByIdAsync(id);

                //var additional = await _context.AdditionalProfiles.Where(p => p.ProfileId == user.Id).ToListAsync();
                //foreach (var item in additional)
                //{
                //    _context.AdditionalProfiles.Remove(item);
                //}
                //var procat = await _context.ProfileCategories.Where(p => p.ProfileId == user.Id).ToListAsync();
                //foreach (var item in procat)
                //{
                //    _context.ProfileCategories.Remove(item);
                //}
                //var fprofileinfo = await _context.ProfileCategoryLists.Where(p => p.ProfileId == user.Id).ToListAsync();
                //foreach (var item in fprofileinfo)
                //{
                //    _context.ProfileCategoryLists.Remove(item);
                //}
                //var staffprofileinfo = await _context.StaffManagers.Where(p => p.UserId == user.Id).ToListAsync();
                //foreach (var item in staffprofileinfo)
                //{
                //    _context.StaffManagers.Remove(item);
                //}
                //var participant = await _context.ParticipantLists.Where(x => x.UserId == user.Id).ToListAsync();
                //foreach (var item in participant)
                //{
                //    var accomodation = _context.Accomodations.Where(x => x.ParticipantListId == item.Id).AsEnumerable();
                //    foreach (var itemlist in accomodation)
                //    {
                //        itemlist.ParticipantListId = null;
                //        _context.Attach(itemlist).State = EntityState.Modified;
                //    }
                //    _context.ParticipantLists.Remove(item);
                //}
                //await _context.SaveChangesAsync();
                //await _userManager.DeleteAsync(user);

            }
            catch (Exception c) { }
            return RedirectToPage("./Index");
        }
    }
}
