using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Users
{
    [Authorize(Roles = "MasterSuperAdmin,CentralAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public class UserViewModel
        {
            public string Id { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public IList<string> Roles { get; set; } = new List<string>();
            public bool IsLockedOut { get; set; }
            public bool EmailConfirmed { get; set; }
        }

        public IList<UserViewModel> CentralUsers { get; set; } = new List<UserViewModel>();

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var list = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var isLockedOut = await _userManager.IsLockedOutAsync(user);

                list.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Roles = roles,
                    IsLockedOut = isLockedOut,
                    EmailConfirmed = user.EmailConfirmed
                });
            }

            CentralUsers = list;
        }
    }
}
