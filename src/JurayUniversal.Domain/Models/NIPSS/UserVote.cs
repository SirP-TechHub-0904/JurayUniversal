using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class UserVote
    {
        public long Id { get; set; }
        public long? VoteCategoryId { get; set; }
        public VoteCategory VoteCategory { get; set; }
        public long? VoteCondidateId {  get; set; }
        public VoteCondidate VoteCondidate { get; set; }
        public string? EncryptCode { get; set; }
        public string? VoterUserId { get; set; }
        public Profile VoterUser {  get; set; }

        public UserVoteStatus UserVoteStatus { get; set; }

    }
}
