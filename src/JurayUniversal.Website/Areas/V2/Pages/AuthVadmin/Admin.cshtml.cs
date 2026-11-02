using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.V2.Pages.AuthVadmin
{

    [AllowAnonymous]
    public class AdminModel : PageModel
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly RoleManager<AppRole> _role;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<ReadonlyModel> _logger;
        private readonly DashboardDbContext _context;


        public AdminModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            RoleManager<AppRole> role,
        ILogger<ReadonlyModel> logger,
            DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _role = role;
            _context = context;
        }


        public string ReturnUrl { get; set; }
        public Profile Profile { get; set; }
        public Profile Profile2 { get; set; }
        public Profile Profile3 { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Profile = await _userManager.FindByEmailAsync("admin@platform.xyz");
            Profile2 = await _userManager.FindByEmailAsync("user@platform.xyz");
            Profile3 = await _userManager.FindByEmailAsync("majoradmin@platform.xyz");
 
            return Page();
        }
        public async Task<IActionResult> OnPostReconcileRoles(string returnUrl = null, string refxid = null)
        {
            AppRole Role = new AppRole("mSuperAdmin");
            var checkrole = await _role.FindByNameAsync("mSuperAdmin");
            if (checkrole == null)
            {
                await _role.CreateAsync(Role);

            }
            //
            AppRole Manager = new AppRole("Manager");
            var checkManager = await _role.FindByNameAsync("Manager");
            if (checkManager == null)
            {
                await _role.CreateAsync(Manager);

            }
            //
            AppRole Managerf = new AppRole("Finance");
            var checkManagerf = await _role.FindByNameAsync("Finance");
            if (checkManagerf == null)
            {
                await _role.CreateAsync(Managerf);

            }
            //
            AppRole Security = new AppRole("Security");
            var checkSecurity = await _role.FindByNameAsync("Security");
            if (checkSecurity == null)
            {
                await _role.CreateAsync(Security);

            }
            //
            AppRole JAdmin = new AppRole("SubAdmin");
            var checkJAdmin = await _role.FindByNameAsync("SubAdmin");
            if (checkJAdmin == null)
            {
                await _role.CreateAsync(JAdmin);

            }

            AppRole User = new AppRole("User");
            var checkUser = await _role.FindByNameAsync("User");
            if (checkUser == null)
            {
                await _role.CreateAsync(User);

            }

            AppRole xRole = new AppRole("Editor");
            var xcheckrole = await _role.FindByNameAsync("Editor");

            if (xcheckrole == null)
            {
                await _role.CreateAsync(xRole);

            }

            AppRole AdminRole = new AppRole("Admin");
            var checkAdminRole = await _role.FindByNameAsync("Admin");
            if (checkAdminRole == null)
            {
                await _role.CreateAsync(AdminRole);

            }
            return RedirectToPage("./Admin");
        }
        // public string REFID { get; set; }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null, string refxid = null)
        {

            //if (ModelState.IsValid)
            //{
            var user = new Profile
            {
                UserName = "admin@platform.xyz",
                Email = "admin@platform.xyz",
                PhoneNumber = "000",

                EmailConfirmed = true,
                LockoutEnabled = false,
                UserStatus = Domain.Enum.Enum.UserStatus.Active


            };



            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, (System.Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? ""));
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");


                await _userManager.AddToRoleAsync(user, "Admin");
                await _userManager.AddToRoleAsync(user, "Editor");
                await _userManager.AddToRoleAsync(user, "Manager");
                await _userManager.AddToRoleAsync(user, "SubAdmin");
                await _userManager.AddToRoleAsync(user, "Security");
                //
                AppRole Managerf = new AppRole("Finance");
                var checkManagerf = await _role.FindByNameAsync("Finance");
                if (checkManagerf == null)
                {
                    await _role.CreateAsync(Managerf);

                }
                await _userManager.AddToRoleAsync(user, "Finance");
                //return LocalRedirect(returnUrl);

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //}

            return RedirectToPage("./Admin");
        }
        public async Task<IActionResult> OnPostNewAcc(string returnUrl = null, string refxid = null)
        {

            //if (ModelState.IsValid)
            //{
            var user = new Profile
            {
                UserName = "user@platform.xyz",
                Email = "user@platform.xyz",
                PhoneNumber = "000",

                EmailConfirmed = true,
                LockoutEnabled = false,
                UserStatus = Domain.Enum.Enum.UserStatus.Active


            };



            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, (System.Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? ""));
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");


                await _userManager.AddToRoleAsync(user, "User"); 
                 

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //}

            return RedirectToPage("./Admin");
        }
        public async Task<IActionResult> OnPostSupNewAcc(string returnUrl = null, string refxid = null)
        {

            //if (ModelState.IsValid)
            //{
            var user = new Profile
            {
                UserName = "majoradmin@platform.xyz",
                Email = "majoradmin@platform.xyz",
                PhoneNumber = "000",

                EmailConfirmed = true,
                LockoutEnabled = false,
                UserStatus = Domain.Enum.Enum.UserStatus.Active


            };



            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, (System.Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? ""));
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");


                await _userManager.AddToRoleAsync(user, "mSuperAdmin");


            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //}

            return RedirectToPage("./Admin");
        }
    }
}
