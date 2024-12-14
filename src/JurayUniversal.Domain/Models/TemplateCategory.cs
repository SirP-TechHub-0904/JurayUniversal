using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class TemplateCategory
    {
        public long Id { get; set; }

        [Display(Name = "Layout")]
        [Required]
        public string Layout { get; set; }

        [Display(Name = "Login Layout")]
        [Required]
        public string LoginLayout { get; set; }


        [Display(Name = "Template Name")]
        [Required]
        public string TemplateName { get; set; }


        [Display(Name = "Template Color Path")]
        [Required]
        public string TemplateColorPath { get; set; }
        public string UniqueKey { get; set; }


        public ICollection<TemplateType> TemplateTypes { get; set; }

    }
}
