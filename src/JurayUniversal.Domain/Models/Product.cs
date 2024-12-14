using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class Product
    {
        public long Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Mini Title")]
        public string? MiniTitle { get; set; }


        public long? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Disable Amount")]
        public bool DisableAmount { get; set; }

        [Display(Name = "Show Samples")]
        public bool ShowSamples { get; set; }


        [Display(Name = "Status Note")]
        public string? StatusNote { get; set; }

        [Display(Name = "Button Text")]
        public string? ButtonText { get; set; }

        public string? VideoUrl { get; set; }
        public string? VideoKey { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageKey { get; set; }

        [Display(Name = "Full Description")]
        public string? FullDescription { get; set; }

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<ProductSample> ProductSamples { get; set; }


        [Display(Name = "Show In Dropdown")]
        public bool ShowInDropdown { get; set; }
         
        [Display(Name = "Show In Home")]
        public bool ShowInHome { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Title Background Image")]
        public string? TitleBackgroundUrl { get; set; }
        public string? TitleBackgroundKey { get; set; }
    }
}
