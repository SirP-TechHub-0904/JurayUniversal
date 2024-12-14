using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.Admin.Pages.IOffice.Activity
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context; _userManager = userManager;
        }

        [BindProperty]
        public OfficeActivityDialy OfficeActivityDialy { get; set; }

        [BindProperty]
        public string INote { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OfficeActivityDialy = await _context.OfficeActivityDialies
                .Include(o => o.LastUpdateBy)
                .Include(o => o.OfficeActivityCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (OfficeActivityDialy == null)
            {
                return NotFound();
            }
            INote = "<br>=====<br> Last Log Before Update <br> Updated By: " + OfficeActivityDialy.LastUpdateBy.FullnameX 
                +" <br> Date: "+OfficeActivityDialy.LastUpdate+" <br> Note: "+OfficeActivityDialy.Note  +" <br> Amount: "+OfficeActivityDialy.Amount
                +" <br> Customer: "+OfficeActivityDialy.Customer ;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
                        var user = await _userManager.GetUserAsync(User);
            OfficeActivityDialy.Logs = OfficeActivityDialy.Logs + "<br>"+ INote;
            OfficeActivityDialy.LastUpdateById = user.Id;
            OfficeActivityDialy.LastUpdate = DateTime.UtcNow.AddHours(1);
            OfficeActivityDialy.UpdateCount = OfficeActivityDialy.UpdateCount + 1;

            
            _context.Attach(OfficeActivityDialy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeActivityDialyExists(OfficeActivityDialy.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new {id = OfficeActivityDialy.Id});;
        }

        private bool OfficeActivityDialyExists(long id)
        {
            return _context.OfficeActivityDialies.Any(e => e.Id == id);
        }
    }
}
