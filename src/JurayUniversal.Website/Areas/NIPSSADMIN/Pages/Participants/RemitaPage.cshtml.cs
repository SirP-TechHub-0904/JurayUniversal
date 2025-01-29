using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Participants
{
     [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class RemitaPageModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public RemitaPageModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<ParticipantList> ParticipantList { get; set; }
        public Course Course { get; set; }

        public int PendingApproval { get;set;}
        public int Approved { get;set; }
        public int PendingUploaded { get;set; }
        public async Task OnGetAsync(long id)
        {
            ParticipantList = await _context.ParticipantLists
                .Include(x => x.Accomodation)
               
                .Include(x => x.User).Where(x => x.CourseId == id && x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active).OrderBy(x => x.IdNumber)
                 
                .ToListAsync();
             

            Course = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == id);

            PendingApproval = ParticipantList.Where(x=>x.RemitaApproved == false).Count();
            Approved = ParticipantList.Where(x=>x.RemitaApproved == true).Count();
            PendingUploaded = ParticipantList.Where(x=>String.IsNullOrEmpty(x.RemitaKey)).Count();
        }

        public async Task<string> GetAccomodation(int id)
        {
            var acc = await _context.Accomodations.FirstOrDefaultAsync(x => x.ParticipantListId == id);
            if (acc != null)
            {
                return acc.Name;
            }
            return "";
        }

    }

}
