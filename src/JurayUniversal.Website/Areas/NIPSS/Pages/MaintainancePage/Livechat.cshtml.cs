using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.MaintainancePage
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class LivechatModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
