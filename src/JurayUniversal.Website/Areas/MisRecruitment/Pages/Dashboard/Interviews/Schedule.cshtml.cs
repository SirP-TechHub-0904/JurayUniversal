using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Dashboard.Interviews
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ScheduleModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ScheduleModel(DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(long applicantId, DateTime interviewDate, JurayUniversal.Domain.Enum.Enum.InterviewType interviewType, string locationOrLink)
        {
            var applicant = await _context.MisJobApplicants.FindAsync(applicantId);
            if (applicant == null) return NotFound();

            var interview = new MisJobInterview
            {
                MisJobApplicantId = applicantId,
                InterviewDate = interviewDate,
                InterviewType = interviewType,
                LocationOrLink = locationOrLink,
                CreatedAt = DateTime.UtcNow
            };

            _context.MisJobInterviews.Add(interview);
            
            // Auto update status to Interview Scheduled
            if (interviewType == Domain.Enum.Enum.InterviewType.OnlineVideo)
                applicant.Status = Domain.Enum.Enum.ApplicationStatus.OnlineInterview;
            else if (interviewType == Domain.Enum.Enum.InterviewType.OfflinePhysical)
                applicant.Status = Domain.Enum.Enum.ApplicationStatus.OfflineInterview;
            else
                applicant.Status = Domain.Enum.Enum.ApplicationStatus.PhoneScreening;

            await _context.SaveChangesAsync();

            TempData["success"] = "Interview scheduled successfully!";
            return Redirect($"/MisRecruitment/Dashboard/Applicants/Details?id={applicantId}");
        }
    }
}
