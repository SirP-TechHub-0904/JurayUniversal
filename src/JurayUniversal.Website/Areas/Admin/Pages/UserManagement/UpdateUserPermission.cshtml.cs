
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class UpdateUserPermissionModel : PageModel
    {
       

        private readonly SignInManager<Profile> _signInManager;
        private readonly RoleManager<IdentityRole> _role;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<UpdateUserPermissionModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;


        public UpdateUserPermissionModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            RoleManager<IdentityRole> role,
        ILogger<UpdateUserPermissionModel> logger,
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _role = role;
            _context = context;
        }


        public IList<string> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
        public IList<string> RemainingUserRoles { get; set; }
        public Profile UserInfo { get; set; }

        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string Fullname { get; set; }

        public async Task<IActionResult> OnGetAsync(string uid, string fullname)
        {
            if (uid == null)
            {
                return NotFound();
            }
            Fullname = fullname;
            Roles = await _role.Roles.Where(x => x.Name != "mSuperAdmin" && x.Name != "CFO" && x.Name != "CEO" && x.Name != "Fund").Select(x => x.Name).ToListAsync();
            //Roles = await _role.Roles.Where(x => x.Name != "mSuperAdmin").Select(x => x.Name).ToListAsync();

            UserInfo = await _userManager.FindByIdAsync(uid);
            UserRoles = await _userManager.GetRolesAsync(UserInfo);
            var profile = await _userManager.FindByIdAsync(uid);
            var roleslist = string.Join(", ", UserRoles);
            //profile. = roleslist;
            await _userManager.UpdateAsync(profile);

            var RemainingRoles = Roles.Except(UserRoles);
            RemainingUserRoles = RemainingRoles.ToList();

            if (UserInfo == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string fullname)
        {
            Fullname = fullname;
            var role = await _role.FindByIdAsync(id);
            var user = await _userManager.FindByIdAsync(UserId);
            var checkuserroles = await _userManager.IsInRoleAsync(user, role.Name);
            if (checkuserroles == true)
            {
                try
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                catch (Exception d) { }
            }
            else
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                catch (Exception d) { }
            }
            return RedirectToPage("./UpdateUserPermission", new { uid = user.Id, fullname = Fullname });
        }

    }
}
