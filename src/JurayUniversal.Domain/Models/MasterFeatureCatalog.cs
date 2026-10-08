using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// Master repository of all available MIS features/modules that can be provisioned to tenants.
    /// </summary>
    [Table("MasterFeatureCatalogs")]
    public class MasterFeatureCatalog
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Unique module key matching Area folder name (e.g. "MisAlumni", "MisFinances", "MisFleet", "MisVoting").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FeatureKey { get; set; } = null!;

        /// <summary>
        /// User-friendly name of the feature (e.g. "Alumni & Network Directory").
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Detailed description of the feature module's capabilities.
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Logical category (e.g. "Core & Governance", "Financials & Grants", "Operations & Logistics", "Engagement & Surveys").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = "General";

        /// <summary>
        /// Bootstrap icon or FontAwesome icon class (e.g. "bi-people-fill").
        /// </summary>
        [MaxLength(100)]
        public string IconClass { get; set; } = "bi-app-indicator";

        /// <summary>
        /// Flag indicating whether the feature is compulsory/core and always active on all workspaces.
        /// </summary>
        public bool IsCompulsory { get; set; } = false;

        /// <summary>
        /// Flag indicating whether the module is globally active in the master platform.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Sort order for display.
        /// </summary>
        public int SortOrder { get; set; } = 0;
    }
}
