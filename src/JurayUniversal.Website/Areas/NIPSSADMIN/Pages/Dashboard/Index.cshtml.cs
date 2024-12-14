using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Directing,Management")]

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
        public Course Course { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Total { get; set; }
        public int DirectingStaff { get; set; }
        public int ManagingStaff { get; set; }
        public int Staff { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            
            var list = _context.ParticipantLists.Include(x => x.User).Where(x=> x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active).AsEnumerable();

            Male = list.Where(x => x.User.Gender == Domain.Enum.Enum.GenderStatus.Male).Count();
            Female = list.Where(x => x.User.Gender == Domain.Enum.Enum.GenderStatus.Female).Count();
            Total = list.Count();

            var users = _context.Users.AsEnumerable();
            DirectingStaff = users.Where(x=>x.Role == "Directing").Count();
            ManagingStaff = users.Where(x=>x.Role == "Management").Count();
            Staff = users.Where(x=>x.Role == "Staff").Count();
            return Page();
        }
    }
}
