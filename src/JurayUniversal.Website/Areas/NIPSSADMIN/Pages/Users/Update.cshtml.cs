using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL.Migrations;
namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class UpdateModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<UpdateModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UpdateModel(SignInManager<Profile> signInManager,
            ILogger<UpdateModel> logger,
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public Profile UserDatas { get; set; }

        [BindProperty]
        public ParticipantList ParticipantList { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);

            if (UserDatas == null)
            {
                return NotFound();
            }

            //

            ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory).Include(x => x.User).FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == id);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var userupdate = await _userManager.FindByIdAsync(UserDatas.Id);
            //UserDatas = userupdate;
            try
            {


                userupdate.FirstName = UserDatas.FirstName;
                userupdate.LastName = UserDatas.LastName;
                userupdate.MiddleName = UserDatas.MiddleName;
                userupdate.PhoneNumber = UserDatas.PhoneNumber;
                userupdate.PositionOrRank = UserDatas.PositionOrRank;
                var xParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory).Include(x => x.User).FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == userupdate.Id);
                if (xParticipantList != null)
                {
                    xParticipantList.ParticipantStatus = ParticipantList.ParticipantStatus;
                    if (xParticipantList.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Deleted)
                    {
                        xParticipantList.StudyGroupId = null;
                        var accomodation = await _context.Accomodations.FirstOrDefaultAsync(x => x.ParticipantListId == xParticipantList.Id);
                        if (accomodation != null)
                        {
                            accomodation.ParticipantListId = null;
                            _context.Attach(accomodation).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        var studygroup = await _context.StudyGroups.FirstOrDefaultAsync(x => x.UserId == xParticipantList.UserId);
                        if (studygroup != null)
                        {
                            _context.StudyGroups.Remove(studygroup);
                            await _context.SaveChangesAsync();
                        }

                    }
                    _context.Attach(xParticipantList).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    userupdate.UserStatus = Domain.Enum.Enum.UserStatus.Deleted;
                }
                else
                {
                    userupdate.UserStatus = UserDatas.UserStatus;
                }


                await _userManager.UpdateAsync(userupdate);




                TempData["success"] = "updated successful";
                return RedirectToPage("./info", new { id = userupdate.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["error"] = "Unable to update user";
                return Page();

            }


        }

    }
}

