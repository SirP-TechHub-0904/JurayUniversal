using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class ProjectFile
    {
        public long Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "File")]
        public string FileUrl { get; set; }
        public string FileKey { get; set; }

        public long ProjectMainId { get; set; }
        public ProjectMain ProjectMain { get; set; }

    }
}
