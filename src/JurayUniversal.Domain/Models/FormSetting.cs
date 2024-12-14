using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class FormSetting
    {
        public long Id { get; set; }

        public bool EnableCV { get; set; }
        public bool PositionOrRank { get; set; }
        public bool PreviousAppointment { get; set; }
        public bool InternationalPassportNumber { get; set; }
        public bool InternationalPassportExpiringDate { get; set; }
        public bool Sponsor { get; set; }
        public bool SponsorAddress { get; set; }

    }
}
