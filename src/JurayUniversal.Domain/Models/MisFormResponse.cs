using System;

namespace JurayUniversal.Domain.Models
{
    public class MisFormResponse
    {
        public long Id { get; set; }
        public long MisFormId { get; set; }
        public MisForm MisForm { get; set; }

        public string UserProfileId { get; set; }
        public Profile UserProfile { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        public string ResponsesJson { get; set; }
    }
}
