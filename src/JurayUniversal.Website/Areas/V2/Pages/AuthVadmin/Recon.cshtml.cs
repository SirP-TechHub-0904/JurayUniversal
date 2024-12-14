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
    public class ReconModel : PageModel
    {
        private readonly SignInManager<Profile> _signInManager;
        private readonly RoleManager<IdentityRole> _role;
        private readonly UserManager<Profile> _userManager;
        private readonly ILogger<ReadonlyModel> _logger;
        private readonly DashboardDbContext _context;


        public ReconModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            RoleManager<IdentityRole> role,
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
            var getby = await _userManager.FindByEmailAsync("universal@platform.io");
            var remv = await _userManager.RemoveFromRoleAsync(getby, "mSuperAdmin");
            await _userManager.AddToRoleAsync(getby, "SubAdmin");
            await _userManager.AddToRoleAsync(getby, "Editor");
            await _userManager.AddToRoleAsync(getby, "Manager");
            return RedirectToPage("/V3/Index", new { area = "management" });

        }

    }
}
