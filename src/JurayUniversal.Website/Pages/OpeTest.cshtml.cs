using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace JurayUniversal.Website.Pages
{
    [AllowAnonymous]
    public class OpeTestModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;

        private readonly IEmailSender _sender;
        public OpeTestModel(IEmailSender sender, UserManager<Profile> userManager)
        {
            _sender = sender;
            _userManager = userManager;
        }
        public bool Result = false;
        public async Task<IActionResult> OnGetAsync()
        {
           
             await _sender.SendEmailAsync("onwukaemeka41@gmail.com", "Test Mail Message", "Email Test");
            return Page();
        }
    }
}
