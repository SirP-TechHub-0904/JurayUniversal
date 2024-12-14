using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class ReportDetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ReportDetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Report Report { get; set; }
        [BindProperty]
        public Report NewReport { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Report = await _context.Report
                .Include(r => r.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Report == null)
            {
                return NotFound();
            }
             return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var reportdata = await _context.Report.FirstOrDefaultAsync(x => x.Id == Report.Id);
            if(NewReport.Achievement != null)
            {
            reportdata.Achievement = reportdata.Achievement + "<hr style=\"border:2px solid white; margin:0;padding:0;\"> @updated on " + DateTime.UtcNow.AddHours(1) + "<br>"+ NewReport.Achievement;

            }
            if (NewReport.IssuesAndChallenges != null)
            {
            reportdata.IssuesAndChallenges = reportdata.IssuesAndChallenges + "<hr style=\"border:2px solid white; margin:0;padding:0;\">@updated on " + DateTime.UtcNow.AddHours(1) + "<br>"+ NewReport.IssuesAndChallenges;

            }
            if (NewReport.PlansForNextDay != null)
            {
            reportdata.PlansForNextDay = reportdata.PlansForNextDay + "<hr style=\"border:2px solid white; margin:0;padding:0;\">@updated on " + DateTime.UtcNow.AddHours(1) + "<br>"+ NewReport.PlansForNextDay;

            }
            reportdata.ReportStatus = Domain.Enum.Enum.ReportStatus.Submitted;
            reportdata.ReportPeriodStatus = Domain.Enum.Enum.ReportPeriodStatus.Ontime;
            _context.Attach(reportdata).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(Report.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ReportDetails", new {id = Report.Id});
        }

        private bool ReportExists(long id)
        {
            return _context.Report.Any(e => e.Id == id);
        }
    }
}
