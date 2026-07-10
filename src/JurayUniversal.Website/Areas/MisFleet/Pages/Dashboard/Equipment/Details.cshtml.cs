using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFleet.Pages.Dashboard.Equipment
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisEquipment Asset { get; set; }
        
        [BindProperty]
        public string SelectedUserId { get; set; }

        [BindProperty]
        public MisMaintenanceLog NewLog { get; set; }

        public SelectList UsersList { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Asset = await _context.MisEquipments
                .Include(e => e.MisEquipmentCategory)
                .Include(e => e.MisMaintenanceLogs)
                .Include(e => e.AssignedToUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Asset == null)
            {
                return NotFound();
            }

            // For Check-Out modal
            var users = await _userManager.Users.ToListAsync();
            UsersList = new SelectList(users, "Id", "Email"); // Or FullName if accessible easily

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync(long id)
        {
            var asset = await _context.MisEquipments.FindAsync(id);
            if (asset == null) return NotFound();

            if (string.IsNullOrEmpty(SelectedUserId))
            {
                TempData["error"] = "You must select a user.";
                return RedirectToPage(new { id = id });
            }

            asset.AssignedToUserId = SelectedUserId;
            asset.CurrentStatus = Domain.Enum.Enum.MisEquipmentStatus.InUse;
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Asset checked out successfully.";
            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostCheckInAsync(long id)
        {
            var asset = await _context.MisEquipments.FindAsync(id);
            if (asset == null) return NotFound();

            asset.AssignedToUserId = null;
            asset.CurrentStatus = Domain.Enum.Enum.MisEquipmentStatus.Available;
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Asset checked in and is now Available.";
            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostMaintenanceAsync(long id)
        {
            var asset = await _context.MisEquipments.FindAsync(id);
            if (asset == null) return NotFound();

            NewLog.MisEquipmentId = id;
            if (NewLog.Date == default) NewLog.Date = DateTime.UtcNow;

            _context.MisMaintenanceLogs.Add(NewLog);
            await _context.SaveChangesAsync();

            TempData["success"] = "Maintenance log added successfully.";
            return RedirectToPage(new { id = id });
        }
    }
}
