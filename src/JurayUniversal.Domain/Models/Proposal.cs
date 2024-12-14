using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class Proposal
    {
        public long Id { get; set; }
        public string SubmittedById { get; set; }
        public Profile SubmittedBy { get; set; }
        public ProposalStatus ProposalStatus { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public string? TypedProposal { get; set; }

        public ICollection<ProposalFile> ProposalFile { get; set; }
    }
}
