using System;

namespace JurayUniversal.Domain.Models
{
    public class MisUserVote
    {
        public long Id { get; set; }

        public long MisVoteCategoryId { get; set; }
        public MisVoteCategory MisVoteCategory { get; set; }

        public long MisCandidateId { get; set; }
        public MisCandidate MisCandidate { get; set; }

        public string? EncryptCode { get; set; }
        public DateTime DateCast { get; set; }
    }
}
