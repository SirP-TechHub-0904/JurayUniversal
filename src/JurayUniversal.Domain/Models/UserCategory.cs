using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class UserCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Profile> Profiles { get; set; }

        public bool Publish { get; set; }


        [Display(Name = "Show In Menu")]
        public bool ShowInMenu { get; set; }

        [Display(Name = "Show In Dashboard")]
        public bool ShowInDashboard { get; set; }

    }
}
