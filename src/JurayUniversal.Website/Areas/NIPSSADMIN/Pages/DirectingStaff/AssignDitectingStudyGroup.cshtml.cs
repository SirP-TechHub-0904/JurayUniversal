using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.DirectingStaff
{
       [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class AssignDitectingStudyGroupModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public AssignDitectingStudyGroupModel(
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
        public long StudyGroupId { get; set; }

        [BindProperty]
        public StudyGroup StudyGroup { get; set; }

        [BindProperty]
        public Profile UserData { get; set; } 
        public StaffManager StaffManager { get; set; }
        public async Task<IActionResult> OnGetAsync(string id, long studygroupId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserData = user;
            //
           

            StaffManager = await _context.StaffManagers
                .Include(x => x.User).Include(x => x.StudyGroup).ThenInclude(x => x.StudyGroupCategory)

                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            var getstudygroup = await _context.StudyGroups.Include(x=>x.StudyGroupCategory).FirstOrDefaultAsync(x=>x.Id == studygroupId);
            if(getstudygroup == null)
            {
                return NotFound($"Unable to load study group with ID '{_userManager.GetUserId(User)}'.");

            }
            else
            {
                StudyGroupId = getstudygroup.Id;
            }
             
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
            StaffManager = await _context.StaffManagers
              .Include(x => x.StudyGroup).ThenInclude(x => x.StudyGroupCategory) 
              .FirstOrDefaultAsync(x => x.UserId == user.Id);
            var getstudygroup = await _context.StudyGroupCategory.FindAsync(StudyGroup.StudyGroupCategoryId);
            if (getstudygroup != null)
            {
                //check if user is in a group
                var checkifuserisingroup = await _context.StudyGroups.Include(x => x.StudyGroupCategory).FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == StudyGroupId);
                if (checkifuserisingroup != null)
                {
                    checkifuserisingroup.UserId = null;
                    _context.Attach(checkifuserisingroup).State = EntityState.Modified;
                }
                StudyGroup sngroup = new StudyGroup();
                sngroup.Position = StudyGroup.Position;
                sngroup.UserId = user.Id;

                sngroup.StudyGroupCategoryId = StudyGroup.StudyGroupCategoryId;
                _context.StudyGroups.Add(sngroup);
                await _context.SaveChangesAsync();
                StaffManager.StudyGroupId = sngroup.Id;
                _context.Attach(StaffManager).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["success"] = "Successful";
            }


            ViewData["StudyGroupCategoryId"] = new SelectList(_context.StudyGroupCategory, "Id", "Title");
            return RedirectToPage("./UpdateStudyGroup", new { id = user.Id });
        }


    }
}
