using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class ProjectTeam
    {
        public long Id { get; set; } 
        [Display(Name = "Project")]
         public long ProjectMainId { get; set; }
        public ProjectMain ProjectMain { get; set; }
         [Display(Name = "Title")]
        public string Title { get;set;}
       [Display(Name = "Description")]
        public string Description { get;set;}
        [Display(Name = "Date")]
        public DateTime? Date { get;set;}

        public ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
