using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class ProductSample
    {
        public long Id { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoKey { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageKey { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Full Description")]
        public string? FullDescription { get; set; }

        public long? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
