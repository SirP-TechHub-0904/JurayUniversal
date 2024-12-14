using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Account
{
    [Microsoft.AspNetCore.Authorization.Authorize]
     

    public class CourseModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CourseModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Course Course { get; set; }
        [BindProperty]
        public CourseSubTheme CourseSubTheme { get; set; }

        [BindProperty]
        public StudyGroupCategory StudyGroupCategory { get; set; }
        public List<StudyGroupCategory> StudyGroupCategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        { 
            var user = await _userManager.GetUserAsync(User);

            var participant = await _context.ParticipantLists
                .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);


            if (participant == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/Index");
            }
            
            Course = await _context.Courses.Include(x => x.CourseCategory)
                .Include(x => x.SubThemes)
                .Include(x => x.StudyGroupCategories)
                .FirstOrDefaultAsync(m => m.Id == participant.CourseId);

            if (Course == null)
            {
                return RedirectToPage("/Dashboard/Index");
            }
            return Page();
        }

 
    }
} 