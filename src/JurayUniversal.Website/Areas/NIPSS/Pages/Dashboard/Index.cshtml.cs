using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Dashboard
{

    [Microsoft.AspNetCore.Authorization.Authorize]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<ParticipantList> ParticipantList { get; set; }
        public ParticipantList Participant { get; set; }

        public int Male { get; set; }
        public int Female { get; set; }
        public int Total { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User); 
            Participant = await _context.ParticipantLists
                .Include(x=>x.User)
                .Include(x => x.Course).ThenInclude(x => x.CourseCategory) 
                .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);
            if (Participant == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/ParticipantError");
            }
            var list = _context.ParticipantLists.Include(x=>x.User).Where(x=>x.CourseId == Participant.CourseId && x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active).AsEnumerable();
              
            Male = list.Where(x=>x.User.Gender == Domain.Enum.Enum.GenderStatus.Male).Count();
            Female = list.Where(x=>x.User.Gender == Domain.Enum.Enum.GenderStatus.Female).Count();
            Total = list.Count();
            
          
            return Page();
        }

    }
}
