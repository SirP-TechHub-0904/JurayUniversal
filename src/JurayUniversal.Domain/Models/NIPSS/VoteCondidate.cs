using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class VoteCondidate
    {
        public long Id { get; set; }

        public long? VoteCategoryId { get; set; }
        public VoteCategory VoteCategory { get; set; }

        public string? CandidateId { get; set; }
        public Profile Candidate { get; set; }
        public DateTime Date { get; set; }
        public string? Title { get; set; }
        public int ResultPosition { get; set; }

        public string? CandidateImageUrl { get; set; }
        public string? CandidateImageKey { get; set; }
        [Display(Name = "Short Profile")]

        public string? ShortProfile { get; set; }
        public string? Manifesto { get; set; }

        public ICollection<UserVote> UserVote { get; set; }
    }
}
