using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayUniversal.Domain.Models
{
    public class MisJobApplicant
    {
        public long Id { get; set; }

        public long MisJobPostingId { get; set; }
        public MisJobPosting? MisJobPosting { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        public string? ProfessionalSummary { get; set; }
        public string? WorkExperience { get; set; }
        // Link to the Applicant's isolated account
        public long MisApplicantAccountId { get; set; }
        public MisApplicantAccount MisApplicantAccount { get; set; }

        public string? Education { get; set; }
        public string? Skills { get; set; }

        public string? PassportFileUrl { get; set; }
        public string? ResumeFileUrl { get; set; }

        public Enum.Enum.ApplicationStatus Status { get; set; } = Enum.Enum.ApplicationStatus.Applied;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisJobInterview> Interviews { get; set; } = new List<MisJobInterview>();
        public ICollection<MisJobApplicantDocument> Documents { get; set; } = new List<MisJobApplicantDocument>();
    }
}
