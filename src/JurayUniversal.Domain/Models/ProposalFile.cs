using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class ProposalFile
    {
        public long Id { get; set; }
        public string Title { get; set; }
       public long ProposalId { get; set; }
       public Proposal Proposal { get; set; }

        public string? Url { get; set; }
        public string? Key { get; set; }
    }
}
