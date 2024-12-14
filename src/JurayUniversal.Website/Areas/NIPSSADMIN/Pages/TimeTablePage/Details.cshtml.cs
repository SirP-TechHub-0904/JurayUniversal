using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Text.Encodings.Web;
using System.Web.Helpers;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.TimeTablePage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Editor")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public TimeTable TimeTable { get; set; }

        [BindProperty]
        public long ModeratorId { get; set; }


        [BindProperty]
        public string? NewUserId { get; set; }

        [BindProperty]
        public string? Fullname { get; set; }

        [BindProperty]
        public string? Position { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeTable = await _context.TimeTables
                .Include(t => t.Course)
                .Include(t => t.Moderators)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (TimeTable == null)
            {
                return NotFound();
            }
            //list moderators
            var listmoderators = await _context.Moderators.Include(x => x.User).ToListAsync();
            var oulist = listmoderators.Select(x => new DropDownListCourse
            {
                Id = x.Id,
                Title = (x.User?.FullnameX ?? x.Fullname) + " (" + x.Position + ")",
            }).ToList();

            ViewData["ModeratorId"] = new SelectList(oulist, "Id", "Title");

            //list db management and staff
            var liststaff = await _context.Users.Where(x => x.Role.Contains("Staff") || x.Role.Contains("Directing") || x.Role.Contains("Management")).ToListAsync();
            var outliststaff = liststaff.Select(x => new DropDownListCourse
            {
                UserId = x.Id,
                Title = x.FullnameX,
            }).ToList();

            ViewData["UserId"] = new SelectList(outliststaff, "UserId", "Title");

            return Page();
        }


        public async Task<IActionResult> OnPostNewModeratorAsync()
        {
            Moderator mr = new Moderator();
            var getmoderator = await _context.Moderators.FindAsync(ModeratorId);
            if (getmoderator == null)
            {
                TempData["error"] = "unable to fetch moderator";
                return RedirectToPage("./Details", new { id = TimeTable.Id });
            }
            mr.Fullname = getmoderator.Fullname;
            mr.UserId = getmoderator.UserId;
            mr.Position = Position;
            mr.TimeTableId = TimeTable.Id;

            _context.Moderators.Add(mr);
            await _context.SaveChangesAsync();
            TempData["success"] = "successful";
            return RedirectToPage("./Details", new { id = TimeTable.Id });

        }


        public async Task<IActionResult> OnPostUpdateModeratorAsync()
        {
            Moderator mr = new Moderator();
            if (!String.IsNullOrEmpty(NewUserId))
            {
                var user = await _userManager.FindByIdAsync(NewUserId);
                if (user == null)
                {
                    TempData["error"] = "unable to fetch user";
                    return RedirectToPage("./Details", new { id = TimeTable.Id });

                }
                mr.UserId = user.Id;
                mr.Position = user.PositionOrRank;
            }
            else
            {
                mr.Fullname = Fullname;
                mr.Position = Position;
            }

            mr.TimeTableId = TimeTable.Id;

            _context.Moderators.Add(mr);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = TimeTable.Id });
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mr = await _context.Moderators.FindAsync(id);

            if (mr != null)
            {
                _context.Moderators.Remove(mr);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }
}
