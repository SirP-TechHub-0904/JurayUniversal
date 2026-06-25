using System;

namespace JurayUniversal.Domain.Models
{
    public class MisReportTimeLog
    {
        public long Id { get; set; }
        public long MisReportSubmissionId { get; set; }
        public MisReportSubmission MisReportSubmission { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string ActivityDescription { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsBreak { get; set; }
    }
}
