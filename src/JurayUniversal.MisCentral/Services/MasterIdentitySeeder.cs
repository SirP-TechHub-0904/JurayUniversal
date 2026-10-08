using JurayUniversal.MisCentral.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.MisCentral.Services
{
    public static class MasterIdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Seed Roles
            foreach (var roleName in CentralRoles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Default Super Admin if no users exist
            const string defaultEmail = "superadmin@jurayuniversal.com";
            const string defaultPassword = "SuperAdmin@Juray2026!";

            var existingUser = await userManager.FindByEmailAsync(defaultEmail);
            if (existingUser == null)
            {
                var superAdmin = new IdentityUser
                {
                    UserName = defaultEmail,
                    Email = defaultEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(superAdmin, defaultPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, CentralRoles.MasterSuperAdmin);
                }
            }
            else
            {
                // Ensure existing user has MasterSuperAdmin role
                if (!await userManager.IsInRoleAsync(existingUser, CentralRoles.MasterSuperAdmin))
                {
                    await userManager.AddToRoleAsync(existingUser, CentralRoles.MasterSuperAdmin);
                }
            }
        }
    }
}
