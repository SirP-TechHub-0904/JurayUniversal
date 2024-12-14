using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class Accomodation
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Disable { get; set; }
        public string Description { get; set; }

        public int? ParticipantListId { get; set; }
        public ParticipantList ParticipantList { get; set; }
    }
}
