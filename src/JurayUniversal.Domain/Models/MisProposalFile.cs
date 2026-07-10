using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProposalFile
    {
        public long Id { get; set; }

        public long MisProposalId { get; set; }
        public MisProposal MisProposal { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        public string? Key { get; set; }
    }
}
