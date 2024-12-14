using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{ 
     [Microsoft.AspNetCore.Authorization.Authorize]
      
    public class IStudyGroupsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IStudyGroupsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public StudyGroup StudyGroup { get; set; }
       
        public List<StudyGroupCategory> StudyGroupCategory { get; set; }

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

            StudyGroupCategory = await _context.StudyGroupCategory.Include(x => x.StudyGroups)
                .ThenInclude(x => x.User) 
                .Where(m => m.CourseId == participant.CourseId).ToListAsync();

            
            return Page();
        }


    }
}
