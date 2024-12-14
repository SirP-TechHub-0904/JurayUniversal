using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class CompanyProgramCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public ICollection<CompanyProgram> CompanyPrograms { get; set; }
        public bool Publish { get; set; }


        [Display(Name = "Show In Menu")]
        public bool ShowInMenu { get; set; }

        [Display(Name = "Show In Dashboard")]
        public bool ShowInDashboard { get; set; }


    }
}
