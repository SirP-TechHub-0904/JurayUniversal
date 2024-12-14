using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MeetingAttendance
    {
        public long Id {get; set; }
        public string? UserId { get; set; }
        public Profile User { get; set; }

        public long MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public DateTime Time { get; set; }
    }
}
