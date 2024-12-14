using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.Management.Pages.V3
{
    public class CreateModel : PageModel
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly RoleManager<IdentityRole> _role;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<CreateModel> _logger;
        private readonly DashboardDbContext _context;


        public CreateModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            RoleManager<IdentityRole> role,
        ILogger<CreateModel> logger,
            DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _role = role;
            _context = context;
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnGetAsync()
        {
            var se = await _context.SuperSettings.FirstOrDefaultAsync();
            if (se == null)
            {
                SuperSetting SuperSetting = new SuperSetting();
                _context.SuperSettings.Add(SuperSetting);
                Setting s = new Setting();

                _context.Settings.Add(s);
                DataConfig fig = new DataConfig();
                _context.DataConfigs.Add(fig);

                DashboardSetting dsetting = new DashboardSetting();
                _context.DashboardSettings.Add(dsetting);

                FormSetting fsetting = new FormSetting();
                _context.FormSettings.Add(fsetting);

                await _context.SaveChangesAsync();

                var user = new Profile
                {
                    UserName = "universal@juray.io",
                    Email = "universal@juray.io",
                    PhoneNumber = "000",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    UserStatus = Domain.Enum.Enum.UserStatus.Active

                };



                user.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, "UnixMain@2023");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    IdentityRole Role = new IdentityRole("mSuperAdmin");
                    var checkrole = await _role.FindByNameAsync("mSuperAdmin");
                    if (checkrole == null)
                    {
                        await _role.CreateAsync(Role);

                    }
                    //
                    IdentityRole Manager = new IdentityRole("Manager");
                    var checkManager = await _role.FindByNameAsync("Manager");
                    if (checkManager == null)
                    {
                        await _role.CreateAsync(Manager);

                    }
                    //
                    IdentityRole Managerf = new IdentityRole("Finance");
                    var checkManagerf = await _role.FindByNameAsync("Finance");
                    if (checkManagerf == null)
                    {
                        await _role.CreateAsync(Managerf);

                    }
                    //
                    IdentityRole Security = new IdentityRole("Security");
                    var checkSecurity = await _role.FindByNameAsync("Security");
                    if (checkSecurity == null)
                    {
                        await _role.CreateAsync(Security);

                    }
                    //
                    IdentityRole JAdmin = new IdentityRole("SubAdmin");
                    var checkJAdmin = await _role.FindByNameAsync("SubAdmin");
                    if (checkJAdmin == null)
                    {
                        await _role.CreateAsync(JAdmin);

                    }
                     
                    IdentityRole xRole = new IdentityRole("Editor");
                    var xcheckrole = await _role.FindByNameAsync("Editor");

                    if (xcheckrole == null)
                    {
                        await _role.CreateAsync(xRole);

                    }
                    IdentityRole xstaffUser = new IdentityRole("Staff");
                    var staffUser = await _role.FindByNameAsync("Staff");
                    if (staffUser == null)
                    {
                        await _role.CreateAsync(xstaffUser);

                    }
                    IdentityRole AdminRole = new IdentityRole("Admin");
                    var checkAdminRole = await _role.FindByNameAsync("Admin");
                    if (checkAdminRole == null)
                    {
                        await _role.CreateAsync(AdminRole);

                    }
                    IdentityRole User = new IdentityRole("User");
                    var checkUser = await _role.FindByNameAsync("User");
                    if (checkUser == null)
                    {
                        await _role.CreateAsync(User);

                    }
                    await _userManager.AddToRoleAsync(user, "mSuperAdmin");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
