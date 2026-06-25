using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisReportSubmission
    {
        public long Id { get; set; }

        public long MisReportSchemaId { get; set; }
        public MisReportSchema MisReportSchema { get; set; }

        [Required]
        public string UserProfileId { get; set; }
        public Profile UserProfile { get; set; }

        // Core Content (Optional now since template is dynamic)
        public string? Content { get; set; }

        // Structured Report Data
        public string? TaskAssigned { get; set; }
        public string? Achievements { get; set; }
        public string? Challenges { get; set; }
        public string? Impact { get; set; }
        public string? PlansForNextPeriod { get; set; }
        public string? AttachmentUrl { get; set; }

        // Evaluation
        public string? ManagerComment { get; set; }
        public string? AdminComment { get; set; }

        public DateTime PeriodDate { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } // "OnTime", "Late"

        public ICollection<MisReportTimeLog> TimeLogs { get; set; }
    }
}
