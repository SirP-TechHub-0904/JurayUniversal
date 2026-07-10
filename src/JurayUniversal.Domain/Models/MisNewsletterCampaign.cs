using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayUniversal.Domain.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterCampaign
    {
        public long Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public Profile? User { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Display(Name = "HTML Body")]
        public string HtmlBody { get; set; } = string.Empty;

        public Enum.Enum.MisNewsletterCampaignStatus Status { get; set; } = Enum.Enum.MisNewsletterCampaignStatus.Draft;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Scheduled Date")]
        public DateTime? ScheduledDate { get; set; }

        [Display(Name = "Sent At")]
        public DateTime? SentAt { get; set; }

        [Display(Name = "Total Recipients")]
        public int TotalRecipients { get; set; }

        public ICollection<MisNewsletterRecipient>? Recipients { get; set; }
    }
}
