using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class UpdateAccomodationModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UpdateAccomodationModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public long? StudyGroupIdNew { get; set; }
        [BindProperty]
        public long AccomodationId { get; set; }

        [BindProperty]
        public StudyGroup StudyGroup { get; set; }

        [BindProperty]
        public Profile UserData { get; set; }
        public ParticipantList ParticipantList { get; set; } 
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserData = user;
            //
            ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                .Include(x=>x.Accomodation)
                .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

 
            ViewData["AccomodationId"] = new SelectList(_context.Accomodations.Where(x=>x.ParticipantListId == null), "Id", "Name");
             return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(UserData.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserData = user;
            ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory)

              .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

            //ParticipantList.AccomodationId = AccomodationId;
            _context.Attach(ParticipantList).State = EntityState.Modified;

            var existingAccomodation = await _context.Accomodations.FirstOrDefaultAsync(x => x.ParticipantListId == ParticipantList.Id);
            if(existingAccomodation != null)
            {
                existingAccomodation.ParticipantListId = null;
                _context.Attach(existingAccomodation).State = EntityState.Modified;
            }

            var getUserAccomodation = await _context.Accomodations.FirstOrDefaultAsync(x => x.Id == AccomodationId);
            getUserAccomodation.ParticipantListId = ParticipantList.Id;
            _context.Attach(getUserAccomodation).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            TempData["success"] = "Successful";
        
            return RedirectToPage("./UpdateAccomodation", new { id = user.Id
    });
        }


    }
}
