using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class ProductFeature
    {
        public long Id { get; set; }
         
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Display(Name = "Icon Text")]
        public string? IconText { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public long? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
