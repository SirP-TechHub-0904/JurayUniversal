using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Platform
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<EnrolledSchemaDto> EnrolledSchemas { get; set; } = new List<EnrolledSchemaDto>();
        public IList<MisReportSubmission> MyRecentSubmissions { get; set; } = new List<MisReportSubmission>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var now = DateTime.Now;
            var today = now.Date;

            // 1. Get recent actual submissions
            MyRecentSubmissions = await _context.MisReportSubmissions
                .Include(s => s.MisReportSchema)
                .Where(s => s.UserProfileId == user.Id)
                .OrderByDescending(s => s.SubmittedAt)
                .Take(10)
                .ToListAsync();

            // 2. Fetch enrolled schemas
            var myParticipants = await _context.MisReportParticipants
                .Include(p => p.MisReportSchema)
                .Where(p => p.UserProfileId == user.Id && p.MisReportSchema.IsActive)
                .ToListAsync();

            foreach (var p in myParticipants)
            {
                var schema = p.MisReportSchema;
                
                var todaySubmission = await _context.MisReportSubmissions
                    .FirstOrDefaultAsync(s => s.MisReportSchemaId == schema.Id && s.UserProfileId == user.Id && s.PeriodDate == today);

                string status = "Upcoming";
                
                bool isDueToday = false;
                if (schema.Frequency == "Daily")
                {
                    isDueToday = true;
                }
                else if (schema.Frequency == "Weekly" && schema.DeadlineDayOfWeek.HasValue)
                {
                    if (now.DayOfWeek == schema.DeadlineDayOfWeek.Value)
                        isDueToday = true;
                }
                else if (schema.Frequency == "Monthly" && schema.DeadlineDayOfMonth.HasValue)
                {
                    if (now.Day == schema.DeadlineDayOfMonth.Value)
                        isDueToday = true;
                }

                if (isDueToday)
                {
                    if (todaySubmission != null)
                    {
                        status = todaySubmission.Status == "OnTime" ? "Submitted (On-Time)" : "Submitted (Late)";
                    }
                    else
                    {
                        var deadlineTime = today.Add(schema.DeadlineTime);
                        status = (now > deadlineTime) ? "Missed" : "Pending";
                    }
                }
                else
                {
                    if (todaySubmission != null)
                    {
                        status = todaySubmission.Status == "OnTime" ? "Submitted (On-Time)" : "Submitted (Late)";
                    }
                    else
                    {
                        status = "Not Due Today";
                    }
                }

                EnrolledSchemas.Add(new EnrolledSchemaDto
                {
                    SchemaId = schema.Id,
                    Name = schema.Name,
                    Description = schema.Description,
                    DeadlineTime = schema.DeadlineTime,
                    Frequency = schema.Frequency,
                    TodayStatus = status,
                    TodaySubmissionId = todaySubmission?.Id
                });
            }

            return Page();
        }
    }

    public class EnrolledSchemaDto
    {
        public long SchemaId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan DeadlineTime { get; set; }
        public string Frequency { get; set; }
        public string TodayStatus { get; set; } // "Submitted (On-Time)", "Submitted (Late)", "Pending", "Missed", "Not Due Today"
        public long? TodaySubmissionId { get; set; }
    }
}
