using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard
{
    [Authorize(Roles = "mSuperAdmin,Admin,Administrator")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
