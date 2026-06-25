using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Platform
{
    [Authorize]
    public class SchemaDetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public SchemaDetailsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisReportSchema Schema { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }

        public int OnTimeCount { get; set; }
        public int LateCount { get; set; }
        public int MissedCount { get; set; }
        public int ComplianceRate { get; set; }

        public bool CanSubmitToday { get; set; }
        public long? TodaySubmissionId { get; set; }
        public bool TodayIsMissed { get; set; }

        public IList<DayStatusDto> DayStatuses { get; set; } = new List<DayStatusDto>();
        public IList<CalendarDayDto> CalendarDays { get; set; } = new List<CalendarDayDto>();

        public async Task<IActionResult> OnGetAsync(long schemaId, int? year, int? month)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Schema = await _context.MisReportSchemas.FindAsync(schemaId);
            if (Schema == null) return NotFound();

            // Verify participant enrollment
            bool isEnrolled = await _context.MisReportParticipants
                .AnyAsync(p => p.MisReportSchemaId == schemaId && p.UserProfileId == user.Id);

            if (!isEnrolled)
            {
                TempData["error"] = "You are not enrolled in this report schedule.";
                return RedirectToPage("./Index");
            }

            var now = DateTime.Now;
            Year = year ?? now.Year;
            Month = month ?? now.Month;
            
            // Limit navigation to reasonable range
            if (Year < 2000 || Year > 2100 || Month < 1 || Month > 12)
            {
                Year = now.Year;
                Month = now.Month;
            }

            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);

            var startDate = new DateTime(Year, Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var today = now.Date;

            // Get submissions in this month
            var dbSubmissions = await _context.MisReportSubmissions
                .Where(s => s.MisReportSchemaId == schemaId && s.UserProfileId == user.Id && s.PeriodDate >= startDate && s.PeriodDate <= endDate)
                .ToListAsync();

            // Evaluate statuses for all calendar days
            int daysInMonth = DateTime.DaysInMonth(Year, Month);
            for (int d = 1; d <= daysInMonth; d++)
            {
                var date = new DateTime(Year, Month, d);
                
                // Is this day due based on frequency?
                bool isDue = false;
                if (Schema.Frequency == "Daily")
                {
                    isDue = true;
                }
                else if (Schema.Frequency == "Weekly" && Schema.DeadlineDayOfWeek.HasValue)
                {
                    if (date.DayOfWeek == Schema.DeadlineDayOfWeek.Value)
                        isDue = true;
                }
                else if (Schema.Frequency == "Monthly" && Schema.DeadlineDayOfMonth.HasValue)
                {
                    if (date.Day == Schema.DeadlineDayOfMonth.Value)
                        isDue = true;
                }

                if (isDue)
                {
                    var sub = dbSubmissions.FirstOrDefault(s => s.PeriodDate.Date == date);

                    if (sub != null)
                    {
                        DayStatuses.Add(new DayStatusDto
                        {
                            Date = date,
                            Status = sub.Status, // "OnTime", "Late"
                            SubmissionId = sub.Id,
                            SubmittedAt = sub.SubmittedAt
                        });
                    }
                    else
                    {
                        if (date < today)
                        {
                            DayStatuses.Add(new DayStatusDto
                            {
                                Date = date,
                                Status = "Missed"
                            });
                        }
                        else if (date == today)
                        {
                            var deadlineTime = today.Add(Schema.DeadlineTime);
                            if (now > deadlineTime)
                            {
                                DayStatuses.Add(new DayStatusDto
                                {
                                    Date = date,
                                    Status = "Missed"
                                });
                            }
                            else
                            {
                                DayStatuses.Add(new DayStatusDto
                                {
                                    Date = date,
                                    Status = "Pending"
                                });
                            }
                        }
                        else
                        {
                            DayStatuses.Add(new DayStatusDto
                            {
                                Date = date,
                                Status = "Upcoming"
                            });
                        }
                    }
                }
                else
                {
                    // Check if submitted even though not strictly due (ad-hoc submission)
                    var sub = dbSubmissions.FirstOrDefault(s => s.PeriodDate.Date == date);
                    if (sub != null)
                    {
                        DayStatuses.Add(new DayStatusDto
                        {
                            Date = date,
                            Status = sub.Status,
                            SubmissionId = sub.Id,
                            SubmittedAt = sub.SubmittedAt
                        });
                    }
                    else
                    {
                        DayStatuses.Add(new DayStatusDto
                        {
                            Date = date,
                            Status = "NotDue"
                        });
                    }
                }
            }

            // Calculate stats for checked days (up to today)
            var checkedDays = DayStatuses.Where(ds => ds.Date <= today).ToList();
            
            OnTimeCount = checkedDays.Count(ds => ds.Status == "OnTime");
            LateCount = checkedDays.Count(ds => ds.Status == "Late");
            MissedCount = checkedDays.Count(ds => ds.Status == "Missed");

            int totalSubmitted = OnTimeCount + LateCount;
            int totalDue = totalSubmitted + MissedCount;
            ComplianceRate = totalDue > 0 ? (int)Math.Round((double)OnTimeCount / totalDue * 100) : 100;

            // Action flags for today
            var todayStatus = DayStatuses.FirstOrDefault(ds => ds.Date == today);
            if (todayStatus != null)
            {
                CanSubmitToday = todayStatus.Status == "Pending";
                TodaySubmissionId = todayStatus.SubmissionId;
                TodayIsMissed = todayStatus.Status == "Missed";
            }

            // Construct Calendar Grid
            // Monday start offset (0 = Monday, 6 = Sunday)
            int startOffset = ((int)startDate.DayOfWeek + 6) % 7;

            // Padding cells before day 1
            for (int i = 0; i < startOffset; i++)
            {
                CalendarDays.Add(new CalendarDayDto { Status = "Empty" });
            }

            // Add actual calendar days
            for (int d = 1; d <= daysInMonth; d++)
            {
                var date = new DateTime(Year, Month, d);
                var dayStatus = DayStatuses.FirstOrDefault(ds => ds.Date == date);

                CalendarDays.Add(new CalendarDayDto
                {
                    Date = date,
                    DayNumber = d,
                    Status = dayStatus?.Status ?? "NotDue",
                    SubmissionId = dayStatus?.SubmissionId,
                    IsToday = (date == today)
                });
            }

            // Padding cells after last day to complete the week
            while (CalendarDays.Count % 7 != 0)
            {
                CalendarDays.Add(new CalendarDayDto { Status = "Empty" });
            }

            return Page();
        }
    }

    public class DayStatusDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public long? SubmissionId { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }

    public class CalendarDayDto
    {
        public DateTime? Date { get; set; }
        public int DayNumber { get; set; }
        public string Status { get; set; } // "OnTime", "Late", "Missed", "Pending", "Upcoming", "NotDue", "Empty"
        public long? SubmissionId { get; set; }
        public bool IsToday { get; set; }
    }
}
