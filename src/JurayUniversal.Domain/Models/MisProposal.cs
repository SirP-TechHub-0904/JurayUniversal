using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProposal
    {
        public long Id { get; set; }

        public string? UserId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("UserId")]
        public Profile? UserProfile { get; set; }

        [Required, StringLength(300)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Note { get; set; }

        public string? TypedProposal { get; set; }

        public Enum.Enum.ProposalStatus Status { get; set; } = Enum.Enum.ProposalStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }

        public ICollection<MisProposalFile> ProposalFiles { get; set; } = new List<MisProposalFile>();
        
        // Link to resulting grant if approved
        public MisGrant? LinkedGrant { get; set; }

        public string? ExecutionDuration { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }

        public ICollection<MisProposalPhase> Phases { get; set; } = new List<MisProposalPhase>();
    }
}
