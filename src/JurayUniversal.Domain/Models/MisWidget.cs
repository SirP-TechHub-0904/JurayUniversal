using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MisWidget
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Widget Code")]
        public string WidgetCode { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Column Span")]
        public int ColSpan { get; set; } // 3, 4, 6, 8, 12
    }
}
