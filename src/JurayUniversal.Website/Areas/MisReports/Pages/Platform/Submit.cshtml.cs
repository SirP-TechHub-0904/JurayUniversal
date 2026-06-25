using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Platform
{
    [Authorize]
    public class SubmitModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly JurayUniversal.Application.Services.AWS.IStorageService _storageService;

        public SubmitModel(DashboardDbContext context, UserManager<Profile> userManager, JurayUniversal.Application.Services.AWS.IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _storageService = storageService;
        }

        public MisReportSchema Schema { get; set; }

        [BindProperty]
        public MisReportSubmission Submission { get; set; }

        [BindProperty]
        public List<MisReportTimeLog> TimeLogs { get; set; } = new List<MisReportTimeLog>();

        [BindProperty]
        public long SchemaId { get; set; }

        public async Task<IActionResult> OnGetAsync(long schemaId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Schema = await _context.MisReportSchemas.FindAsync(schemaId);
            if (Schema == null) return NotFound();

            // Verify they are enrolled
            bool isEnrolled = await _context.MisReportParticipants.AnyAsync(p => p.MisReportSchemaId == schemaId && p.UserProfileId == user.Id);
            if (!isEnrolled)
            {
                TempData["error"] = "You are not required to submit this report.";
                return RedirectToPage("./Index");
            }

            var now = DateTime.Now;
            var today = now.Date;

            // Check if already submitted today
            bool submitted = await _context.MisReportSubmissions
                .AnyAsync(s => s.MisReportSchemaId == schemaId && s.UserProfileId == user.Id && s.PeriodDate == today);

            if (submitted)
            {
                TempData["warning"] = "You have already submitted this report for today.";
                return RedirectToPage("./Index");
            }

            // Check if deadline has passed
            var deadlineTime = today.Add(Schema.DeadlineTime);
            if (now > deadlineTime)
            {
                TempData["error"] = "The submission deadline for today's report has passed. This report is marked as Missed.";
                return RedirectToPage("./Index");
            }

            SchemaId = schemaId;

            if (Schema.RequireTimeLog)
            {
                var workStart = Schema.WorkStartTime ?? new TimeSpan(8, 0, 0);
                var workEnd = Schema.WorkEndTime ?? new TimeSpan(17, 0, 0);
                var breakStart = Schema.BreakStartTime ?? new TimeSpan(12, 0, 0);
                var breakEnd = Schema.BreakEndTime ?? new TimeSpan(13, 0, 0);

                if (workEnd < workStart && workEnd.Hours < 12)
                {
                    workEnd = workEnd.Add(TimeSpan.FromHours(12));
                }
                if (breakEnd < breakStart && breakEnd.Hours < 12)
                {
                    breakEnd = breakEnd.Add(TimeSpan.FromHours(12));
                }

                var current = workStart;
                var end = workEnd;
                int interval = Schema.TimeLogIntervalHours > 0 ? Schema.TimeLogIntervalHours : 1;

                while (current < end)
                {
                    var slotEnd = current.Add(TimeSpan.FromHours(interval));
                    
                    if (current >= breakStart && current < breakEnd)
                    {
                        TimeLogs.Add(new MisReportTimeLog
                        {
                            StartTime = breakStart,
                            EndTime = breakEnd,
                            IsBreak = true
                        });
                        
                        current = breakEnd;
                        continue;
                    }
                    
                    if (slotEnd > breakStart && current < breakStart)
                    {
                        slotEnd = breakStart;
                    }

                    if (slotEnd > end) slotEnd = end;

                    TimeLogs.Add(new MisReportTimeLog
                    {
                        StartTime = current,
                        EndTime = slotEnd
                    });

                    current = slotEnd;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var schema = await _context.MisReportSchemas.FindAsync(SchemaId);
            if (schema == null) return NotFound();

            //if (!ModelState.IsValid)
            //{
            //    Schema = schema;
            //    return Page();
            //}

            var now = DateTime.Now;
            var today = now.Date;

            // Check if already submitted today
            bool submitted = await _context.MisReportSubmissions
                .AnyAsync(s => s.MisReportSchemaId == SchemaId && s.UserProfileId == user.Id && s.PeriodDate == today);

            if (submitted)
            {
                TempData["warning"] = "You have already submitted this report for the current period.";
                return RedirectToPage("./Index");
            }

            // Check if deadline has passed
            var deadlineTime = today.Add(schema.DeadlineTime);
            if (now > deadlineTime)
            {
                TempData["error"] = "The submission deadline for today's report has passed. This report is marked as Missed.";
                return RedirectToPage("./Index");
            }

            // Calculate status: Late if submitted after working hours
            var workEndTime = today.Add(schema.WorkEndTime ?? new TimeSpan(17, 0, 0));
            string status = (now > workEndTime) ? "Late" : "OnTime";

            Submission.MisReportSchemaId = SchemaId;
            Submission.UserProfileId = user.Id;
            Submission.PeriodDate = today;
            Submission.SubmittedAt = now;
            Submission.Status = status;

            // Handle File Upload
            if (schema.RequireFileUpload && Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files["attachmentFile"];
                if (file != null)
                {
                    var fileResponse = await _storageService.MainUploadFileReturnUrlAsync("", file);
                    if (fileResponse != null && !string.IsNullOrEmpty(fileResponse.Url))
                    {
                        Submission.AttachmentUrl = fileResponse.Url;
                    }
                }
            }

            _context.MisReportSubmissions.Add(Submission);
            await _context.SaveChangesAsync();

            // Handle TimeLogs
            if (schema.RequireTimeLog && TimeLogs.Any())
            {
                foreach(var log in TimeLogs)
                {
                    if (log.IsBreak)
                    {
                        log.ActivityDescription = "Official Break Time";
                    }
                    log.MisReportSubmissionId = Submission.Id;
                    _context.MisReportTimeLogs.Add(log);
                }
                await _context.SaveChangesAsync();
            }

            TempData["success"] = $"Report submitted successfully ({status}).";
            return RedirectToPage("./Index");
        }
    }
}
