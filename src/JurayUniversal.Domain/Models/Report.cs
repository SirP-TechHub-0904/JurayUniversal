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
    }
}
