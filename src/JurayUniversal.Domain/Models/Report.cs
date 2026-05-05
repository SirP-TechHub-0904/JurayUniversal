using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class Report
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Profile User { get; set; }
        public string? Achievement { get; set; }
        [Display(Name = "Plans For Next Day")]
        public string? PlansForNextDay { get; set; }
        [Display(Name = "Issues And Challenges")]
        public string? IssuesAndChallenges { get; set; }
        [Display(Name = "Task Note")]
        public string? TaskNote { get; set; }
        [Display(Name = "Task Status")]
        public TaskNoteStatus TaskNoteStatus { get; set; }
        [Display(Name = "Report Status")]
        public ReportStatus ReportStatus { get; set; }

        [Display(Name = "Manager Report")]
        public string? ManagerReport { get; set; }
        [Display(Name = "Admin Note")]
        public string? AdminNote { get; set; }

        [Display(Name = "Report Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Report Log")]
        public string? Log { get; set; }
        [Display(Name = "Report Period Status")]
        public ReportPeriodStatus ReportPeriodStatus { get; set; }
        [Display(Name = "Verification Status")]
        public VerificationStatus VerificationStatus { get; set; }


        [Display(Name = "Impact")]
        public string? Impact { get; set; }

        [Display(Name = "Time 8am - 10am")]
        public string? Time8to10 { get; set; }

        [Display(Name = "Time 10am - 12pm")]
        public string? Time10to12 { get; set; } = null;
        [Display(Name = "Time 1pm - 3pm")]
        public string? Time1to3 { get; set; }

        [Display(Name = "Time 3pm - 5pm")]
        public string? Time3to5 { get; set; }

    }
}
