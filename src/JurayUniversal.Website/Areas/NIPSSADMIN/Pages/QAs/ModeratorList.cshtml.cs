using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.QAs
{

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Directing,Management")]

    public class ModeratorListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ModeratorListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Moderator> Moderators { get; set; }
        public Course Course { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
        {
           
            if (id == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/Index");
            }
            Course = await _context.Courses
                .Include(x=>x.CourseCategory)
                .FirstOrDefaultAsync(x=>x.Id == id);
            if(Course == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/Index");
            }
                
                Moderators = await _context.Moderators.Include(x => x.TimeTable)
                .Include(x => x.User)
                .Include(x => x.QuestionAndAnswers)
                .Where(m => m.TimeTable.CourseId == id).ToListAsync();


            return Page();
        }


    }

}
