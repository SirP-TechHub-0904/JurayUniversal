using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisCandidate
    {
        public long Id { get; set; }

        public long MisVoteCategoryId { get; set; }
        public MisVoteCategory MisVoteCategory { get; set; }

        public string? ProfileId { get; set; }
        public Profile Profile { get; set; }

        [Required]
        [Display(Name = "Candidate Name (Title)")]
        public string Title { get; set; }

        public string? CandidateImageUrl { get; set; }
        public string? CandidateImageKey { get; set; }

        [Display(Name = "Short Profile")]
        public string? ShortProfile { get; set; }
        
        public string? Manifesto { get; set; }

        public ICollection<MisUserVote> MisUserVotes { get; set; }
    }
}
