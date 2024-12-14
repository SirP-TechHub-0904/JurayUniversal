using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class ProjectCategory
    {
         public long Id { get;set;}
        public string Title { get;set;}
        public string Description { get;set;}
        [Display(Name = "Projects")]
        public ICollection<ProjectMain> ProjectMains { get; set;}
    }
}
