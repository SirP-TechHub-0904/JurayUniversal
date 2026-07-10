using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class VoterRegister
    {
        public long Id { get; set; }

        public long? EvotingId { get; set; }
        public Evoting Evoting { get; set; }

        public string? UserId { get; set; }
        public Profile User { get; set; }

        public bool HasVoted { get; set; }
    }
}
