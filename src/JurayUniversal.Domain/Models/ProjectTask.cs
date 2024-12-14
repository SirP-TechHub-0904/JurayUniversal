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
    public class ProjectTask
    {
        public long Id { get; set; }
        [Display(Name = "Feature")]
        public long ProjectFeatureId { get; set; }
        public ProjectFeature ProjectFeature { get; set; }
        [Display(Name = "Date Start")]
        public DateTime? DateStarted { get; set; }
        [Display(Name = "Date Finised")]
        public DateTime? DateFinised { get; set; }
        public string? Report { get; set; }
        public string? Challenges { get; set; }
        public int Percentage { get; set; }
        public decimal Cost { get; set; }

        [Display(Name = "Task Type")]
        public TaskType TaskType { get;set;}

         [Display(Name = "Task Type")]
        public ProjectTaskStatus ProjectTaskStatus { get;set;}
        [Display(Name = "Profile")]
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
