using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFleet.Pages.Dashboard.Equipment
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
        public MisEquipment Asset { get; set; }

        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await _context.MisEquipmentCategories.ToListAsync();
            if (!categories.Any())
            {
                TempData["error"] = "You must create at least one Equipment Category first.";
                return RedirectToPage("../Categories/Index");
            }
            CategoryList = new SelectList(categories, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CategoryList = new SelectList(await _context.MisEquipmentCategories.ToListAsync(), "Id", "Name");
                return Page();
            }

            Asset.CurrentStatus = Domain.Enum.Enum.MisEquipmentStatus.Available;
            _context.MisEquipments.Add(Asset);
            await _context.SaveChangesAsync();

            TempData["success"] = "Asset registered successfully!";
            return RedirectToPage("./Details", new { id = Asset.Id });
        }
    }
}
