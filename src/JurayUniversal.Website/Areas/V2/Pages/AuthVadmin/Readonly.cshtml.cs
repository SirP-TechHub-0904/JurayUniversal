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
    public class ReadonlyModel : PageModel
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly RoleManager<AppRole> _role;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<ReadonlyModel> _logger;
        private readonly DashboardDbContext _context;


        public ReadonlyModel(
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

        public IList<AuthenticationScheme> ExternalLogins { get; set; }



        // public string REFID { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            //if (ModelState.IsValid)
            //{
            var user = new Profile
            {
                UserName = "universal@platform.io",
                Email = "universal@platform.io",
                PhoneNumber = "000",
               
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserStatus = Domain.Enum.Enum.UserStatus.Active

            };



            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, "[REDACTED_BOOTSTRAP_PASSWORD]");
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
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


                
                AppRole xRole = new AppRole("Editor");
                var xcheckrole = await _role.FindByNameAsync("Editor");

                if (xcheckrole == null)
                {
                    await _role.CreateAsync(xRole);

                }
                AppRole xstaffUser = new AppRole("Staff");
                 var staffUser = await _role.FindByNameAsync("Staff");
                if (staffUser == null)
                {
                    await _role.CreateAsync(xstaffUser);

                }
                AppRole AdminRole = new AppRole("Admin");
                 var checkAdminRole = await _role.FindByNameAsync("Admin");
                if (checkAdminRole == null)
                {
                    await _role.CreateAsync(AdminRole);

                }
                 AppRole User = new AppRole("User");
                 var checkUser = await _role.FindByNameAsync("User");
                if (checkUser == null)
                {
                    await _role.CreateAsync(User);

                }
                await _userManager.AddToRoleAsync(user, "SubAdmin");

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            

            return RedirectToPage("./SuperReadonly");

        }

    }
}
