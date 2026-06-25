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
    public class EditModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditModel(DashboardDbContext context)
        {
            _context = context;
        }

        public string GroupAlias { get; set; } = "Group";

        [BindProperty]
        public MisGroup MisGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MisGroup = await _context.MisGroups.FirstOrDefaultAsync(m => m.Id == id);

            if (MisGroup == null)
            {
                return NotFound();
            }

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

            var groupToUpdate = await _context.MisGroups.FirstOrDefaultAsync(g => g.Id == MisGroup.Id);
            if (groupToUpdate == null) return NotFound();

            groupToUpdate.Title = MisGroup.Title;
            groupToUpdate.Description = MisGroup.Description;
            groupToUpdate.IsActive = MisGroup.IsActive;

            await _context.SaveChangesAsync();
            TempData["success"] = "Group updated successfully.";

            return RedirectToPage("./List");
        }
    }
}
