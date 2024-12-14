using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]
  
    public class IndexModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }
        public IEnumerable<ProfileDto> UserDatasList { get; set; }

        public async Task OnGetAsync()
        {
            UserDatasList = new List<ProfileDto>();
            var UserDatas = _userManager.Users
                .Where(x=>x.Email != "universal@j.io" && x.Email != "dashboard@platform.io" && x.Email != "admin@platform.io")
                
                .AsEnumerable();
            UserDatasList = UserDatas.Select(x=> new ProfileDto
            {
                Fullname = x.FirstName + " " + x.MiddleName + " " + x.LastName,
                Phone = x.PhoneNumber,
                Email = x.Email,
                Status = x.UserStatus,
                Id = x.Id,
                Category = x.Role
            });
          
        }
    }

    public class RoleDTO
    {
        public string RoleName { get; set; }
        // You can add more properties related to roles if needed
    }
    public class ProfileDto
    {
        public string Id { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }
        public GenderStatus Gender { get; set; }
        public string Roles { get; set; }
        public string Category { get; set; }
    }
}
