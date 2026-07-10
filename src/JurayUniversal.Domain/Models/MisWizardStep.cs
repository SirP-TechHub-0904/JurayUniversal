using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MisWizardStep
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Feature Name")]
        public string FeatureName { get; set; } // e.g., MisTicketing

        [Display(Name = "Step Order")]
        public int StepOrder { get; set; }

        [StringLength(100)]
        [Display(Name = "Target Element")]
        public string? TargetElement { get; set; } // e.g., #btnCreateTicket

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
