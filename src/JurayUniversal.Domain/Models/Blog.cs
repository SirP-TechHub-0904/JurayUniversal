using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class Blog
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoKey { get; set; }

        public bool Publish { get; set; }
        public int SortOrder { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageKey { get; set; }
        public long? BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Full Description")]
        public string FullDescription { get; set; }

        [Display(Name = "Add To Ribon")]
        public bool AddToRibon { get; set; }

        [Display(Name = "Ribon Custom Display Title (if you want a title that will override the main title in the ribon.)")]
        public string? RibonCustomDisplayTitle { get; set; }

        [Display(Name = "Ribon Sort Order")]
        public int RibonSortOrder { get; set; }
    }
}
