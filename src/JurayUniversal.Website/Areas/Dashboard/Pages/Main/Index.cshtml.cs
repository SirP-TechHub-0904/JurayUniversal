using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.Dashboard.Pages.Main
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,SubAdmin")]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
