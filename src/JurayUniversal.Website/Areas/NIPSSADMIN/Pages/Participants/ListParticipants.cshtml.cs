using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Application.Dtos;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Participants
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class ListParticipantsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ListParticipantsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<ParticipantList> ParticipantList { get; set; }
        public Course Course { get; set; }
        public async Task OnGetAsync(long id)
        {
            ParticipantList = await _context.ParticipantLists
                .Include(x=>x.Accomodation)
                .Include(x => x.StudyGroup).ThenInclude(x => x.StudyGroupCategory)
                .Include(x => x.User).Where(x => x.CourseId == id && x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active).OrderBy(x => x.IdNumber)
                 .GroupBy(x => x.UserId)
    .Select(g => g.First())
                .ToListAsync();

            //ParticipantList = await Task.WhenAll(ParticipantLists.Select(async d => new ParticipantDto
            //{
            //    Id = d.Id,
            //    UserId = d.UserId,
            //    StudyGroupCategory = d.StudyGroup?.StudyGroupCategory?.Title,
            //    Email = d.User.Email,
            //    PhoneNumber = d.User.PhoneNumber,
            //    FullnameX = d.User.FullnameX,
            //    LastLogin = d.User.LastLogin,
            //    UserStatus = d.User.UserStatus,
            //    Accomodation = await GetAccomodation(d.Id)
            //}));

            Course = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == id);
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
