using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class OfficeActivityDialy
    {
        public long Id { get; set; }
         [Display(Name = "Office Activity")]
        public long OfficeActivityCategoryId { get;set; }
        public OfficeActivityCategory OfficeActivityCategory { get;set; }

        public string? Note { get;set;}
        public int Customer { get;set;}
        public decimal Amount { get; set;}
        public DateTime Date { get;set;}
         [Display(Name = "Last Update")]
        public DateTime LastUpdate { get;set;}
         [Display(Name = "Last Update By")]
        public string? LastUpdateById { get;set;} 
        public Profile LastUpdateBy { get;set;}
        public string? Logs { get;set;}
         [Display(Name = "Update Count")]
        public int UpdateCount { get;set;}

    }
}
