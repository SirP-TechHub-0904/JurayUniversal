using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using JurayUniversal.Website.Filters;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Leaves
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class ReviewModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ReviewModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public MisLeaveRequest LeaveRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            LeaveRequest = await _context.MisLeaveRequests
                .Include(x => x.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (LeaveRequest == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminUser = await _userManager.GetUserAsync(User);
            if (adminUser == null)
            {
                return NotFound();
            }

            var requestToUpdate = await _context.MisLeaveRequests.FindAsync(LeaveRequest.Id);
            if (requestToUpdate == null)
            {
                return NotFound();
            }

            requestToUpdate.Status = LeaveRequest.Status;
            requestToUpdate.ReviewerRemarks = LeaveRequest.ReviewerRemarks;
            requestToUpdate.ReviewedById = adminUser.Id;
            requestToUpdate.DateReviewed = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request has been reviewed successfully.";
            return RedirectToPage("./Index");
        }
    }
}
