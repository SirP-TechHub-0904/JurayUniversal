using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class CompanyProgram
    {
        public long Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }         

        public long? CompanyProgramCategoryId { get; set; }
        public CompanyProgramCategory CompanyProgramCategory { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        //[Display(Name = "Program User")]
        //public ICollection<ProgramUser> ProgramUsers { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "State")]
        public string? State { get; set; }
         

        [Display(Name = "Show")]
        public bool Show { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }
         
    }
}
