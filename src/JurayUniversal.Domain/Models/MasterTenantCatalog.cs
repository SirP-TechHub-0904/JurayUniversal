using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// Master Catalog entity tracking registered tenant organizations, connection strings, domains, and lifecycle state.
    /// </summary>
    [Table("MasterTenantCatalogs")]
    public class MasterTenantCatalog
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Unique tenant key / subdomain identifier (e.g. "urualla", "globalcorp").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TenantKey { get; set; } = null!;

        /// <summary>
        /// Organization or business name.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string OrganizationName { get; set; } = null!;

        /// <summary>
        /// Default platform subdomain (e.g. "urualla.jurayuniversal.com").
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string PrimaryDomain { get; set; } = null!;

        /// <summary>
        /// Optional custom domain owned by the tenant (e.g. "www.uruallaclan.com").
        /// </summary>
        [MaxLength(255)]
        public string? CustomDomain { get; set; }

        /// <summary>
        /// API endpoint URL.
        /// </summary>
        [MaxLength(255)]
        public string? ApiUrl { get; set; }

        /// <summary>
        /// Website URL for the tenant portal.
        /// </summary>
        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }

        /// <summary>
        /// Dedicated SQL Server database name (e.g. "JurayUniversal_Tenant_urualla").
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string DatabaseName { get; set; } = null!;

        /// <summary>
        /// Full connection string for this tenant's dedicated database.
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string ConnectionString { get; set; } = null!;

        /// <summary>
        /// Primary contact / administrator email address.
        /// </summary>
        [MaxLength(255)]
        public string? AdminEmail { get; set; }

        /// <summary>
        /// Contact phone number.
        /// </summary>
        [MaxLength(50)]
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Industry sector / organization category.
        /// </summary>
        [MaxLength(100)]
        public string? Sector { get; set; }

        /// <summary>
        /// Flag indicating whether the tenant workspace is currently active and reachable.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Timestamp when the tenant database was initially provisioned.
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when tenant metadata was last modified.
        /// </summary>
        public DateTime? UpdatedAtUtc { get; set; }

        /// <summary>
        /// Optional notes entered by the central super administrator.
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

        /// <summary>
        /// Active feature module subscriptions for this tenant.
        /// </summary>
        public virtual ICollection<MasterTenantFeatureSubscription> FeatureSubscriptions { get; set; } = new List<MasterTenantFeatureSubscription>();
    }
}
