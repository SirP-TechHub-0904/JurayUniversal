using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisJobPosting
    {
        public long Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? AmountOrSalary { get; set; }

        public string? RequiredSkills { get; set; }

        public string? Benefits { get; set; }

        public Enum.Enum.JobType JobType { get; set; } = Enum.Enum.JobType.FullTime;

        public Enum.Enum.JobStatus Status { get; set; } = Enum.Enum.JobStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ClosingDate { get; set; }

        public ICollection<MisJobApplicant> Applicants { get; set; } = new List<MisJobApplicant>();
    }
}
