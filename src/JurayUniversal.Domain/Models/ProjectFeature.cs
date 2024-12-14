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
    public class ProjectFeature
    {
         public long Id { get;set;}
        [Display(Name = "Feature")]
        public long GeneralFeatureId { get;set;}
        public GeneralFeature GeneralFeature { get;set;} 
        public decimal Cost { get;set;}
        public bool UseFeaturesCost { get;set;}
        public string? Report { get;set;}
        public string? Challenges { get;set;}
        [Display(Name = "Date Started")]
        public DateTime? DateStarted { get; set; }
        [Display(Name = "Date Finished")]
        public DateTime? DateFinised { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Projects")]
           public long ProjectMainId { get; set; }
        public ProjectMain ProjectMain { get; set; }
         [Display(Name = "Feature Status")]
        public ProjectFeatureStatus ProjectFeatureStatus { get;set;}
        [Display(Name = "Tasks")]
        public ICollection<ProjectTask> ProjectTasks { get; set; }
    }
}
