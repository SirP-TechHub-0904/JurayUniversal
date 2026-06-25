using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGroups.Pages.Dashboard
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        public string GroupAlias { get; set; } = "Group";

        [BindProperty]
        public MisGroup MisGroup { get; set; } = new MisGroup();

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
            {
                GroupAlias = settings.GroupAlias;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var settings = await _context.MisSettings.FirstOrDefaultAsync();
                if (settings != null && !string.IsNullOrEmpty(settings.GroupAlias))
                {
                    GroupAlias = settings.GroupAlias;
                }
                return Page();
            }

            MisGroup.CreatedAt = System.DateTime.UtcNow;
            _context.MisGroups.Add(MisGroup);
            await _context.SaveChangesAsync();

            TempData["success"] = "Group created successfully.";
            return RedirectToPage("./List");
        }
    }
}
