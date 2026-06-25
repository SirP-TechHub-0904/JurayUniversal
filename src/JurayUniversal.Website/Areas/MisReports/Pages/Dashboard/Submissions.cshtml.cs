using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard
{
    [Authorize(Policy = "MisReportsViewer")]
    public class SubmissionsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public SubmissionsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisReportSubmission> Submissions { get; set; }
        public IList<MissedReportDto> MissedReports { get; set; } = new List<MissedReportDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            Submissions = await _context.MisReportSubmissions
                .Include(s => s.MisReportSchema)
                .Include(s => s.UserProfile)
                .Include(s => s.TimeLogs)
                .OrderByDescending(s => s.SubmittedAt)
                .Take(200) // limit for performance
                .ToListAsync();

            // Compute missed reports in the last 14 days
            var now = DateTime.Now;
            var today = now.Date;
            var fourteenDaysAgo = today.AddDays(-14);

            var recentSubmissions = await _context.MisReportSubmissions
                .Where(s => s.PeriodDate >= fourteenDaysAgo)
                .Select(s => new { s.MisReportSchemaId, s.UserProfileId, s.PeriodDate })
                .ToListAsync();

            var submissionSet = new HashSet<(long schemaId, string userId, DateTime date)>(
                recentSubmissions.Select(s => (s.MisReportSchemaId, s.UserProfileId, s.PeriodDate.Date))
            );

            var schemas = await _context.MisReportSchemas
                .Include(s => s.Participants)
                    .ThenInclude(p => p.UserProfile)
                .Where(s => s.IsActive)
                .ToListAsync();

            var missedList = new List<MissedReportDto>();

            for (int i = 14; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                
                foreach (var schema in schemas)
                {
                    bool isDue = false;
                    if (schema.Frequency == "Daily")
                    {
                        isDue = true;
                    }
                    else if (schema.Frequency == "Weekly" && schema.DeadlineDayOfWeek.HasValue)
                    {
                        if (date.DayOfWeek == schema.DeadlineDayOfWeek.Value)
                            isDue = true;
                    }
                    else if (schema.Frequency == "Monthly" && schema.DeadlineDayOfMonth.HasValue)
                    {
                        if (date.Day == schema.DeadlineDayOfMonth.Value)
                            isDue = true;
                    }

                    if (isDue)
                    {
                        if (date == today)
                        {
                            var deadlineTime = today.Add(schema.DeadlineTime);
                            if (now <= deadlineTime)
                            {
                                continue;
                            }
                        }

                        foreach (var participant in schema.Participants)
                        {
                            if (participant.UserProfile == null) continue;

                            if (!submissionSet.Contains((schema.Id, participant.UserProfileId, date)))
                            {
                                missedList.Add(new MissedReportDto
                                {
                                    SchemaId = schema.Id,
                                    SchemaName = schema.Name,
                                    UserId = participant.UserProfileId,
                                    UserFullName = $"{participant.UserProfile.FirstName} {participant.UserProfile.LastName}",
                                    UserEmail = participant.UserProfile.Email,
                                    Date = date,
                                    Deadline = date.Add(schema.DeadlineTime)
                                });
                            }
                        }
                    }
                }
            }

            MissedReports = missedList.OrderByDescending(m => m.Deadline).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateCommentAsync(long submissionId, string managerComment)
        {
            var submission = await _context.MisReportSubmissions.FindAsync(submissionId);
            if (submission != null)
            {
                submission.ManagerComment = managerComment;
                await _context.SaveChangesAsync();
                TempData["success"] = "Comment updated successfully.";
            }
            return RedirectToPage();
        }
    }

    public class MissedReportDto
    {
        public long SchemaId { get; set; }
        public string SchemaName { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
    }
}
