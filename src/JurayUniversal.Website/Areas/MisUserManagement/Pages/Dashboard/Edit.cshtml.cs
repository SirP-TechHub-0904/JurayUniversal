using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ProfileId { get; set; } = string.Empty;

        [BindProperty]
        public long? SelectedCategoryId { get; set; }

        [BindProperty]
        public UserStatus SelectedStatus { get; set; }

        [BindProperty]
        public GenderStatus SelectedGender { get; set; }

        public string FullName { get; set; } = string.Empty;
        public List<UserCategory> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var profile = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            ProfileId = profile.Id;
            SelectedCategoryId = profile.UserCategoryId;
            SelectedStatus = profile.UserStatus;
            SelectedGender = profile.Gender;
            FullName = profile.GetFullName();

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

            var profile = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.Id == ProfileId);
            if (profile == null)
            {
                return NotFound();
            }

            profile.UserCategoryId = SelectedCategoryId;
            profile.UserStatus = SelectedStatus;
            profile.Gender = SelectedGender;

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["success"] = "User category, status, and gender updated successfully!";
            return RedirectToPage("/Dashboard/List");
        }
    }
}
