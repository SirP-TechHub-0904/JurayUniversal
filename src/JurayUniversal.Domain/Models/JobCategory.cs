using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class JobCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public bool Disable { get; set; }

        public ICollection<Plan> Plans { get; set; }
        public ICollection<Job> Jobs { get; set; }

         
    }
}
