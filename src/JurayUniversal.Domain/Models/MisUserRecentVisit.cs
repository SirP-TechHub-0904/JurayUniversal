using System;

namespace JurayUniversal.Domain.Models
{
    public class MisUserRecentVisit
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Profile User { get; set; }
        public string ModuleFolder { get; set; }
        public DateTime LastVisitedAt { get; set; }
    }
}
