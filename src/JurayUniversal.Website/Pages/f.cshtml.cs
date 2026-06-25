using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Pages
{
    public class fModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public fModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisForm Form { get; set; }

        public async Task<IActionResult> OnGetAsync(string code)
        {
            if (string.IsNullOrEmpty(code)) return NotFound();

            Form = await _context.MisForms.FirstOrDefaultAsync(f => f.ShortCode == code);

            if (Form == null) return NotFound();
            if (!Form.IsPublished) return NotFound("This form is no longer available.");

            if (Form.RequireLogin && !User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = $"/f/{code}" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string code)
        {
            if (string.IsNullOrEmpty(code)) return NotFound();
            var form = await _context.MisForms.FirstOrDefaultAsync(f => f.ShortCode == code);
            
            if (form == null || !form.IsPublished) return NotFound();

            if (form.RequireLogin && !User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = $"/f/{code}" });
            }

            // Capture raw JSON payload
            using var reader = new StreamReader(Request.Body);
            var responsesJson = await reader.ReadToEndAsync();

            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = _userManager.GetUserId(User);
            }

            var response = new MisFormResponse
            {
                MisFormId = form.Id,
                UserProfileId = userId,
                ResponsesJson = responsesJson,
                SubmittedAt = DateTime.UtcNow
            };

            _context.MisFormResponses.Add(response);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true, redirectUrl = "/fSuccess" });
        }
    }
}
