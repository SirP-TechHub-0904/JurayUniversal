using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Library
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class IPublicationsModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Dashboard/ComingSoon", new { area = "NIPSS" });
        }
    }
}
