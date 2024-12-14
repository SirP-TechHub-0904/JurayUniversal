using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class VoteCategory
    {
        public long Id { get; set; }
        public long? EvotingId { get; set; }
        public Evoting Evoting { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }

        public ICollection<VoteCondidate> VoteCondidates { get; set; }
        public ICollection<UserVote> UserVotes { get; set; }
    }
}
