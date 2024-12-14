using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class Facility
    {
        public long Id { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Ward { get; set; }
        public string? FacilityLocation { get; set; }

    }
}
