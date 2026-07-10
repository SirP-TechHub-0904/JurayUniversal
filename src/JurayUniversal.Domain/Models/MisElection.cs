using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisElection
    {
        public long Id { get; set; }
        
        [Required]
        [Display(Name = "Election Title")]
        public string Title { get; set; }
        
        public string? ShortCode { get; set; }
        
        [Display(Name = "Instructions for Voters")]
        public string? Instructions { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Election Type")]
        public MisElectionType ElectionType { get; set; }

        [Display(Name = "Status")]
        public MisVotingStatus VotingStatus { get; set; }

        [Display(Name = "Publish Live Results")]
        public bool ShowResults { get; set; }
        
        [Display(Name = "Enable Candidate Images")]
        public bool EnableCandidateImages { get; set; }
        
        public bool RequireAllCategories { get; set; } // Enforce voting in all categories

        public string? ElectionBannerUrl { get; set; }
        public string? ElectionBannerKey { get; set; }

        public ICollection<MisVoteCategory> MisVoteCategories { get; set; }
        
        [InverseProperty("MisElection")]
        public ICollection<MisElectionVoter> MisElectionVoters { get; set; }
    }
}
