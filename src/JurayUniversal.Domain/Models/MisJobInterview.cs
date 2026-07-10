using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisJobInterview
    {
        public long Id { get; set; }

        public long MisJobApplicantId { get; set; }
        public MisJobApplicant? MisJobApplicant { get; set; }

        public DateTime InterviewDate { get; set; }

        public string? LocationOrLink { get; set; }

        public Enum.Enum.InterviewType InterviewType { get; set; } = Enum.Enum.InterviewType.OnlineVideo;

        [Range(0, 100)]
        public int? Score { get; set; }

        public string? AdminFeedback { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
