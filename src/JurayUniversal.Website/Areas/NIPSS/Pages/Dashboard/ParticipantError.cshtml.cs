using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class ParticipantErrorModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Dashboard/ComingSoon", new { area = "NIPSS" });
        }
    }
}
