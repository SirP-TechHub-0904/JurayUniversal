using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.IPortfolio.Pages.Account
{
    public class DashboardModel : PageModel
    {
        [BindProperty]
    public string Name { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Process the submitted data (e.g., save to a database)
        // ...

        return RedirectToPage("Confirmation", new { name = Name });
    }

    }
}
