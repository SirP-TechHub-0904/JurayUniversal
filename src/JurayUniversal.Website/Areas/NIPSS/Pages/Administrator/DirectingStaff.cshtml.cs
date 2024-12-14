using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Administrator
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class DirectingStaffModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Dashboard/ComingSoon", new { area = "NIPSS" });
        }
    }
}
