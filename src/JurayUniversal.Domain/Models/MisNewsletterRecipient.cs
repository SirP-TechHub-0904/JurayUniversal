using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayUniversal.Domain.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterRecipient
    {
        public long Id { get; set; }

        public long MisNewsletterCampaignId { get; set; }
        public MisNewsletterCampaign? MisNewsletterCampaign { get; set; }

        public string UserId { get; set; } = string.Empty;
        public Profile? User { get; set; }

        [Display(Name = "Contact Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        public Enum.Enum.MisNewsletterDeliveryStatus DeliveryStatus { get; set; } = Enum.Enum.MisNewsletterDeliveryStatus.Pending;

        [Display(Name = "Opened At")]
        public DateTime? OpenedAt { get; set; }

        public Guid TrackingGuid { get; set; } = Guid.NewGuid();
    }
}
