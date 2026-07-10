using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisGrant
    {
        public long Id { get; set; }

        // Optional link to the proposal that triggered this grant
        public long? MisProposalId { get; set; }
        public MisProposal? MisProposal { get; set; }

        [Required, StringLength(300)]
        public string Title { get; set; } // The name of the funded project

        public long MisDonorId { get; set; }
        public MisDonor MisDonor { get; set; }

        public decimal TotalAwardAmount { get; set; }

        [StringLength(10)]
        public string Currency { get; set; } = "NGN";

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisGrantMilestone> Milestones { get; set; } = new List<MisGrantMilestone>();
    }
}
