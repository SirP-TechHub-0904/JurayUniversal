using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Platform.Leaves
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CreateModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public MisLeaveRequest LeaveRequest { get; set; } = new MisLeaveRequest();

        public void OnGet()
        {
            LeaveRequest.StartDate = DateTime.UtcNow.AddHours(1).Date;
            LeaveRequest.EndDate = DateTime.UtcNow.AddHours(1).Date;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            LeaveRequest.UserId = user.Id;
            LeaveRequest.DateSubmitted = DateTime.UtcNow.AddHours(1);
            LeaveRequest.Status = Domain.Enum.Enum.LeaveRequestStatus.Pending;

            // Ensure EndDate is at least StartDate if it's null
            if (!LeaveRequest.EndDate.HasValue)
            {
                LeaveRequest.EndDate = LeaveRequest.StartDate;
            }

            _context.MisLeaveRequests.Add(LeaveRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your request has been submitted successfully.";
            return RedirectToPage("./Index");
        }
    }
}
