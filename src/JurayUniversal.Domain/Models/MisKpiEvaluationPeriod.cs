using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// MIS: Defines an Evaluation Period created and managed by administrators.
    /// e.g., "Q3 2026", "2026 Mid-Year Audit", "Special Sales Blitz"
    /// </summary>
    public class MisKpiEvaluationPeriod
    {
        public long Id { get; set; }

        [Display(Name = "Period Title")]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
