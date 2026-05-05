using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.IMeetingAttendance
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Secretary")]

    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(long? id)
        {
            await LoadUsers();

            Meeting = await _context.Meetings.FirstOrDefaultAsync(x=>x.Id == id);
            if(Meeting == null)
            {
                TempData["success"] = "Error Invalid Meeting.";
                return RedirectToPage("/IMeetings/Index");
            }
            return Page();
        }
        [BindProperty]
        public Meeting Meeting { get; set; }

        [BindProperty]
        public DateTime? MeetingTime { get; set; }

        [BindProperty]
        public MeetingAttendance MeetingAttendance { get; set; }
        private async Task LoadUsers()
        {
            var excludedRoles = new List<string> { "mSuperAdmin", "SubAdmin" };

            var users = await _userManager.Users.ToListAsync();

            var filteredUsers = new List<SelectListItem>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // skip excluded roles
                if (roles.Any(r => excludedRoles.Contains(r)))
                    continue;

                var fullname = (user.FirstName + " " + user.MiddleName + " " + user.LastName).Trim();

                filteredUsers.Add(new SelectListItem
                {
                    Value = user.Id,
                    Text = string.IsNullOrWhiteSpace(fullname) ? user.Email ?? user.UserName : fullname
                });
            }

            ViewData["UserId"] = filteredUsers
                .OrderBy(x => x.Text)
                .ToList();
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
           if(MeetingAttendance.Time.Date != MeetingTime.Value.Date)
            {
                Meeting = await _context.Meetings.FirstOrDefaultAsync(x => x.Id == MeetingAttendance.MeetingId);
                if (Meeting == null)
                {
                    TempData["success"] = "Error Invalid Meeting.";
                    return RedirectToPage("/IMeetings/Index");
                }
                await LoadUsers();
                TempData["error"] = "Date not in range";
                return Page();
            }
            if (MeetingAttendance.Time.TimeOfDay < MeetingTime.Value.TimeOfDay)
            {
                Meeting = await _context.Meetings.FirstOrDefaultAsync(x => x.Id == MeetingAttendance.MeetingId);
                if (Meeting == null)
                {
                    TempData["success"] = "Error Invalid Meeting.";
                    return RedirectToPage("/IMeetings/Index");
                }
                await LoadUsers();
                TempData["error"] = "Time not in range";
                return Page();
            }
            _context.MeetingAttendances.Add(MeetingAttendance);
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("/IMeetings/Details", new {id = MeetingAttendance.MeetingId});
        }
    }
}
