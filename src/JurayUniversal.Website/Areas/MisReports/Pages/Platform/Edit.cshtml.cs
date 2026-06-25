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
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly JurayUniversal.Application.Services.AWS.IStorageService _storageService;

        public EditModel(DashboardDbContext context, UserManager<Profile> userManager, JurayUniversal.Application.Services.AWS.IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _storageService = storageService;
        }

        [BindProperty]
        public MisReportSubmission Submission { get; set; }

        [BindProperty]
        public List<MisReportTimeLog> TimeLogs { get; set; } = new List<MisReportTimeLog>();

        public MisReportSchema Schema { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Submission = await _context.MisReportSubmissions
                .Include(s => s.MisReportSchema)
                .Include(s => s.TimeLogs)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Submission == null) return NotFound();

            // Verify ownership
            if (Submission.UserProfileId != user.Id) return Challenge();

            Schema = Submission.MisReportSchema;

            // Ensure the deadline has not passed
            var targetTime = Submission.PeriodDate.Add(Schema.DeadlineTime);
            if (DateTime.Now > targetTime)
            {
                TempData["error"] = "You cannot edit this report because the submission deadline has passed.";
                return RedirectToPage("./Details", new { id = Submission.Id });
            }

            TimeLogs = Submission.TimeLogs.OrderBy(l => l.StartTime).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var existingSubmission = await _context.MisReportSubmissions
                .Include(s => s.MisReportSchema)
                .Include(s => s.TimeLogs)
                .FirstOrDefaultAsync(s => s.Id == Submission.Id);

            if (existingSubmission == null) return NotFound();
            if (existingSubmission.UserProfileId != user.Id) return Challenge();

            Schema = existingSubmission.MisReportSchema;

            // Double check the deadline
            var targetTime = existingSubmission.PeriodDate.Add(Schema.DeadlineTime);
            if (DateTime.Now > targetTime)
            {
                TempData["error"] = "You cannot edit this report because the submission deadline has passed.";
                return RedirectToPage("./Details", new { id = existingSubmission.Id });
            }

            // Update main submission fields
            existingSubmission.Content = Submission.Content;
            existingSubmission.Achievements = Submission.Achievements;
            existingSubmission.Challenges = Submission.Challenges;
            existingSubmission.PlansForNextPeriod = Submission.PlansForNextPeriod;
            existingSubmission.SubmittedAt = DateTime.Now;

            // Re-evaluate status based on WorkEndTime
            var workEndTime = existingSubmission.PeriodDate.Add(Schema.WorkEndTime ?? new TimeSpan(17, 0, 0));
            existingSubmission.Status = (DateTime.Now > workEndTime) ? "Late" : "OnTime";

            // Handle File Upload if there's a new attachment
            if (Schema.RequireFileUpload && Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files["attachmentFile"];
                if (file != null)
                {
                    var fileResponse = await _storageService.MainUploadFileReturnUrlAsync("", file);
                    if (fileResponse != null && !string.IsNullOrEmpty(fileResponse.Url))
                    {
                        existingSubmission.AttachmentUrl = fileResponse.Url;
                    }
                }
            }

            // Update TimeLogs
            if (Schema.RequireTimeLog && TimeLogs.Any())
            {
                foreach (var log in TimeLogs)
                {
                    var existingLog = existingSubmission.TimeLogs.FirstOrDefault(l => l.StartTime == log.StartTime && l.EndTime == log.EndTime);
                    if (existingLog != null)
                    {
                        if (log.IsBreak)
                        {
                            existingLog.ActivityDescription = "Official Break Time";
                        }
                        else
                        {
                            existingLog.ActivityDescription = log.ActivityDescription;
                        }
                    }
                    else
                    {
                        // If for some reason it didn't exist, create it
                        var newLog = new MisReportTimeLog
                        {
                            StartTime = log.StartTime,
                            EndTime = log.EndTime,
                            ActivityDescription = log.IsBreak ? "Official Break Time" : log.ActivityDescription,
                            MisReportSubmissionId = existingSubmission.Id
                        };
                        _context.MisReportTimeLogs.Add(newLog);
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Report updated successfully.";
            return RedirectToPage("./Details", new { id = existingSubmission.Id });
        }
    }
}
