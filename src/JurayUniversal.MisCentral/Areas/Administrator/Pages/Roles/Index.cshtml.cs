using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Roles
{
    [Authorize(Roles = "MasterSuperAdmin,CentralAdmin")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public class RoleViewModel
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public int UserCount { get; set; }
            public bool IsSystemRole { get; set; }
        }

        public IList<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();

        [BindProperty]
        public CreateRoleInputModel Input { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class CreateRoleInputModel
        {
            [Required]
            [StringLength(50, MinimumLength = 3)]
            [Display(Name = "Role Name")]
            public string RoleName { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var systemRoles = new[] { "MasterSuperAdmin", "CentralAdmin", "CentralSupport", "CentralAuditor" };
            var allRoles = await _roleManager.Roles.ToListAsync();
            var list = new List<RoleViewModel>();

            foreach (var r in allRoles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(r.Name!);
                list.Add(new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name ?? string.Empty,
                    UserCount = usersInRole.Count,
                    IsSystemRole = systemRoles.Contains(r.Name)
                });
            }

            Roles = list;
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(Input.RoleName);
                if (roleExists)
                {
                    ErrorMessage = $"Role '{Input.RoleName}' already exists in Central Identity.";
                    return RedirectToPage();
                }

                var result = await _roleManager.CreateAsync(new IdentityRole(Input.RoleName.Trim()));
                if (result.Succeeded)
                {
                    StatusMessage = $"Central role '{Input.RoleName}' created successfully.";
                    return RedirectToPage();
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }

            await OnGetAsync();
            return Page();
        }
    }
}
