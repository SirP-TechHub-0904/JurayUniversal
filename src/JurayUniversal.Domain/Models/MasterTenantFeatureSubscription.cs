using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// Entity tracking modular feature access granted to a specific tenant workspace.
    /// </summary>
    [Table("MasterTenantFeatureSubscriptions")]
    public class MasterTenantFeatureSubscription
    {
        [Key]
        public int Id { get; set; }

        public int MasterTenantCatalogId { get; set; }

        [ForeignKey("MasterTenantCatalogId")]
        public virtual MasterTenantCatalog MasterTenantCatalog { get; set; } = null!;

        /// <summary>
        /// Feature module folder key (e.g. "MisAlumni", "MisFinances", "MisFleet", "MisVoting").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FeatureKey { get; set; } = null!;

        /// <summary>
        /// Feature display name.
        /// </summary>
        [MaxLength(200)]
        public string FeatureName { get; set; } = string.Empty;

        /// <summary>
        /// Flag indicating whether the feature is actively enabled for the tenant.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Timestamp when the feature was enabled.
        /// </summary>
        public DateTime EnabledAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional administrator user who granted the feature.
        /// </summary>
        [MaxLength(100)]
        public string? GrantedBy { get; set; }
    }
}
