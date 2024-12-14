using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class StaffMeetingModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public StaffMeetingModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Meeting Meeting { get;set; }
        public long? IDD { get; set; }
        public async Task OnGetAsync(long? id)
        {
            if (id == null)
            {
                Meeting = await _context.Meetings
                    .Include(x => x.MeetingAttendances)
                    .ThenInclude(x => x.User)
                    .OrderByDescending(x => x.StartTime).FirstOrDefaultAsync();
            }
            else
            {
                Meeting = await _context.Meetings
                   .Include(x => x.MeetingAttendances)
                   .ThenInclude(x => x.User)
                   .OrderByDescending(x => x.StartTime).FirstOrDefaultAsync(x=>x.Id == id);
            }
            //if(Meeting)
        }
        [BindProperty]
        public string Comment { get; set; }

        [BindProperty]
        public long Mid { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var meetingdata = await _context.Meetings.FindAsync(Mid);
            var user = await _userManager.GetUserAsync(User);
            if (meetingdata != null)
            {
                meetingdata.Comment = meetingdata.Comment + "<hr style=\"border:2px solid white; margin:0;padding:0;\"> @updated on " + DateTime.UtcNow.AddHours(1) + "<br> by "+ user.GetFullName() + "<br> " + Comment;
            }
            _context.Attach(meetingdata).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";
            }
            catch (DbUpdateConcurrencyException)
            { 
                    throw;
                
            }

            return RedirectToPage("./StaffMeeting");
        }


    }
}
