using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Platform.Auth
{
    public class LoginModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly PasswordHasher<MisApplicantAccount> _passwordHasher;

        public LoginModel(DashboardDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<MisApplicantAccount>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync("ApplicantAuth");

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/MisRecruitment/Platform/JobBoard");

            if (ModelState.IsValid)
            {
                var account = await _context.MisApplicantAccounts.FirstOrDefaultAsync(u => u.Email == Input.Email);
                if (account != null)
                {
                    var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, Input.Password);
                    if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                            new Claim(ClaimTypes.Name, account.Email),
                            new Claim(ClaimTypes.GivenName, account.FirstName),
                            new Claim("ApplicantRole", "Applicant")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "ApplicantAuth");

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(
                            "ApplicantAuth",
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return LocalRedirect(returnUrl);
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            return Page();
        }
    }
}
