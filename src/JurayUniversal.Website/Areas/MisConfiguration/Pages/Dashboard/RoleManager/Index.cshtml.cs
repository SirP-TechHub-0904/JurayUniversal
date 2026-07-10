using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.RoleManager
{
    [Authorize(Roles = "mSuperAdmin,Admin,Administrator")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;

        public IndexModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IList<AppRole> Roles { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Base System Roles
            var baseRoles = new List<(string Name, JurayUniversal.Domain.Enum.Enum.RoleCategory Category)>
            {
                ("mSuperAdmin", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("SuperAdmin", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("Admin", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("SubAdmin", JurayUniversal.Domain.Enum.Enum.RoleCategory.Legacy),
                ("User", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("Manager", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("Finance", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("Security", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("Editor", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both),
                ("Staff", JurayUniversal.Domain.Enum.Enum.RoleCategory.Both)
            };

            foreach (var role in baseRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new AppRole { Name = role.Name, RoleCategory = role.Category });
                }
            }

            // 2. MIS Feature Roles (18 active features)
            var activeMisFeatures = new List<string>
            {
                "MisAlumni", "MisConfiguration", "MisFinances", "MisFleet", 
                "MisForms", "MisGrants", "MisGroups", "MisManagement", 
                "MisManual", "MisMassComm", "MisNewsletter", "MisProjects", 
                "MisProposals", "MisRecruitment", "MisReports", "MisTicketing", 
                "MisUserManagement", "MisVoting"
            };

            foreach (var feature in activeMisFeatures)
            {
                var featureRoles = new[]
                {
                    $"{feature}_GlobalEditor",
                    $"{feature}_FeatureEditor",
                    $"{feature}_Readonly"
                };

                foreach (var fRole in featureRoles)
                {
                    if (!await _roleManager.RoleExistsAsync(fRole))
                    {
                        await _roleManager.CreateAsync(new AppRole { Name = fRole, RoleCategory = JurayUniversal.Domain.Enum.Enum.RoleCategory.MIS });
                    }
                }
            }

            // Finally, fetch the roles to display
            Roles = await _roleManager.Roles.ToListAsync();
        }
    }
}
