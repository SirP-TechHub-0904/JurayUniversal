using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisRecruitment.Pages.Platform.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly PasswordHasher<MisApplicantAccount> _passwordHasher;

        public RegisterModel(DashboardDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<MisApplicantAccount>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, StringLength(100)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required, StringLength(100)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Phone]
            public string? Phone { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var existingUser = await _context.MisApplicantAccounts.FirstOrDefaultAsync(u => u.Email == Input.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "An account with this email already exists.");
                    return Page();
                }

                var account = new MisApplicantAccount
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Email = Input.Email,
                    Phone = Input.Phone
                };

                account.PasswordHash = _passwordHasher.HashPassword(account, Input.Password);

                _context.MisApplicantAccounts.Add(account);
                await _context.SaveChangesAsync();

                // Automatically log them in after registration
                return RedirectToPage("./Login", new { returnUrl = ReturnUrl });
            }

            return Page();
        }
    }
}
