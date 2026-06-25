using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisSetting
    {
        public long Id { get; set; }
        
        [Display(Name = "Category Alias")]
        public string? CategoryAlias { get; set; }

        [Display(Name = "Sub-Category Alias")]
        public string? SubCategoryAlias { get; set; }

        [Display(Name = "Enable List Download")]
        public bool EnableListDownload { get; set; }

        [Display(Name = "Group/Committee Alias")]
        public string? GroupAlias { get; set; }

        [Display(Name = "Alumni/Past Member Alias")]
        public string? AlumniAlias { get; set; } = "Alumni";
    }
}
