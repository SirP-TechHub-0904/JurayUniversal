using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.IAttendance
{
    [Authorize(Roles = "mSuperAdmin,Manager")]
    public class TakeAttendanceModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public TakeAttendanceModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DateTime AttendanceDate { get; set; } = DateTime.UtcNow.Date;

        [BindProperty]
        public List<StaffInputModel> Staff { get; set; } = new List<StaffInputModel>();

        public class StaffInputModel
        {
            public string UserId { get; set; }
            public string FullName { get; set; }
            public bool Present { get; set; }
            public bool Late { get; set; }
            public string Note { get; set; }
        }

        public async Task OnGetAsync()
        {
            var excludedRoles = new[] { "mSuperAdmin", "Admin" };

            var staffUsers = await (
                from user in _context.Users
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                join role in _context.Roles on userRole.RoleId equals role.Id
                where !excludedRoles.Contains(role.Name)
                select user
            )
            .Distinct()
            .OrderBy(x => x.UserName)
            .ToListAsync();

            Staff = staffUsers.Select(u => new StaffInputModel
            {
                UserId = u.Id,
                FullName = string.IsNullOrWhiteSpace(u.UserName) ? u.Email : u.UserName
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Staff == null || Staff.Count == 0)
            {
                return Page();
            }

            foreach (var row in Staff)
            {
                var existing = await _context.Attendances
                    .FirstOrDefaultAsync(x => x.UserId == row.UserId && x.Date.Date == AttendanceDate.Date);

                if (existing == null)
                {
                    existing = new Attendance
                    {
                        UserId = row.UserId,
                        Date = AttendanceDate.Date
                    };
                    _context.Attendances.Add(existing);
                }

                if (!row.Present)
                {
                    existing.AttendanceStatus = JurayUniversal.Domain.Enum.Enum.AttendanceStatus.Absent;
                }
                else if (row.Late)
                {
                    existing.AttendanceStatus = JurayUniversal.Domain.Enum.Enum.AttendanceStatus.Late;
                }
                else
                {
                    existing.AttendanceStatus = JurayUniversal.Domain.Enum.Enum.AttendanceStatus.Present;
                }

                existing.Note = row.Note;
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Attendance submitted successfully";
            return RedirectToPage("./Index", new { searchdate = AttendanceDate.ToString("yyyy-MM-dd") });
        }
    }
}