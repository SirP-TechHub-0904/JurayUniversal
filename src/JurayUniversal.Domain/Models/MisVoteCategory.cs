using System.Collections.Generic;

namespace JurayUniversal.Domain.Models
{
    public class MisVoteCategory
    {
        public long Id { get; set; }
        
        public long MisElectionId { get; set; }
        public MisElection MisElection { get; set; }
        
        public string Title { get; set; }
        public string? Description { get; set; }
        
        public int SortOrder { get; set; }

        public ICollection<MisCandidate> MisCandidates { get; set; }
        public ICollection<MisUserVote> MisUserVotes { get; set; }
    }
}
