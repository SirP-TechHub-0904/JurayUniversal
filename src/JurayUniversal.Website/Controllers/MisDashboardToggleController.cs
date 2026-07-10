using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Controllers
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin,Admin,Administrator,Editor")]
    [Route("api/mis-toggle")]
    [IgnoreAntiforgeryToken] // Allow simple form POST from any layout without strict antiforgery if needed
    public class MisDashboardToggleController : Controller
    {
        private readonly DashboardDbContext _context;

        public MisDashboardToggleController(DashboardDbContext context)
        {
            _context = context;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleDashboard(string returnUrl)
        {
            var superSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            if (superSetting != null)
            {
                superSetting.UseNewMISDashboard = !superSetting.UseNewMISDashboard;
                await _context.SaveChangesAsync();
            }

            // Redirect back to login so it correctly evaluates where to send them (MIS vs Admin area) based on the new setting,
            // OR if returnUrl is provided and safe, we can redirect there. But wait, if they are currently on /Admin/Dashboard and they toggle to New MIS,
            // /Admin/Dashboard will STILL work, but RequiresNewMISDashboardAttribute might block /MIS/Dashboard if it's false.
            // Actually, if we just redirect to the root Dashboard index, it will route them correctly.
            
            return RedirectToPage("/Dashboard/Index", new { area = (superSetting != null && superSetting.UseNewMISDashboard) ? "MIS" : "Admin" });
        }
    }
}
