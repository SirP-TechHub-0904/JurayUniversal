using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class CreateModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly DashboardDbContext _context;

        public CreateModel(UserManager<Profile> userManager, DashboardDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<UserCategory> Categories { get; set; } = new();

        public class InputModel
        {
            public string? Title { get; set; }

            [Required(ErrorMessage = "First Name is required.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Last Name is required.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty;

            [Display(Name = "Middle Name")]
            public string? MiddleName { get; set; }

            [Required(ErrorMessage = "Email Address is required.")]
            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Gender")]
            public GenderStatus Gender { get; set; } = GenderStatus.Male;

            [Display(Name = "Department / Category")]
            public long? UserCategoryId { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "Welcome@123";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.UserCategories.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _context.UserCategories.ToListAsync();
                return Page();
            }

            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Input.Email", "A user with this email address already exists.");
                Categories = await _context.UserCategories.ToListAsync();
                return Page();
            }

            var user = new Profile
            {
                UserName = Input.Email,
                Email = Input.Email,
                EmailConfirmed = true,
                Title = Input.Title,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                MiddleName = Input.MiddleName,
                PhoneNumber = Input.PhoneNumber,
                Gender = Input.Gender,
                UserCategoryId = Input.UserCategoryId,
                UserStatus = UserStatus.Active,
                Date = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                TempData["success"] = $"User {user.GetFullName()} created successfully!";
                return RedirectToPage("/Dashboard/List");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            Categories = await _context.UserCategories.ToListAsync();
            return Page();
        }
    }
}
