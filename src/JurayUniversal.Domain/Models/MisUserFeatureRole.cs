using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// MIS: The Permission Matrix table — the core of the Role-Based Access Control system.
    /// Each row grants a specific user access to one Feature Area at a specific action level.
    ///
    /// Examples:
    ///   - ProfileId: "John" | FeatureAreaKey: "Elections"  | ActionRole: Readonly
    ///   - ProfileId: "John" | FeatureAreaKey: "Ticketing"  | ActionRole: FeatureEditor
    ///   - ProfileId: "Jane" | FeatureAreaKey: "Ticketing"  | ActionRole: GlobalEditor
    ///
    /// A user can have multiple rows — one per feature they are assigned to.
    /// GlobalEditor overrides all feature-specific restrictions.
    /// </summary>
    public class MisUserFeatureRole
    {
        public long Id { get; set; }

        // The user receiving the role
        [Display(Name = "User")]
        [Required]
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }

        // The Area key this role applies to. Must match the Area folder name exactly.
        // e.g., "Elections", "Ticketing", "Fleet", "Recruitment", "UserManagement"
        [Display(Name = "Feature Area Key")]
        [Required]
        public string FeatureAreaKey { get; set; }

        [Display(Name = "Action Role")]
        public MisActionRole ActionRole { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Who granted this role
        [Display(Name = "Assigned By")]
        public string? AssignedBy { get; set; } // ProfileId of the admin

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
