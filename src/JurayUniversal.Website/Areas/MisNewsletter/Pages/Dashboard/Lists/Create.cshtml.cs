using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.Lists
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
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
        public MisNewsletterList NewsletterList { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewsletterList.UserId = _userManager.GetUserId(User);
            _context.MisNewsletterLists.Add(NewsletterList);
            await _context.SaveChangesAsync();

            TempData["success"] = "Mailing list created successfully.";
            return RedirectToPage("./Details", new { id = NewsletterList.Id });
        }
    }
}

