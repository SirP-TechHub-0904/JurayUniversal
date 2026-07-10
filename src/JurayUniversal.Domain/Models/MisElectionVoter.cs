namespace JurayUniversal.Domain.Models
{
    public class MisElectionVoter
    {
        public long Id { get; set; }

        public long MisElectionId { get; set; }
        public MisElection MisElection { get; set; }

        public string UserId { get; set; }
        public Profile User { get; set; }

        public bool IsAccredited { get; set; }
        public bool HasVoted { get; set; }
    }
}
