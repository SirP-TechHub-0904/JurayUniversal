using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Dashboard
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(200)]
            public string Title { get; set; }

            [StringLength(1000)]
            public string Description { get; set; }

            [Display(Name = "Require users to be logged in?")]
            public bool RequireLogin { get; set; }

            [Display(Name = "Allow users to submit multiple times?")]
            public bool AllowMultipleSubmissions { get; set; } = true;

            [Display(Name = "Allow users to view their responses?")]
            public bool AllowUserToViewResponses { get; set; } = false;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ModelState.Remove("MisForm.Responses");
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            var form = new MisForm
            {
                Title = Input.Title,
                Description = Input.Description,
                RequireLogin = Input.RequireLogin,
                AllowMultipleSubmissions = Input.AllowMultipleSubmissions,
                AllowUserToViewResponses = Input.AllowUserToViewResponses,
                ShortCode = GenerateShortCode(),
                SchemaJson = "[]", // Empty builder schema initially
                IsPublished = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.MisForms.Add(form);
            await _context.SaveChangesAsync();

            TempData["success"] = "Form created successfully! Now use the Builder to add your questions.";
            return RedirectToPage("./Builder", new { id = form.Id });
        }

        private string GenerateShortCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string code;
            do
            {
                code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            } while (_context.MisForms.Any(f => f.ShortCode == code));
            
            return code;
        }
    }
}
