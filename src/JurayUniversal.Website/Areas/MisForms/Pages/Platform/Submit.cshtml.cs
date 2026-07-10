using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Platform
{
    public class SubmitModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public SubmitModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisForm Form { get; set; }

        [BindProperty]
        public string ResponsesJson { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Form = await _context.MisForms.FirstOrDefaultAsync(x => x.Id == id && x.IsPublished);
            if (Form == null) return NotFound();

            if (Form.RequireLogin && !User.Identity.IsAuthenticated)
            {
                return Redirect($"/Identity/Account/Login?ReturnUrl=/MisForms/Platform/Submit/{id}");
            }

            if (User.Identity.IsAuthenticated && !Form.AllowMultipleSubmissions)
            {
                var userId = _userManager.GetUserId(User);
                bool hasSubmitted = await _context.MisFormResponses.AnyAsync(r => r.MisFormId == Form.Id && r.UserProfileId == userId);
                if (hasSubmitted)
                {
                    TempData["ErrorMessage"] = "You have already submitted a response for this form. Multiple submissions are not allowed.";
                    return RedirectToPage("./Index");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            var form = await _context.MisForms.FirstOrDefaultAsync(x => x.Id == id && x.IsPublished);
            if (form == null) return NotFound();

            if (form.RequireLogin && !User.Identity.IsAuthenticated)
            {
                return Redirect($"/Identity/Account/Login?ReturnUrl=/MisForms/Platform/Submit/{id}");
            }

            if (User.Identity.IsAuthenticated && !form.AllowMultipleSubmissions)
            {
                var currentUserId = _userManager.GetUserId(User);
                bool hasSubmitted = await _context.MisFormResponses.AnyAsync(r => r.MisFormId == form.Id && r.UserProfileId == currentUserId);
                if (hasSubmitted)
                {
                    return BadRequest("You have already submitted this form.");
                }
            }

            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                userId = user?.Id;
            }

            var response = new MisFormResponse
            {
                MisFormId = id,
                UserProfileId = userId,
                ResponsesJson = ResponsesJson
            };

            _context.MisFormResponses.Add(response);
            await _context.SaveChangesAsync();

            TempData["success"] = "Your response has been submitted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
