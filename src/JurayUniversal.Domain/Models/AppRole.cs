using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Domain.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }

        /// <summary>
        /// Indicates where this role should be visible and usable.
        /// </summary>
        public Enum.Enum.RoleCategory RoleCategory { get; set; } = Enum.Enum.RoleCategory.Both;
    }
}
