using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class XProjectTask
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
         
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Finish Date")]
        public DateTime FinishDate { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public Priority Priority { get; set; }
        public string? UserId { get; set; }
        public Profile User {  get; set; }
        
        
        public string? UserTaskNote { get; set; }
        public DateTime? UserDateFinished { get; set; }


        public int? RateUser { get; set; }
        public long XProjectSectionId { get; set; }
        public XProjectSection XProjectSection { get; set; }
        public ICollection<XProjectFile> Files { get;set;}

        public string TestCriterial { get; set; }
        public XProjectTaskStatus TestStatus { get; set; }
        public string? TestedById { get; set; }
        public Profile TestedBy { get; set; }

        [Display(Name = "Test Date")]
        public DateTime TestDate { get; set; }
        [Display(Name = "Test Finish Date")]
        public DateTime TestFinishDate { get; set; }
        public string? TesterTaskNote { get; set; }
        public DateTime? TesterDateFinished { get; set; }


        public int? RateTester { get; set; }

        public string? ApprovedById { get; set; }
        public Profile ApprovedBy { get; set; }

        public string? ApproveTaskNote { get; set; }
        public DateTime? ApprovedDateFinished { get; set; }


        public XProjectTaskStatus ApproveStatus { get; set; }
        public int? RateApprover { get; set; }

        public bool CloseTask { get; set; }
        public string? Log {  get; set; }
    }
}
