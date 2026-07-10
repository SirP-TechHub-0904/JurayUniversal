using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterPlan
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Plan Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Price / Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Email Quota")]
        public int Quota { get; set; }

        [Display(Name = "Duration (in Months)")]
        public int DurationInMonths { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
