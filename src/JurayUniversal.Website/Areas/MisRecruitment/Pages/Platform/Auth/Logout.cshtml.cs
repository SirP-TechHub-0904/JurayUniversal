using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Platform.Auth
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("ApplicantAuth");
            return LocalRedirect("~/MisRecruitment/Platform/JobBoard");
        }
    }
}
