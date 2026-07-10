using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterEmailLog
    {
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public string Subject { get; set; }

        public string MailBody { get; set; }

        public DateTime SentDatetime { get; set; } = DateTime.UtcNow;

        public DateTime? ReceivedDatetime { get; set; }

        public JurayUniversal.Domain.Enum.Enum.EmailLogStatus Status { get; set; } = JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Pending;

        public long? CampaignId { get; set; }
        public MisNewsletterCampaign? Campaign { get; set; }

        public int RetryCount { get; set; } = 0;

        public string? ErrorMessage { get; set; }
    }
}
