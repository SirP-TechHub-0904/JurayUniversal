using System;
using System.ComponentModel.DataAnnotations;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisFeature
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Feature Name")]
        public string FeatureName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Module / Route Key")]
        public string ModuleFolder { get; set; } = string.Empty;

        [Display(Name = "Cost Per Duration")]
        public decimal CostPerDuration { get; set; }

        [Display(Name = "Billing Duration")]
        public FeatureBillingDuration BillingDuration { get; set; }

        [Display(Name = "Grace Period (Days)")]
        public int GracePeriodDays { get; set; }

        [Display(Name = "Subscription Status")]
        public FeatureBillingStatus Status { get; set; }

        [Display(Name = "Next Billing Date")]
        public DateTime? NextBillingDate { get; set; }
    }
}
