using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class ComingSoonModel : PageModel
    {
        public TimeSpan Countdown { get; private set; }

        public void OnGet(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            Countdown = date > currentDate ? date - currentDate : TimeSpan.Zero;
        }
    }
}
