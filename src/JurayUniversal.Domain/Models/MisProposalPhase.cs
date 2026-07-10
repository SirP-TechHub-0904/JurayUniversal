using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProposalPhase
    {
        public long Id { get; set; }

        public long MisProposalId { get; set; }
        public MisProposal MisProposal { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public Enum.Enum.PhaseStatus Status { get; set; } = Enum.Enum.PhaseStatus.Upcoming;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Markdown or HTML report for this specific phase
        public string? ReportSummary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
