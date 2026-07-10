using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisJobApplicantDocument
    {
        public long Id { get; set; }

        public long MisJobApplicantId { get; set; }
        public MisJobApplicant? MisJobApplicant { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string FileUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
