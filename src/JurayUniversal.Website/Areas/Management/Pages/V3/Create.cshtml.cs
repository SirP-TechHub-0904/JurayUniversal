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
        private readonly RoleManager<AppRole> _role;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<CreateModel> _logger;
        private readonly DashboardDbContext _context;


        public CreateModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            RoleManager<AppRole> role,
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
                var result = await _userManager.CreateAsync(user, "[REDACTED_BOOTSTRAP_PASSWORD]");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    // Roles are now seeded centrally by RoleSeeder.cs
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
