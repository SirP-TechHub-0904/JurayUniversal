using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisReportParticipant
    {
        public long Id { get; set; }

        public long MisReportSchemaId { get; set; }
        public MisReportSchema MisReportSchema { get; set; }

        [Required]
        public string UserProfileId { get; set; }
        public Profile UserProfile { get; set; }
    }
}
