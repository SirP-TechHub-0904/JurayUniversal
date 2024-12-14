using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class Evoting
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Instructions { get; set; }

        [Display(Name = "Begin Date")]

        public DateTime DateToStart { get; set; }
        [Display(Name = "End Date")]

        public DateTime EndDate { get; set; }
        [Display(Name = "Voting Status")]

        public VotingStatus VotingStatus { get; set; }
        [Display(Name = "Start Voting")]

        public bool StartVoting { get; set; }
        public bool ShowResult { get; set; }


        public long CourseId { get; set; }
        public Course Course { get; set; }

        [Display(Name = "Vote Categories")]

        public ICollection<VoteCategory> VoteCategories { get; set; }

        
    }
}
